Shader "Custom/VHSFullScreen"
{
    Properties
    {
        _Intensity ("Overall Intensity", Range(0, 1)) = 1
        _CenterClearRadius ("Center Clear Radius", Range(0, 1)) = 0.15
        _EdgeFadeSmoothness ("Edge Fade Smoothness", Range(0, 1)) = 0.55
        
        _NoiseStrength ("Noise Strength", Range(0, 1)) = 0.25
        _ScanlineStrength ("Scanline Strength", Range(0, 1)) = 0.35
        _ScanlineCount ("Scanline Count", Range(100, 1200)) = 520
        _RGBSplit ("RGB Split Pixels", Range(0, 10)) = 3
        _JitterStrength ("Jitter Strength", Range(0, 0.05)) = 0.008
        _WarpStrength ("Horizontal Warp", Range(0, 0.05)) = 0.01
        _TapeLineStrength ("Tape Line Strength", Range(0, 1)) = 0.3
        _EdgeShadowDarkness ("Edge Shadow Darkness", Range(0, 1)) = 0.6
        _ColorBleed ("Color Bleed", Range(0, 1)) = 0.15
    }

    SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Opaque"
        }

        Pass
        {
            Name "VHS Fullscreen Pass"

            ZTest Always
            ZWrite Off
            Cull Off

            HLSLPROGRAM

            #pragma vertex Vert
            #pragma fragment Frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            CBUFFER_START(UnityPerMaterial)
                float _Intensity;
                float _CenterClearRadius;
                float _EdgeFadeSmoothness;
                float _NoiseStrength;
                float _ScanlineStrength;
                float _ScanlineCount;
                float _RGBSplit;
                float _JitterStrength;
                float _WarpStrength;
                float _TapeLineStrength;
                float _EdgeShadowDarkness;
                float _ColorBleed;
            CBUFFER_END

            float rand(float2 co)
            {
                return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453);
            }

            float noise(float2 uv, float t)
            {
                return rand(uv * _ScreenParams.xy + t);
            }

            half4 Frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                float2 originalUV = input.texcoord;
                float t = _Time.y;

                // --- 1. CALCULATE THE CENTER MASK ---
                // Distance from center (0.5, 0.5)
                float distFromCenter = distance(originalUV, float2(0.5, 0.5));
                
                // effectMask will be 0.0 at the center (clear) and ramp up to 1.0 at the edges (heavy VHS)
                float effectMask = smoothstep(_CenterClearRadius, _EdgeFadeSmoothness, distFromCenter);

                // --- 2. APPLY DISTORTION TO MASKED UVs ---
                float2 distortedUV = originalUV;
                
                float lineY = floor(distortedUV.y * _ScreenParams.y);
                float lineNoise = rand(float2(lineY, floor(t * 24.0)));
                float jitter = (lineNoise - 0.5) * _JitterStrength;

                float wave1 = sin(distortedUV.y * 80.0 + t * 8.0) * _WarpStrength * 0.4;
                float wave2 = sin(distortedUV.y * 240.0 + t * 13.0) * _WarpStrength * 0.2;

                float bandY = frac(t * 0.18);
                float band = 1.0 - smoothstep(0.0, 0.08, abs(distortedUV.y - bandY));
                float bandJitter = band * _JitterStrength * 8.0;

                // Only apply the UV shifting based on the edge mask
                distortedUV.x += (jitter + wave1 + wave2 + bandJitter) * effectMask;

                // --- 3. SAMPLE TEXTURE CHANNELS (RGB SPLIT) ---
                // Scale the RGB split by the mask so it doesn't split in the center
                float pixelOffset = (_RGBSplit * effectMask) / _ScreenParams.x;
                
                half4 rSample = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, distortedUV + float2(pixelOffset, 0));
                half4 gSample = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, distortedUV);
                half4 bSample = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, distortedUV - float2(pixelOffset, 0));

                half3 col;
                col.r = rSample.r;
                col.g = gSample.g;
                col.b = bSample.b;

                // --- 4. APPLY VHS EFFECTS USING THE MASK ---
                // Color bleed
                half3 smear1 = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, distortedUV + float2(2.0 / _ScreenParams.x, 0)).rgb;
                half3 smear2 = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, distortedUV + float2(4.0 / _ScreenParams.x, 0)).rgb;
                half3 smearedCol = lerp(col, (col + smear1 + smear2) / 3.0, _ColorBleed);
                col = lerp(col, smearedCol, effectMask);

                // Scanlines (faded out in center)
                float scan = sin(originalUV.y * _ScanlineCount * 6.2831853);
                float scanMask = 1.0 - ((scan * 0.5 + 0.5) * _ScanlineStrength * effectMask);
                col *= scanMask;

                // Random static noise (only on edges)
                float n = noise(originalUV, t * 60.0);
                col += (n - 0.5) * _NoiseStrength * effectMask;

                // Moving tracking bands (only on edges)
                col += band * _TapeLineStrength * 0.18 * effectMask;
                col -= band * _TapeLineStrength * 0.08 * effectMask;

                // Color crushing (only on edges)
                half3 crushedCol = floor(col * 96.0) / 96.0;
                col = lerp(col, crushedCol, effectMask);

                // --- 5. EDGE SHADOW (VIGNETTE) ---
                // Darkens the outer edges to create a heavy vignette / "shadow around the box"
                float shadowFactor = lerp(1.0, 1.0 - _EdgeShadowDarkness, effectMask);
                col *= shadowFactor;

                // --- 6. FINAL BLEND ---
                half4 original = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearClamp, originalUV);
                half3 finalCol = lerp(original.rgb, col, _Intensity);

                return half4(saturate(finalCol), original.a);
            }

            ENDHLSL
        }
    }
}