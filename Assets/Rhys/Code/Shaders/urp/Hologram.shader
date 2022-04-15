// @brief A custom PBR shader.
Shader "Custom/Hologram"
{
	Properties
	{
		_BaseMap("Base Texture", 2D) = "white" {}
		_BaseColor("Ground Colour", Color) = (0.6, 0.2, 0.6, 1)
		
		_Smoothness("Smoothness", Float) = 0.5

		[Toggle(_ALPHATEST_ON)] _EnableAlphaTest("Enable Alpha Cutoff", Float) = 0.0
		_Cutoff("Alpha Cutoff", Float) = 0.5
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

		HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseMap_ST;
			float4 _BaseColor;
			float _Smoothness;
			float _Cutoff;
			CBUFFER_END
		ENDHLSL

		Pass
		{
			Name "Hologram"
			Tags { "LightMode" = "UniversalForward" }

			HLSLPROGRAM

			// Required to compile gles 2.0 with standard SRP library
			// All shaders must be compiled with HLSLcc and currently only gles is not using HLSLcc by default
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x gles

			//#pragma target 4.5 // https://docs.unity3d.com/Manual/SL-ShaderCompileTargets.html

			#pragma vertex vert
			#pragma fragment frag

			// Material Keywords
			#pragma shader_feature _NORMALMAP
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _ALPHAPREMULTIPLY_ON
			#pragma shader_feature _EMISSION
			#pragma shader_feature _RECEIVE_SHADOWS_OFF

			// URP Keywords
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE

			// Unity defined keywords
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON
			#pragma multi_compile_fog

			struct Attributes
			{
				float4 positionOS   : POSITION;
				float3 normalOS		: NORMAL;
				float4 tangentOS	: TANGENT;
				float4 color		: COLOR;
				float2 uv           : TEXCOORD0;
			};

			struct Varyings
			{
				float4 positionCS   : SV_POSITION;
				float3 normalWS		: NORMAL;
				float4 tangentWS	: TANGENT;
				float4 color		: COLOR;
				float2 uv           : TEXCOORD0;
			};

			Varyings vert(Attributes IN)
			{
				Varyings o = (Varyings)0;

				VertexPositionInputs posIns = GetVertexPositionInputs(IN.positionOS.xyz);
				o.positionCS = posIns.positionCS;
				o.uv = TRANSFORM_TEX(IN.uv, _BaseMap);

				o.color = IN.color;
				VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
				o.normalWS = normalInputs.normalWS;
				return o;
			}

			float4 frag(Varyings IN) : SV_TARGET
			{

				return _BaseColor;
			}

			ENDHLSL
		} 

	}
}