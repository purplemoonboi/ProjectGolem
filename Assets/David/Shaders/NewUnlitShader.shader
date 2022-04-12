Shader "Unlit/NewUnlitShader"
{
    Properties{
        _Tint("Tint Color", Color) = (.5, .5, .5, .5)
        [Gamma] _Exposure("Exposure", Range(0, 8)) = 1.0
        _Rotation("Rotation", Range(0, 360)) = 0
        _Blend("Blend", Range(0.0, 1.0)) = 0
        [NoScaleOffset] _FrontTex0("Front [+Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _FrontTex1("Front [+Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _BackTex0("Back [-Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _BackTex1("Back [-Z]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _LeftTex0("Left [+X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _LeftTex1("Left [+X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _RightTex0("Right [-X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _RightTex1("Right [-X]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _UpTex0("Up [+Y]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _UpTex1("Up [+Y]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _DownTex0("Down [-Y]   (HDR)", 2D) = "grey" {}
        [NoScaleOffset] _DownTex1("Down [-Y]   (HDR)", 2D) = "grey" {}
    }

        SubShader{
            Tags { "Queue" = "Background" "RenderType" = "Background" "PreviewType" = "Skybox" }
            Cull Off ZWrite Off

            CGINCLUDE
            #include "UnityCG.cginc"

            half4 _Tint;
            half _Exposure;
            float _Rotation;
            float _Blend;

            float3 RotateAroundYInDegrees(float3 vertex, float degrees)
            {
                float alpha = degrees * UNITY_PI / 180.0;
                float sina, cosa;
                sincos(alpha, sina, cosa);
                float2x2 m = float2x2(cosa, -sina, sina, cosa);
                return float3(mul(m, vertex.xz), vertex.y).xzy;
            }

            struct appdata_t {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f {
                float4 vertex : SV_POSITION;
                float2 texcoord0 : TEXCOORD0;
                float2 texcoord1 : TEXCOORD1;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                float3 rotated = RotateAroundYInDegrees(v.vertex, _Rotation);
                o.vertex = UnityObjectToClipPos(rotated);
                o.texcoord0 = v.texcoord0;
                o.texcoord1 = v.texcoord1;
                return o;
            }

            float ParametricBlend(float t)
            {
                float sqt = t * t;
                return sqt / (2.0f * (sqt - t) + 1.0f);
            }

            half4 skybox_frag(v2f i, sampler2D smp0, sampler2D smp1, half4 smpDecode0, half4 smpDecode1)
            {
                half4 tex0 = tex2D(smp0, i.texcoord0);
                half4 tex1 = tex2D(smp1, i.texcoord1);
                half3 c0 = DecodeHDR(tex0, smpDecode0);
                half3 c1 = DecodeHDR(tex1, smpDecode1);
                c0 = c0 * _Tint.rgb * unity_ColorSpaceDouble.rgb * (1.0 - ParametricBlend(_Blend));
                c0 += c1 * _Tint.rgb * unity_ColorSpaceDouble.rgb * ParametricBlend(_Blend);
                c0 *= _Exposure;
                return half4(c0, 1);
            }
            ENDCG

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                sampler2D _FrontTex0;
                sampler2D _FrontTex1;
                half4 _FrontTex0_HDR;
                half4 _FrontTex1_HDR;
                half4 frag(v2f i) : SV_Target {
                    return skybox_frag(i, _FrontTex0, _FrontTex1, _FrontTex0_HDR, _FrontTex1_HDR);
                }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                sampler2D _BackTex0;
                sampler2D _BackTex1;
                half4 _BackTex0_HDR;
                half4 _BackTex1_HDR;
                half4 frag(v2f i) : SV_Target {
                    return skybox_frag(i, _BackTex0, _BackTex1, _BackTex0_HDR, _BackTex1_HDR);
                }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                sampler2D _LeftTex0;
                sampler2D _LeftTex1;
                half4 _LeftTex0_HDR;
                half4 _LeftTex1_HDR;
                half4 frag(v2f i) : SV_Target {
                    return skybox_frag(i, _LeftTex0, _LeftTex1, _LeftTex0_HDR, _LeftTex1_HDR);
                }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                sampler2D _RightTex0;
                sampler2D _RightTex1;
                half4 _RightTex0_HDR;
                half4 _RightTex1_HDR;
                half4 frag(v2f i) : SV_Target {
                    return skybox_frag(i, _RightTex0, _RightTex1, _RightTex0_HDR, _RightTex1_HDR);
                }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                sampler2D _UpTex0;
                sampler2D _UpTex1;
                half4 _UpTex0_HDR;
                half4 _UpTex1_HDR;
                half4 frag(v2f i) : SV_Target {
                    return skybox_frag(i, _UpTex0, _UpTex1, _UpTex0_HDR, _UpTex1_HDR);
                }
                ENDCG
            }
            Pass{
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0
                sampler2D _DownTex0;
                sampler2D _DownTex1;
                half4 _DownTex0_HDR;
                half4 _DownTex1_HDR;
                half4 frag(v2f i) : SV_Target {
                    return skybox_frag(i, _DownTex0, _DownTex1, _DownTex0_HDR, _DownTex1_HDR);
                }
                ENDCG
            }
    }
}
