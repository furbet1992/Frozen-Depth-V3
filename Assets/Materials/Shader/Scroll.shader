/*
    File name: Scroll.shader
    Author: Michael Sweetman
    Summary: A Shader that moves texture UVs to create a scrolling effect
    Creation Date: 01/09/2020
    Last Modified: 01/09/2020
*/

Shader "Custom/Scroll"
{
    Properties
    {
        [Header(Textures)]
        // albedo texture
        _Albedo ("Albedo", 2D) = "white" {}
        // normal texture
        _Normal("Normal", 2D) = "bump" {}
        // emission texture
        _Emission("Emission", 2D) = "black" {}
        // metallic, smoothness, occlusion and alpha texture where each texture type is stored in a different channel (r,g,b,a)
        _MetSmoOccAlp("Metallic(r), Smoothness(g), Occulusion(b), Alpha(a)", 2D) = "white" {}


        [Header(Scrolling)]
        // the amount the textures scroll horizontally
        _ScrollX("Scroll X", Float) = 0.0
        // the amount the textures scroll vertically
        _ScrollY("Scroll Y", Float) = 0.0
        
        [Header(Texture Scroll Toggles)]
        // determines whether the albedo texture will scroll
        [Toggle] _ScrollAlbedo("Scroll Albedo?", Float) = 0
        // determines whether the normal texture will scroll
        [Toggle] _ScrollNormal ("Scroll Normal?", Float) = 0
        // determines whether the emission texture will scroll
        [Toggle] _ScrollEmission("Scroll Emission?", Float) = 0
        // determines whether the metallic, smoothness, occlusion and alpha texture will scroll 
        [Toggle] _ScrollMetSmoOccAlp("Scroll Metallic, Smoothness, Occlusion and Alpha?", Float) = 0
    }
    SubShader
    {
        Tags
        {
            // Set RenderType to Transparent
            "RenderType"="Transparent"
            // Set Queue to Transparent
            "Queue" = "Transparent"
        }

        // Set the Level of Detail to 200
        LOD 200

        CGPROGRAM
        
        // Define this shader as a surface shader
        // Call the surf function
        // Use a Physically based Standard lighting model, enable shadows on all light types and support transparency
        #pragma surface surf Standard fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        // define the texture properties as Sampler2D variables
        sampler2D _Albedo;
        sampler2D _Normal;
        sampler2D _Emission;
        sampler2D _MetSmoOccAlp;

        // define the ScrollX and ScrollY properties as fixed variables
        fixed _ScrollX;
        fixed _ScrollY;

        // define the texture scroll toggles as boolean variables
        bool _ScrollAlbedo;
        bool _ScrollNormal;
        bool _ScrollEmission;
        bool _ScrollMetSmoOccAlp;

        // get the uv of each texture
        struct Input
        {
            float2 uv_Albedo;
            float2 uv_Normal;
            float2 uv_Emission;
            float2 uv_MetSmoOccAlp;
        };

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // if the albedo texture should scroll
            if (_ScrollAlbedo)
            {
                // get the albedo texture uv
                float2 scrolledAlbedoUV = IN.uv_Albedo;
                // apply scroll values, multiplied by the time
                scrolledAlbedoUV.x += _ScrollX * _Time;
                scrolledAlbedoUV.y += _ScrollY * _Time;
                // set the albedo using the scrolled uv
                o.Albedo = tex2D(_Albedo, scrolledAlbedoUV);
            }
            // if the albedo texture should not scroll
            else
            {
                // set the albedo using the original uv
                o.Albedo = tex2D(_Albedo, IN.uv_Albedo);
            }

            // if the normal texture should scroll
            if (_ScrollNormal)
            {
                // get the normal texture uv
                float2 scrolledNormalUV = IN.uv_Normal;
                // apply scroll values, multiplied by the time
                scrolledNormalUV.x += _ScrollX * _Time;
                scrolledNormalUV.y += _ScrollY * _Time;
                // set the normal using the scrolled uv
                o.Normal = tex2D(_Normal, scrolledNormalUV);
            }
            // if the normal texture should not scroll
            else
            {
                // set the normal using the original uv
                o.Normal = tex2D(_Normal, IN.uv_Normal);
            }

            // if the emission texture should scroll
            if (_ScrollEmission)
            {
                // get the emission texture uv
                float2 scrolledEmissionUV = IN.uv_Emission;
                // apply scroll values, multiplied by the time
                scrolledEmissionUV.x += _ScrollX * _Time;
                scrolledEmissionUV.y += _ScrollY * _Time;
                // set the emission using the scrolled uv
                o.Emission = tex2D(_Emission, scrolledEmissionUV);
            }
            // if the emission texture should not scroll
            else
            {
                // set the emission using the original uv
                o.Emission = tex2D(_Emission, IN.uv_Emission);
            }

            // if the other textures should scroll
            if (_ScrollMetSmoOccAlp)
            {
                // get the other texture uv
                float2 scrolledMetSmoOccAlpUV = IN.uv_MetSmoOccAlp;
                // apply scroll values, multiplied by the time
                scrolledMetSmoOccAlpUV.x += _ScrollX * _Time;
                scrolledMetSmoOccAlpUV.y += _ScrollY * _Time;

                // get the metallic, smoothness, occlusion and alpha information using the scrolled uv
                half4 metSmoOccAlp = tex2D(_MetSmoOccAlp, scrolledMetSmoOccAlpUV);
                // set the metallic using the red channel of the texture
                o.Metallic = metSmoOccAlp.r;
                // set the smoothness using the green channel of the texture
                o.Smoothness = metSmoOccAlp.g;
                // set the occlusion using the blue channel of the texture
                o.Occlusion = metSmoOccAlp.b;
                // set the alpha using the alpha channel of the texture
                o.Alpha = metSmoOccAlp.a;
            }
            // if the other textures should not scroll
            else
            {
                // get the metallic, smoothness, occlusion and alpha information using the original uv
                half4 metSmoOccAlp = tex2D(_MetSmoOccAlp, IN.uv_MetSmoOccAlp);
                // set the metallic using the red channel of the texture
                o.Metallic = metSmoOccAlp.r;
                // set the smoothness using the green channel of the texture
                o.Smoothness = metSmoOccAlp.g;
                // set the occlusion using the blue channel of the texture
                o.Occlusion = metSmoOccAlp.b;
                // set the alpha using the alpha channel of the texture
                o.Alpha = metSmoOccAlp.a;
            }
        }
        ENDCG
    }
    // If this shader fails to run, use the default diffuse shader
    FallBack "Diffuse"
}
