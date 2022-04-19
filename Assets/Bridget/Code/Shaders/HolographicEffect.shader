// @brief A custom PBR shader.

//Original author: https://sharpcoderblog.com/blog/create-a-hologram-effect-in-unity-3d

Shader "Custom/HolographicEffect"
{
	Properties
	{
		_Color("Color", Color) = (0, 1, 1, 1)
		_MainTex("Base (RGB)", 2D) = "white" {}
		_AlphaTexture("Alpha Mask (R)", 2D) = "white" {}

		//Alpha Mask Properties
		_Scale("Alpha Tiling", Float) = 3
		_ScrollSpeedV("Alpha Scroll Speed", Range(0, 5.0)) = 1.0

		// Glow
		_GlowIntensity("Glow Intensity", Range(0.01, 1.0)) = 0.5
	}

	SubShader
	{
		Tags{ "Queue" = "Overlay" "IgnoreProjector" = "True" "RenderType" = "Transparent" }

		Pass
		{
			Lighting Off
			ZWrite On
			Blend SrcAlpha One
			Cull Back

			HLSLPROGRAM

				#pragma vertex vertexFunc
				#pragma fragment fragmentFunc

				#include "UnityCG.cginc"	//Included for using certain helper functions like UnityObjectToClipPos

				struct Attributes
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct Varyings
				{
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
					float3 grabPos : TEXCOORD1;
					float3 viewDir : TEXCOORD2;
					float3 worldNormal : NORMAL;
				};

				float4 _Color, _MainTex_ST;
				sampler2D _MainTex, _AlphaTexture;
				half _Scale, _ScrollSpeedV, _GlowIntensity;

				Varyings vertexFunc(Attributes IN)
				{
					Varyings OUT;

					OUT.position = UnityObjectToClipPos(IN.vertex);
					OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);

					//Alpha mask coordinates
					OUT.grabPos = UnityObjectToViewPos(IN.vertex);

					//Scroll Alpha mask uv
					OUT.grabPos.y += _Time * _ScrollSpeedV;

					OUT.worldNormal = UnityObjectToWorldNormal(IN.normal);
					OUT.viewDir = normalize(UnityWorldSpaceViewDir(OUT.grabPos.xyz));

					return OUT;
				}

				float4 fragmentFunc(Varyings IN) : SV_Target
				{
					half dirVertex = (dot(IN.grabPos, 1.0) + 1.0) / 2.0;

					float4 alphaColor = tex2D(_AlphaTexture,  IN.grabPos.xy * _Scale);
					float4 pixelColor = tex2D(_MainTex, IN.uv);

					pixelColor.w = alphaColor.w;

					// Rim Light
					half rim = 1.0 - saturate(dot(IN.viewDir, IN.worldNormal));

					return pixelColor * _Color * (rim + _GlowIntensity);
				}

			ENDHLSL
		}
	}
}