/*
    File name: Triplanar.shader
    Author: Michael Sweetman
    Summary: A Shader to project textures onto the front, right and top of a mesh without the need for UV
    Creation Date: 18/08/2020
    Last Modified: 31/08/2020
*/

Shader "Custom/Triplanar"
{
    Properties
    {
        // albedo texture to be projected on the top and bottom of the mesh
        _TopAlbedo("Top Albedo", 2D) = "white" {}

        // albedo texture to be projected on the front and back of the mesh
        _FrontAlbedo("Front Albedo", 2D) = "white" {}

        // albedo texture to be projected on the left and right of the mesh
        _RightAlbedo("Right Albedo", 2D) = "white" {}

        // normal texture to be projected on the top and bottom of the mesh
        _TopNormal("Top Normal", 2D) = "bump" {}
        
        // normal texture to be projected on the front and back of the mesh
        _FrontNormal("Front Normal", 2D) = "bump" {}
        
        // normal texture to be projected on the left and right of the mesh
        _RightNormal("Right Normal", 2D) = "bump" {}

        // emission texture to be projected on the top and bottom of the mesh
        _TopEmission("Top Emission", 2D) = "black" {}

        // emission texture to be projected on the front and back of the mesh
        _FrontEmission("Front Emission", 2D) = "black" {}

        // emission texture to be projected on the left and right of the mesh
        _RightEmission("Right Emission", 2D) = "black" {}

        // metallic, smoothness and alpha textures to be projected on the top and bottom of the mesh
        _TopMetSmoAlp("Top Metallic(r), Smoothness(g) and Alpha(b)", 2D) = "blue" {}

        // metallic, smoothness and alpha textures to be projected on the front and back of the mesh
        _FrontMetSmoAlp("Front Metallic(r), Smoothness(g) and Alpha(b)", 2D) = "blue" {}

        // metallic, smoothness and alpha textures to be projected on the left and right of the mesh
        _RightMetSmoAlp("Right Metallic(r), Smoothness(g) and Alpha(b)", 2D) = "blue" {}
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
        // Use a Physically based Standard lighting model, enable shadows on all light types and support transparency
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // define the three albedo texture properties as Sampler2D variables
        sampler2D _TopAlbedo;
        sampler2D _FrontAlbedo;
        sampler2D _RightAlbedo;

        // define the three normal texture properties as Sampler2D variables
        sampler2D _TopNormal;
        sampler2D _FrontNormal;
        sampler2D _RightNormal;

        // define the three emission texture properties as Sampler2D variables
        sampler2D _TopEmission;
        sampler2D _FrontEmission;
        sampler2D _RightEmission;

        // define the three metallic, smoothness, alpha texture properties as Sampler2D variables
        sampler2D _TopMetSmoAlp;
        sampler2D _FrontMetSmoAlp;
        sampler2D _RightMetSmoAlp;

        // get the tiling and offset information from the albedo textures
        float4 _TopAlbedo_ST;
        float4 _FrontAlbedo_ST;
        float4 _RightAlbedo_ST;

        // get the tiling and offset information from the normal textures
        float4 _TopNormal_ST;
        float4 _FrontNormal_ST;
        float4 _RightNormal_ST;

        // get the tiling and offset information from the emission textures
        float4 _TopEmission_ST;
        float4 _FrontEmission_ST;
        float4 _RightEmission_ST;

        // get the tiling and offset information from the metallic, smoothness, alpha textures
        float4 _TopMetSmoAlp_ST;
        float4 _FrontMetSmoAlp_ST;
        float4 _RightMetSmoAlp_ST;

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
            float3 absNormal = WorldNormalVector(IN, o.Normal);
            absNormal = normalize(mul(unity_WorldToObject, absNormal));

            absNormal.x = abs(absNormal.x);
            absNormal.y = abs(absNormal.y);
            absNormal.z = abs(absNormal.z);

            // get the object space position 
            float3 position = mul(unity_WorldToObject, float4(IN.worldPos, 0.0));

            // get the albedo information from each texture
            fixed4   topAlbedo = tex2D(  _TopAlbedo, float2((position.x *   _TopAlbedo_ST.x) +   _TopAlbedo_ST.z, (position.z *   _TopAlbedo_ST.y) +   _TopAlbedo_ST.w));
            fixed4 frontAlbedo = tex2D(_FrontAlbedo, float2((position.x * _FrontAlbedo_ST.x) + _FrontAlbedo_ST.z, (position.y * _FrontAlbedo_ST.y) + _FrontAlbedo_ST.w));
            fixed4 rightAlbedo = tex2D(_RightAlbedo, float2((position.y * _RightAlbedo_ST.x) + _RightAlbedo_ST.z, (position.z * _RightAlbedo_ST.y) + _RightAlbedo_ST.w));

            // multiply the colours by the normal for that direction, add each colour together
            fixed4 color = topAlbedo * absNormal.y + frontAlbedo * absNormal.z + rightAlbedo * absNormal.x;

            // get the normal information from each texture
            fixed4   topNormal = tex2D(  _TopNormal, float2((position.x *   _TopNormal_ST.x) +   _TopNormal_ST.z, (position.z *   _TopNormal_ST.y) +   _TopNormal_ST.w));
            fixed4 frontNormal = tex2D(_FrontNormal, float2((position.x * _FrontNormal_ST.x) + _FrontNormal_ST.z, (position.y * _FrontNormal_ST.y) + _FrontNormal_ST.w));
            fixed4 rightNormal = tex2D(_RightNormal, float2((position.y * _RightNormal_ST.x) + _RightNormal_ST.z, (position.z * _RightNormal_ST.y) + _RightNormal_ST.w));

            // multiply the normals by the normal for that direction, add each normal together
            fixed3 normal = normalize(topNormal * absNormal.y + frontNormal *absNormal.z + rightNormal * absNormal.x);

            // get the emission information from each texture
            fixed4   topEmission = tex2D(  _TopEmission, float2((position.x *   _TopEmission_ST.x) +   _TopEmission_ST.z, (position.z *   _TopEmission_ST.y) +   _TopEmission_ST.w));
            fixed4 frontEmission = tex2D(_FrontEmission, float2((position.x * _FrontEmission_ST.x) + _FrontEmission_ST.z, (position.y * _FrontEmission_ST.y) + _FrontEmission_ST.w));
            fixed4 rightEmission = tex2D(_RightEmission, float2((position.y * _RightEmission_ST.x) + _RightEmission_ST.z, (position.z * _RightEmission_ST.y) + _RightEmission_ST.w));

            // multiply the emissions by the normal for that direction, add each emission together
            half4 emission = topEmission * absNormal.y + frontEmission * absNormal.z + rightEmission * absNormal.x;

            // get the Metallic, Smoothness and Alpha information from each texture
            fixed4   topMetSmoAlp = tex2D(  _TopMetSmoAlp, float2((position.x *   _TopMetSmoAlp_ST.x) +   _TopMetSmoAlp_ST.z, (position.z *   _TopMetSmoAlp_ST.y) +   _TopMetSmoAlp_ST.w));
            fixed4 frontMetSmoAlp = tex2D(_FrontMetSmoAlp, float2((position.x * _FrontMetSmoAlp_ST.x) + _FrontMetSmoAlp_ST.z, (position.y * _FrontMetSmoAlp_ST.y) + _FrontMetSmoAlp_ST.w));
            fixed4 rightMetSmoAlp = tex2D(_RightMetSmoAlp, float2((position.y * _RightMetSmoAlp_ST.x) + _RightMetSmoAlp_ST.z, (position.z * _RightMetSmoAlp_ST.y) + _RightMetSmoAlp_ST.w));

            // multiply the metallics by the normal for that direction, add each metallic together
            half4 metSmoAlp = topMetSmoAlp * absNormal.y + frontMetSmoAlp * absNormal.z + rightMetSmoAlp * absNormal.x;

            // set the albedo
             o.Albedo = color;
            // set the normal
            o.Normal = normal;
            // set the emission
            o.Emission = emission.rgb;
            // set the metallic with the red channel of the metSmoAlp texture
            o.Metallic = metSmoAlp.r;
            // set the smoothness with the green channel of the metSmoAlp texture
            o.Smoothness = metSmoAlp.g;
            // set the alpha with the blue channel of the metSmoAlp texture
            o.Alpha = metSmoAlp.b;
        }
        ENDCG
    }
    // If this shader fails to run, use the default diffuse shader
    FallBack "Diffuse"
}
