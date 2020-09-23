﻿/*
    File name: Triplanar.shader
    Author: Michael Sweetman
    Summary: A Shader to project textures onto the front, right and top of a mesh without the need for UV (Opaque Version)
    Creation Date: 18/08/2020
    Last Modified: 23/09/2020
*/

Shader "Custom/TriplanarOpaque"
{
    Properties
    {
        [Header(Albedo)]
        // albedo texture to be projected on the top and bottom of the mesh
        _TopAlbedo("Top Albedo", 2D) = "white" {}

        // albedo texture to be projected on the front and back of the mesh
        _FrontAlbedo("Front Albedo", 2D) = "white" {}

        // albedo texture to be projected on the left and right of the mesh
        _RightAlbedo("Right Albedo", 2D) = "white" {}

        [Header(Normal)]
        // normal texture to be projected on the top and bottom of the mesh
        _TopNormal("Top Normal", 2D) = "bump" {}
        
        // normal texture to be projected on the front and back of the mesh
        _FrontNormal("Front Normal", 2D) = "bump" {}
        
        // normal texture to be projected on the left and right of the mesh
        _RightNormal("Right Normal", 2D) = "bump" {}

        [Header(Emission)]
        // emission texture to be projected on the top and bottom of the mesh
        _TopEmission("Top Emission", 2D) = "black" {}

        // emission texture to be projected on the front and back of the mesh
        _FrontEmission("Front Emission", 2D) = "black" {}

        // emission texture to be projected on the left and right of the mesh
        _RightEmission("Right Emission", 2D) = "black" {}

        [Header(Metallic Smoothness Occlusion)]
        // metallic, smoothness and occlusion textures to be projected on the top and bottom of the mesh
        _TopMetSmoOcc("Top Metallic(r), Smoothness(g), Occlusion(b)", 2D) = "white" {}

        // metallic, smoothness, and occlusion textures to be projected on the front and back of the mesh
        _FrontMetSmoOcc("Front Metallic(r), Smoothness(g), Occlusion(b)", 2D) = "white" {}

        // metallic, smoothness and occlusion textures to be projected on the left and right of the mesh
        _RightMetSmoOcc("Right Metallic(r), Smoothness(g), Occlusion(b)", 2D) = "white" {}

        [Header(Normal Map Options)]
        // intensity of the normal maps
        _NormalMapIntensity("Normal Map Intensity", Float) = 1.0

        [Header(Fresnel)]
        // determines whether the fresnel value will be applied to the emission 
        [Toggle] _ApplyToEmission("Apply Fresnel to Emissive?", Float) = 0
        // the emission colour outputted by the Fresnel shader
        _EmissionFresnelColor("Fresnel Colour", Color) = (1,1,1,1)
        // determines the intensity of the Fresnel effect for the Emission
        _EmissionFresnelExponent("Emission Fresnel Exponent", Float) = 1
    }
    SubShader
    {
        Tags
        {
            // Set RenderType to Opaque
            "RenderType" = "Opaque"
            // Set Queue to Geometry
            "Queue" = "Geometry"
        }

        // Set Level of Detail to 200
        LOD 200

        CGPROGRAM
        // Define this shader as a surface shader
        // Call the surf function
        // Use a Physically based Standard lighting model and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

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
        sampler2D _TopMetSmoOcc;
        sampler2D _FrontMetSmoOcc;
        sampler2D _RightMetSmoOcc;

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

        // get the tiling and offset information from the metallic, smoothness, occlusion textures
        float4 _TopMetSmoOcc_ST;
        float4 _FrontMetSmoOcc_ST;
        float4 _RightMetSmoOcc_ST;

        // define the normal map intensity as a fixed variable
        fixed _NormalMapIntensity;

        // define the fresnel alpha toggle as a boolean
        bool _ApplyToAlpha;
        // define the fresnel emission toggle as a boolean
        bool _ApplyToEmission;
        // define the Fresnel Colour as a float3
        float3 _EmissionFresnelColor;
        // define the Alpha Fresnel Exponent as a float
        float _AlphaFresnelExponent;
        // define the Emission Fresnel Exponent as a float
        float _EmissionFresnelExponent;

        // get the World Normal and World Position from the mesh
        struct Input
        {
            float3 worldPos;
            float3 viewDir;
            float3 worldNormal;
            INTERNAL_DATA
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
            fixed3 normal = topNormal * absNormal.y + frontNormal *absNormal.z + rightNormal * absNormal.x;
            // shift the normal values to be [-1,1] rather than [0,1]
            normal = 2 * normal - 1;

            // if the normal map intensity is greater than 0
            if (_NormalMapIntensity > 0)
            {
                // divide the z values of the normal by the intensity
                normal.z /= _NormalMapIntensity;
            }
            // normalise the normal vector
            normal = normalize(normal);

            // get the emission information from each texture
            fixed4   topEmission = tex2D(  _TopEmission, float2((position.x *   _TopEmission_ST.x) +   _TopEmission_ST.z, (position.z *   _TopEmission_ST.y) +   _TopEmission_ST.w));
            fixed4 frontEmission = tex2D(_FrontEmission, float2((position.x * _FrontEmission_ST.x) + _FrontEmission_ST.z, (position.y * _FrontEmission_ST.y) + _FrontEmission_ST.w));
            fixed4 rightEmission = tex2D(_RightEmission, float2((position.y * _RightEmission_ST.x) + _RightEmission_ST.z, (position.z * _RightEmission_ST.y) + _RightEmission_ST.w));

            // multiply the emissions by the normal for that direction, add each emission together
            half4 emission = topEmission * absNormal.y + frontEmission * absNormal.z + rightEmission * absNormal.x;

            // get the Metallic, Smoothness and Occlusion information from each texture
            fixed4   topMetSmoOcc = tex2D(  _TopMetSmoOcc, float2((position.x *   _TopMetSmoOcc_ST.x) +   _TopMetSmoOcc_ST.z, (position.z *   _TopMetSmoOcc_ST.y) +   _TopMetSmoOcc_ST.w));
            fixed4 frontMetSmoOcc = tex2D(_FrontMetSmoOcc, float2((position.x * _FrontMetSmoOcc_ST.x) + _FrontMetSmoOcc_ST.z, (position.y * _FrontMetSmoOcc_ST.y) + _FrontMetSmoOcc_ST.w));
            fixed4 rightMetSmoOcc = tex2D(_RightMetSmoOcc, float2((position.y * _RightMetSmoOcc_ST.x) + _RightMetSmoOcc_ST.z, (position.z * _RightMetSmoOcc_ST.y) + _RightMetSmoOcc_ST.w));

            // multiply the textures by the normal for that direction, add each texture together
            half4 metSmoOcc = topMetSmoOcc * absNormal.y + frontMetSmoOcc * absNormal.z + rightMetSmoOcc * absNormal.x;

            // get the dot prouct of the world normal and the view direction
            float fresnel = dot(o.Normal, IN.viewDir);

            // set the emission Fresnel value to be the inversed and clamped Fresnel value
            float emissionFresnel = saturate(1 - fresnel);
            // apply the emission Fresnel Exponent as power to emission Fresnel
            emissionFresnel = pow(emissionFresnel, _EmissionFresnelExponent);
            // apply the emission Fresnel value to the emission Fresnel colour
            float3 emissionFresnelColor = emissionFresnel * _EmissionFresnelColor;
            
            // if the fresnel value should be applied to the emission
            if (_ApplyToEmission)
            {
                // add the fresnel colour to the emission
                emission += fixed4(emissionFresnelColor, 1);
            }

            // set the albedo
            o.Albedo = color;
            // set the normal
            o.Normal = normal;
            // set the emission
            o.Emission = emission.rgb;
            // set the metallic with the red channel of the metSmoOcc texture
            o.Metallic = metSmoOcc.r;
            // set the smoothness with the green channel of the metSmoOcc texture
            o.Smoothness = metSmoOcc.g;
            // set the alpha with the blue channel of the metSmoOcc texture
            o.Occlusion = metSmoOcc.b;
            // set the alpha to 1
            o.Alpha = 1.0f;
        }
        ENDCG
    }
    // If this shader fails to run, use the default diffuse shader
    FallBack "Diffuse"
}