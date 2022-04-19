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

		_SandStrength("Sand Strength", Float) = 0.5

		[Toggle(_EMISSION)] _EnableEmission("Enable Emission", Float) = 0.0
		_EmissionMap("Emission Texture", 2D) = "white" {}
		_EmissionColor("Emission Colour", Color) = (0, 0, 0, 0)

		//Tessellation attributes
		_TessellationFactor("Tessellation Factor", Vector) = (1.0, 1.0, 1.0, 1.0)
		_PlayerPosition("Player Position", Vector) = (0.0, 0.0, 0.0, 1.0)
		_TessDeformThreshold("Tessellation Threshold", Float) = 10
		_TessellationBias("Tessellation Bias", Float) = 2
		_TessellationTolerance("Tessellation Tolerance", Float) = 2
		_BackCullTolerance("Back Cull Tolerance", Float) = 2
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

			float4 _TessellationFactor;
			float4 _PlayerPosition;
			float _TessDeformThreshold;
			float _TessellationBias;
			float _TessellationTolerance;
			float _BackCullTolerance;

			float _BumpScale;
			float4 _EmissionColor;
			float _Smoothness;
			float _Cutoff;

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
				#pragma hull Hull
				#pragma domain Domain

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

				struct TessellationControlPoint
				{
					float4 positionCS : SV_POSITION;
					float3 positionWS : INTERNALTESSPOS;
					float3 normalWS : NORMAL;
					UNITY_VERTEX_INPUT_INSTANCE_ID
				};

				struct TessellationFactors 
				{
					float edge[3] : SV_TessFactor;
					float inside : SV_InsideTessFactor;
				};

				struct Varyings
				{
					//Position in clip space.
					float4 positionCS				: SV_POSITION;
					float4 color					: COLOR;
					float2 uv						: TEXCOORD0;

					DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);

					#ifdef REQUIRES_WORLD_SPACE_POS_INTERPOLATOR
					float3 positionWS			: TEXCOORD2;
					#endif

					//Normal now in world space.
					float3 normalWS : TEXCOORD3;
					UNITY_VERTEX_INPUT_INSTANCE_ID
					UNITY_VERTEX_OUTPUT_STEREO
					float4 tangentWS 				: TEXCOORD4;
					float3 viewDirWS 				: TEXCOORD5;
					half4 fogFactorAndVertexLight	: TEXCOORD6; // x: fogFactor, yzw: vertex light

					#ifdef REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
					float4 shadowCoord			: TEXCOORD7;
					#endif
				};


				//VERTEX STAGE


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
				TessellationControlPoint vert(Attributes input)
				{
					TessellationControlPoint output;

					UNITY_SETUP_INSTANCE_ID(input);
					UNITY_TRANSFER_INSTANCE_ID(input, output);

					VertexPositionInputs posnInputs = GetVertexPositionInputs(input.positionOS);
					VertexNormalInputs normalInputs = GetVertexNormalInputs(input.normalOS);

					output.positionCS = posnInputs.positionCS;
					output.positionWS = posnInputs.positionWS;
					output.normalWS = normalInputs.normalWS;
					return output;
				}


				//HULL STAGE

				bool IsOutOfBounds(float3 position, float3 lower, float3 higher)
				{
					return position.x < lower.x || position.x > higher.x ||
						   position.y < lower.y || position.y > higher.y || 
						   position.z < lower.z || position.z > higher.z;
				}

				bool ShouldFrustrumCull(float4 positionCS)
				{
					float3 culling = positionCS.xyz;
					float w = positionCS.w;
					float3 lowerBounds = float3(-w, -w, -w * UNITY_RAW_FAR_CLIP_VALUE);
					float3 higherBounds = float3(w, w, w);

					return IsOutOfBounds(culling, lowerBounds, higherBounds);
				}

				bool IsPointOutOfFrustrum(float4 positionCS, float tolerance)
				{
					float3 culling = positionCS.xyz;
					float w = positionCS.w;
					float3 lowerBounds = float3(-w - tolerance, -w - tolerance, -w * UNITY_RAW_FAR_CLIP_VALUE - tolerance);
					float3 higherBounds = float3(w + tolerance, w + tolerance, w + tolerance);

					return IsOutOfBounds(culling, lowerBounds, higherBounds);
				}

				bool ShouldBackFaceCull(float4 p0PositionCS, float4 p1PositionCS, float4 p2PositionCS, float tolerance)
				{
					float3 point0 = p0PositionCS.xyz / p0PositionCS.w;
					float3 point1 = p1PositionCS.xyz / p1PositionCS.w;
					float3 point2 = p2PositionCS.xyz / p2PositionCS.w;
					float3 normal = cross(point1 - point0, point2 - point0);

					//Only accounts for DX11, need to flip tolerance for OpenGL API.
					return cross(point1 - point0, point2 - point0).z < -tolerance;
				}

				bool ShouldClipPatch(float4 position0CS, float4 position1CS, float4 position2CS, float frustrumCullTolerance)
				{
					bool allPointsOutOfBounds =
						ShouldFrustrumCull(position0CS) &&
						ShouldFrustrumCull(position1CS) &&
						ShouldFrustrumCull(position2CS);

					return allPointsOutOfBounds || ShouldBackFaceCull(position0CS, position1CS, position2CS, _TessellationTolerance);
				}

				// Calculate the tessellation factor for an edge
				float EdgeTessellationFactor(float scale, float bias, float3 p0PositionWS, float3 p1PositionWS)
				{
					float factor = distance(p0PositionWS, p1PositionWS) / scale;

					return max(1, factor + bias);
				}

				// @brief The patch constant function.
				TessellationFactors PatchConstantFunction(
					InputPatch<TessellationControlPoint, 3> patch)
				{
					UNITY_SETUP_INSTANCE_ID(patch[0]);

					// Calculate tessellation factors
					TessellationFactors f = (TessellationFactors)0;

					if (ShouldClipPatch(patch[0].positionCS, patch[1].positionCS, patch[2].positionCS, _TessellationTolerance))
					{
						f.edge[0] =	0;
						f.edge[1] =	0;
						f.edge[2] =	0;
						f.inside  =	0;
					}
					else
					{
						// Calculate tessellation factors
						f.edge[0] = EdgeTessellationFactor(_TessellationFactor, _TessellationBias, patch[1].positionWS, patch[2].positionWS);
						f.edge[1] = EdgeTessellationFactor(_TessellationFactor, _TessellationBias, patch[2].positionWS, patch[0].positionWS);
						f.edge[2] = EdgeTessellationFactor(_TessellationFactor, _TessellationBias, patch[0].positionWS, patch[1].positionWS);
						f.inside = ( // If the compiler doesn't play nice...
							EdgeTessellationFactor(_TessellationFactor, _TessellationBias, patch[1].positionWS, patch[2].positionWS) +
							EdgeTessellationFactor(_TessellationFactor, _TessellationBias, patch[2].positionWS, patch[0].positionWS) +
							EdgeTessellationFactor(_TessellationFactor, _TessellationBias, patch[0].positionWS, patch[1].positionWS)
							) / 3.0;
					}
					return f;
				}

				// @brief Hull Shader
				[domain("tri")] // Signal we're inputting triangles
				[outputcontrolpoints(3)] // Triangles have three points
				[outputtopology("triangle_cw")] // Signal we're outputting triangles
				[patchconstantfunc("PatchConstantFunction")] // Register the patch constant function
				[partitioning("pow2")] // Select a partitioning mode: integer, fractional_odd, fractional_even or pow2
				TessellationControlPoint Hull(
					InputPatch<TessellationControlPoint, 3> patch, // Input triangle
					uint id : SV_OutputControlPointID)
				{ // Vertex index on the triangle

					return patch[id];
				}
				

				//DOMAIN STAGE

