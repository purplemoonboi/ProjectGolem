// @brief A custom PBR shader.
Shader "Custom/SandShader" 
{
	Properties
	{
		//Base properties
		_BaseMap("Base Texture", 2D) = "white" {}
		_BaseColor("Sand Colour", Color) = (0.6, 0.2, 0.6, 1)
		//Specular properties.
		_GlitterColour("Glitter Colour", Color) = (1.0, 1.0, 1.0, 1.0)
		_OceanColour("Ocean Colour", Color) = (0.7, 0.7, 0.7, 1.0)
		//Fresnel Attributes.
		_RimColour("Rim Colour", Color) = (0.1, 0.6, 0.8, 1.0)
		_RimPower("Rim Power", Float) = 2
		_RimStrength("Rim Strength", Float) = 2

		_Smoothness("Smoothness", Float) = 0.5

		[Toggle(_ALPHATEST_ON)] _EnableAlphaTest("Enable Alpha Cutoff", Float) = 0.0
		_Cutoff("Alpha Cutoff", Float) = 0.5

		//Normal map properties.
		[Toggle(_NORMALMAP)] _EnableBumpMap("Enable Normal Map", Float) = 0.0
		_BumpMap("Normal/Bump Texture", 2D) = "bump" {}
		_BumpScale("Bump Scale", Float) = 1
		_SandStrength("Sand Strength", Float) = 0.5

		[Toggle(_EMISSION)] _EnableEmission("Enable Emission", Float) = 0.0
		_EmissionMap("Emission Texture", 2D) = "white" {}
		_EmissionColor("Emission Colour", Color) = (0, 0, 0, 0)

		//Legacy terrain properties.
		_GradSlopeColour("Gradual Slope Colour", Color) = (0.75, 0.14, 0.72, 1)
		_SteepSlopeColour("Steep Slope Colour", Color) = (0.84, 0.2, 0.83, 1)
		_GroundThreshold("Ground Threshold", Float) = 0.2
		_SlopeThreshold("Gradual Threshold", Float) = 0.7

	}
	SubShader
	{
		Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalPipeline" }

		HLSLINCLUDE
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

			CBUFFER_START(UnityPerMaterial)
			float4 _BaseMap_ST;
			float4 _BaseColor;
			float4 _GlitterColour;
			float4 _RimColour;
			float _RimPower;
			float _RimStrength;
			float4 _OceanColour;
			float _SandStrength;


			float4 _GradSlopeColour;
			float4 _SteepSlopeColour;
			float _BumpScale;
			float4 _EmissionColor;
			float _Smoothness;
			float _Cutoff;

			float _GroundThreshold;
			float _SlopeThreshold;

			CBUFFER_END

		ENDHLSL

		Pass
		{
			Name "CustomLit"
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
				#pragma shader_feature   _NORMALMAP
				#pragma shader_feature   _ALPHATEST_ON
				#pragma shader_feature   _ALPHAPREMULTIPLY_ON
				#pragma shader_feature   _EMISSION
				//#pragma shader_feature _METALLICSPECGLOSSMAP
				//#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
				//#pragma shader_feature _OCCLUSIONMAP
				//#pragma shader_feature _ _CLEARCOAT _CLEARCOATMAP // URP v10+

				//#pragma shader_feature _SPECULARHIGHLIGHTS_OFF
				//#pragma shader_feature _ENVIRONMENTREFLECTIONS_OFF
				#pragma shader_feature   _SPECULAR_SETUP
				#pragma shader_feature   _RECEIVE_SHADOWS_OFF

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

				// Includes
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
				#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

				struct Attributes 
				{
					//Position and normal/tangent in object space.
					float4 positionOS   : POSITION;
					float3 normalOS		: NORMAL;
					float4 tangentOS	: TANGENT;


					float4 color		: COLOR;
					float2 uv           : TEXCOORD0;
					float2 lightmapUV   : TEXCOORD1;
				};

				struct Varyings
				{
					//Position in clip space.
					float4 positionCS				: SV_POSITION;
					float4 color					: COLOR;
					float2 uv					: TEXCOORD0;

					DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

					#ifdef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
						float3 positionWS			: TEXCOORD2;
					#endif

					//Normal now in world space.
					float3 normalWS					: TEXCOORD3;
					float4 tangentWS 				: TEXCOORD4;


					float3 viewDirWS 				: TEXCOORD5;
					half4 fogFactorAndVertexLight	: TEXCOORD6; // x: fogFactor, yzw: vertex light

					#ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
						float4 shadowCoord			: TEXCOORD7;
					#endif
				};


				//We define this function to support olders versions of URP.
				//Calculates the view vector from the user.
				#if SHADER_LIBRARY_VERSION_MAJOR < 9
				float3 GetWorldSpaceViewDir(float3 positionWS)
				{
					if (unity_OrthoParams.w == 0) 
					{
						// Perspective
						return _WorldSpaceCameraPos - positionWS;
					}
					else 
					{
						// Orthographic
						float4x4 viewMat = GetWorldToViewMatrix();
						return viewMat[2].xyz;
					}
				}
				#endif

				// @brief Vertex shader 
				Varyings vert(Attributes IN)
				{
					Varyings OUT;

					VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.positionOS.xyz);
					OUT.positionCS = positionInputs.positionCS;
					OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
					OUT.color = IN.color;

					//If world position is required.
					#ifdef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
						OUT.positionWS = positionInputs.positionWS;
					#endif

					OUT.viewDirWS = GetWorldSpaceViewDir(positionInputs.positionWS);

					VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normalOS, IN.tangentOS);
					OUT.normalWS = normalInputs.normalWS;

					//If normal map has been defined.
					#ifdef _NORMALMAP
						real sign = IN.tangentOS.w * GetOddNegativeScale();
						OUT.tangentWS = half4(normalInputs.tangentWS.xyz, sign);
					#endif

					half3 vertexLight = VertexLighting(positionInputs.positionWS, normalInputs.normalWS);
					half fogFactor = ComputeFogFactor(positionInputs.positionCS.z);

					OUT.fogFactorAndVertexLight = half4(fogFactor, vertexLight);

					OUTPUT_LIGHTMAP_UV(IN.lightmapUV, unity_LightmapST, OUT.lightmapUV);
					OUTPUT_SH(OUT.normalWS.xyz, OUT.vertexSH);

					//If shadow coordinates have been defined.
					#ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
						OUT.shadowCoord = GetShadowCoord(positionInputs);
					#endif

					return OUT;
				}

				// @brief Initialises the input data ensuring all values are set to 0.
				InputData InitializeInputData(Varyings IN, half3 normalTS) 
				{
					InputData inputData = (InputData)0;


					//World space
					#if defined(REQUIRES_WORLD_SPACE_POS_INTERPOLATOR)
						inputData.positionWS = IN.positionWS;
					#endif

					half3 viewDirWS = SafeNormalize(IN.viewDirWS);

					#ifdef _NORMALMAP
						float sgn = IN.tangentWS.w; // should be either +1 or -1
						float3 bitangent = sgn * cross(IN.normalWS.xyz, IN.tangentWS.xyz);
						inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(IN.tangentWS.xyz, bitangent.xyz, IN.normalWS.xyz));
					#else
						inputData.normalWS = IN.normalWS;
					#endif

					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
					inputData.viewDirectionWS = viewDirWS;

					//Shadow coordinates.
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						inputData.shadowCoord = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
					#else
						inputData.shadowCoord = float4(0, 0, 0, 0);
					#endif

					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
					inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
					inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.vertexSH, inputData.normalWS);
					return inputData;
				}

				half3 RimLighting(float3 normalWS, float3 viewVecWS)
				{
					float rim = 1.0f - saturate(dot(normalWS, viewVecWS));
					rim = saturate(pow(rim, _RimPower) * _RimStrength);
					rim = max(rim, 0);
					return rim * _RimColour;
				}

				// @brief Initialises the surface data ensuring all values are set to 0.
				SurfaceData InitializeSurfaceData(Varyings IN) 
				{
					//Initialise surface data to 0. (All variables will now be set to 0)
					SurfaceData surfaceData = (SurfaceData)0;

					half4 albedoAlpha = SampleAlbedoAlpha(IN.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
					surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);

					surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb * IN.color.rgb;
/*
					//Gradient based colour.
					half4 color = (half4)0;
					float blendAmount = 1.0f;

					float slope = 1.0f - IN.normalWS.y;

					if (slope < _GroundThreshold)
					{
						blendAmount = slope / 0.2f;
						color = lerp(_BaseColor, _GradSlopeColour, blendAmount);
					}
					if ((slope < _SlopeThreshold) && (slope >= _GroundThreshold))
					{
						blendAmount = (slope - _GroundThreshold) * (1.0f / (_SlopeThreshold - _GroundThreshold));
						color = lerp(_GradSlopeColour, _SteepSlopeColour, blendAmount);
					}
					if (slope >= _SlopeThreshold)
					{
						color = _SteepSlopeColour;
					}

					surfaceData.albedo = color;
*/
					//surfaceData.normalTS = SampleNormal(IN.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);

					//Sand normals.
					float3 randomNormal = SampleNormal(IN.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
					surfaceData.normalTS = normalize(lerp(-IN.normalWS, randomNormal, _SandStrength));
					surfaceData.smoothness = 0.5;

					//Emission 
					surfaceData.emission = SampleEmission(IN.uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
					surfaceData.occlusion = 1;
					return surfaceData;
				}

				// @brief Fragment shader pass.
				half4 frag(Varyings IN) : SV_Target 
				{
					SurfaceData surfaceData = InitializeSurfaceData(IN);
					InputData inputData = InitializeInputData(IN, surfaceData.normalTS);

					half4 color = UniversalFragmentPBR
					(
						inputData, 
						surfaceData.albedo, 
						surfaceData.metallic,
						surfaceData.specular, 
						surfaceData.smoothness, 
						surfaceData.occlusion,
						surfaceData.emission,
						surfaceData.alpha
					);
					half3 rimLight = RimLighting(-IN.normalWS, IN.viewDirWS);
					color.rgb += rimLight;
					color.rgb = MixFog(color.rgb, inputData.fogCoord);
					color.a = saturate(color.a);

					return color; 
				}
				ENDHLSL
		}

		//Shadow pass.
		Pass
		{
			Name "ShadowCaster"
			Tags { "LightMode" = "ShadowCaster" }

			ZWrite On
			ZTest LEqual

			HLSLPROGRAM
			// Required to compile gles 2.0 with standard srp library
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x gles
			//#pragma target 4.5

			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			// GPU Instancing
			#pragma multi_compile_instancing
			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment

			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"

			// Note if we want to do any vertex displacment, we'll need to change the vertex function :
			/*
			//  e.g.
			#pragma vertex vert

			Varyings vert(Attributes input) {
				Varyings output;
				UNITY_SETUP_INSTANCE_ID(input);

				// Example Displacement
				input.positionOS += float4(0, _SinTime.y, 0, 0);

				output.uv = TRANSFORM_TEX(input.texcoord, _BaseMap);
				output.positionCS = GetShadowPositionHClip(input);
				return output;
			}*/

			// Using the ShadowCasterPass means we also need _BaseMap, _BaseColor and _Cutoff shader properties.
			// Also including them in cbuffer, with the exception of _BaseMap as it's a texture.

			ENDHLSL
		}

		// The DepthOnly pass is very similar to the ShadowCaster but doesn't include the shadow bias offsets.
		// I believe Unity uses this pass when rendering the depth of objects in the Scene View.
		// But for the Game View / actual camera Depth Texture it renders fine without it.
		// It's possible that it could be used in Forward Renderer features though, so we should probably still include it.
		Pass 
		{
			Name "DepthOnly"
			Tags { "LightMode" = "DepthOnly" }

			ZWrite On
			ColorMask 0

			HLSLPROGRAM
			#pragma prefer_hlslcc gles
			#pragma exclude_renderers d3d11_9x gles
			//#pragma target 4.5

			// Material Keywords
			#pragma shader_feature _ALPHATEST_ON
			#pragma shader_feature _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			// GPU Instancing
			#pragma multi_compile_instancing
			#pragma multi_compile _ DOTS_INSTANCING_ON

			#pragma vertex DepthOnlyVertex
			#pragma fragment DepthOnlyFragment

			// Note, the Lit shader that URP provides uses this, but it also handles the cbuffer which we already have.
			// We could change the shader to use their cbuffer, but we can also just do this :
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/CommonMaterial.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/Shaders/DepthOnlyPass.hlsl"

			ENDHLSL
		}
	}
}