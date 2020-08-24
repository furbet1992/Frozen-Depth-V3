/*
    File name: Triplanar.shader
    Author: Michael Sweetman
    Summary: A Shader to project textures onto the front, right and top of a mesh without the need for UV
    Creation Date: 18/08/2020
    Last Modified: 19/08/2020
*/

Shader "Custom/Triplanar"
{
    Properties
    {
        // albedo texture to be projected on the top and bottom of the mesh
        _TopTexture ("Top Albedo", 2D) = "white" {}

        // albedo texture to be projected on the front and back of the mesh
        _FrontTexture("Front Albedo", 2D) = "white" {}

        // albedo texture to be projected on the left and right of the mesh
        _RightTexture("Right Albedo", 2D) = "white" {}

        // normal texture to be projected on the top and bottom of the mesh
        _TopNormal("Top Normal", 2D) = "bump" {}
        
        // normal texture to be projected on the front and back of the mesh
        _FrontNormal("Front Normal", 2D) = "bump" {}
        
        // normal texture to be projected on the left and right of the mesh
        _RightNormal ("Right Normal", 2D) = "bump" {}

        // normal texture to be projected on the top and bottom of the mesh
        _TopEmission("Top Emission", 2D) = "bump" {}

        // normal texture to be projected on the front and back of the mesh
        _FrontEmission("Front Emission", 2D) = "bump" {}

        // normal texture to be projected on the left and right of the mesh
        _RightEmission("Right Emission", 2D) = "bump" {}

        _Alpha("Alpha", Range(0,1)) = 1
    }
    SubShader
    {
        Tags
        {
            // Set RenderType to Transparent
            "RenderType" = "Transparent" 
            // Set Queue to Transparent
            "Queue" = "Transparent"
        }
        
        // Set Level of Detail to 200
        LOD 200

        CGPROGRAM
        // Define this shader as a surface shader
        // Call the surf function
        // Use a pPhysically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // define the three albedo texture properties as Sampler2D variables
        sampler2D _TopTexture;
        sampler2D _FrontTexture;
        sampler2D _RightTexture;

        // define the three normal texture properties as Sampler2D variables
        sampler2D _TopNormal;
        sampler2D _FrontNormal;
        sampler2D _RightNormal;

        // define the three emission texture properties as Sampler2D variables
        sampler2D _TopEmission;
        sampler2D _FrontEmission;
        sampler2D _RightEmission;

        fixed _Alpha;

        // get the World Normal and World Position from the mesh
        struct Input
        {
            float3 worldPos;
            float3 worldNormal; INTERNAL_DATA
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // get the absolute values of the normal in object space
            float3 absNormal = normalize(mul(unity_WorldToObject,float4(IN.worldNormal, 0.0)));
            //float3 absNormal = IN.worldNormal;
            absNormal.x = abs(absNormal.x);
            absNormal.y = abs(absNormal.y);
            absNormal.z = abs(absNormal.z);

            // get the object space position 
            float3 position = (mul(unity_WorldToObject, float4(IN.worldPos, 0.0)));

            // get the colour information from each texture
            fixed4 topColor   = tex2D(_TopTexture, float2(position.x, position.z));
            fixed4 frontColor = tex2D(_FrontTexture, float2(position.x, position.y));
            fixed4 rightColor = tex2D(_RightTexture, float2(position.y, position.z));

            // multiply the colours by the normal for that direction, add each colour together
            fixed4 color = topColor * absNormal.y + frontColor * absNormal.z + rightColor * absNormal.x;

            // get the normal information from each texture
            fixed3 topNormal = UnpackNormal(tex2D(_TopNormal, float2(position.x, position.z)));
            fixed3 frontNormal = UnpackNormal(tex2D(_FrontNormal, float2(position.x, position.y)));
            fixed3 rightNormal = UnpackNormal(tex2D(_RightNormal, float2(position.y, position.z)));

            // multiply the normals by the normal for that direction, add each normal together
            fixed3 normal = normalize(topNormal * absNormal.y + frontNormal *absNormal.z + rightNormal * absNormal.x);

            // get the emission information from each texture
            fixed4 topEmission = tex2D(_TopEmission, float2(position.x, position.z));
            fixed4 frontEmission = tex2D(_FrontEmission, float2(position.x, position.y));
            fixed4 rightEmission = tex2D(_RightEmission, float2(position.y, position.z));

            // multiply the emissions by the normal for that direction, add each emission together
            half4 emission = topEmission * absNormal.y + frontEmission * absNormal.z + rightEmission * absNormal.x;

            // set the albedo using the color's rgb
            o.Albedo = color;
            // set the normal using the normal's rgb
            //o.Normal = normal;
            // set the emission using the emission's rgb
            //o.Emission = emission.rgb;
            // set the alpha using the colors alpha
            o.Alpha = _Alpha;
        }
        ENDCG
    }
    // If this shader fails to run, use the default diffuse shader
    FallBack "Diffuse"
}