/*
				// Calculate the maximum deform length among all deformation points
				float DeformationLength(float3 vertexPositionWS, float3 movementDirectionWS)
				{
					// Calculate and return the amount to deform along movementDirectionWS
					float amountToDeform = 1f;
					return amountToDeform;
				}

				// The length to perturb postions along the tangent and bitangent to reconstruct normals
				#define NORMAL_RECONSTRUCTION_LENGTH 0.01

				void CalculateDeformations(float3 originalPositionWS, float3 originalNormalWS,
					float3 tangentWS, out float3 deformedPositionWS, out float3 deformedNormalWS)
				{
					float3 deformDirectionWS = -originalNormalWS;
					float maxDeformLength = DeformationLength(originalPositionWS, deformDirectionWS);
					deformedPositionWS = originalPositionWS + maxDeformLength * deformDirectionWS; // The deformed position

					// Calculate a deformed normal position by offsetting the original position by the tangent and bitangent
					// Deform both of those positions and take their cross product to find a new normal vector
					float3 bitangentWS = normalize(cross(originalNormalWS, tangentWS));
					float3 positionNudgedTangentWS = originalPositionWS + tangentWS * NORMAL_RECONSTRUCTION_LENGTH;

					float3 positionNudgedBitangentWS = originalPositionWS + bitangentWS * NORMAL_RECONSTRUCTION_LENGTH;

					float3 deformedPosnNTangentWS = positionNudgedTangentWS +
						DeformationLength(positionNudgedTangentWS, deformDirectionWS, 0) * deformDirectionWS;

					float3 deformedPosnNBitangentWS = positionNudgedBitangentWS +
						DeformationLength(positionNudgedBitangentWS, deformDirectionWS, 0) * deformDirectionWS;
					deformedNormalWS = normalize(cross(
						deformedPosnNTangentWS - deformedPositionWS,
						deformedPosnNBitangentWS - deformedPositionWS));
				}

*/

				#define BARYCENTRIC_INTERPOLATE(fieldName) \
						patch[0].fieldName * barycentricCoordinates.x + \
						patch[1].fieldName * barycentricCoordinates.y + \
						patch[2].fieldName * barycentricCoordinates.z


				// @brief Domain Shader
				[domain("tri")]											
				Varyings Domain(
					TessellationFactors factors,						 
					OutputPatch<TessellationControlPoint, 3> patch,		
					float3 barycentricCoordinates : SV_DomainLocation)
				{ 

					Varyings output;

					// Setup instancing and stereo support (for VR)
					UNITY_SETUP_INSTANCE_ID(patch[0]);
					UNITY_TRANSFER_INSTANCE_ID(patch[0], output);
					UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

					float3 positionWS = BARYCENTRIC_INTERPOLATE(positionWS);
					float3 normalWS = BARYCENTRIC_INTERPOLATE(normalWS);
					//float2 uvs = BARYCENTRIC_INTERPOLATE();

					output.positionCS = TransformWorldToHClip(positionWS);
					output.normalWS = normalWS;
					output.positionWS = positionWS;

					return output;
				}


				//FRAGMENT STAGE


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

				// @brief Rim lighting of the terrain.
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
			#pragma target 5.0

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