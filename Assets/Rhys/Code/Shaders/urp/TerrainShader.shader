Shader "Custom/TerrainShader"
{
    Properties
    {
        [MainTexture]
        _MainTex("Texture", 2D) = "white" {}
        [MainColor]
        _BaseColor("Base Colour", Color) = (1,1,1,1)

        [Space(20)]
        [Toggle(_ALPHATEST_ON)] _AlphaTestToggle("Alpha Clipping", Float) = 0
        _Cutoff("Alpha Cutoff", Float) = 0.5

        [Space(20)]
        [Toggle(_SPECULAR_SETUP)] _MetallicSpecToggle("Workflow, Specular (if on), Metallic (if off)", Float) = 0
        [Toggle(_SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A)] _SmoothnessSource("Smoothness Source, Albedo Alpha (if on) vs Metallic (if off)", Float) = 0
        _Metallic("Metallic", Range(0.0, 1.0)) = 0
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.5
        _SpecColor("Specular Color", Color) = (0.5, 0.5, 0.5, 0.5)
        [Toggle(_METALLICSPECGLOSSMAP)] _MetallicSpecGlossMapToggle("Use Metallic/Specular Gloss Map", Float) = 0
        _MetallicSpecGlossMap("Specular or Metallic Map", 2D) = "black" {}
        // Usually this is split into _SpecGlossMap and _MetallicGlossMap, but I find
        // that a bit annoying as I'm not using a custom ShaderGUI to show/hide them.

        [Space(20)]
        [Toggle(_NORMALMAP)] _NormalMapToggle("Use Normal Map", Float) = 0
        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Bump Scale", Float) = 1

        // Not including Height (parallax) map in this example/template
        [Space(20)]
        [Toggle(_OCCLUSIONMAP)] _OcclusionToggle("Use Occlusion Map", Float) = 0
        [NoScaleOffset] _OcclusionMap("Occlusion Map", 2D) = "bump" {}
        _OcclusionStrength("Occlusion Strength", Range(0.0, 1.0)) = 1.0


        [Space(20)]
        [Toggle(_EMISSION)] _Emission("Emission", Float) = 0
        [HDR] _EmissionColor("Emission Color", Color) = (0,0,0)
        [NoScaleOffset]_EmissionMap("Emission Map", 2D) = "black" {}

        [Space(20)]
        [Toggle(_SPECULARHIGHLIGHTS_OFF)] _SpecularHighlights("Turn Specular Highlights Off", Float) = 0
        [Toggle(_ENVIRONMENTREFLECTIONS_OFF)] _EnvironmentalReflections("Turn Environmental Reflections Off", Float) = 0
            
        [Space(20)]
       _Tess("Tessellation", Range(1, 32)) = 20
       _MaxTessDistance("MaxTessDistance", Range(1, 1000)) = 100
       _MinTessDistance("MinTessDistance", Float) = 5.0
    }
    SubShader
     {
     
         Tags
         {
             "RenderPipeline" = "UniversalRenderPipeline"
             "RenderType" = "Opaque"
             "Queue" = "Geometry"
         }
     
         HLSLPROGRAM
         #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl
        /* CBUFFER_START(UnityPerMaterial)
         float4 _MainTex_ST;
         float4 _BaseColor;
         float4 _EmissionColor;
         float4 _SpecColor;
         float _Metallic;
         float _Smoothness;
         float _OcclusionStrength;
         float _Cutoff;
         float _BumpScale;
         float _MinTessDistance;
         float _MaxTessDistance;
         float _Tess;
         CBUFFER_END*/
         ENDHLSL
     
         Pass
         {
             Name "ForwardLit"
             Tags {"LightMode" = "UniversalForward"}
     
             HLSLPROGRAM
     
     
             #pragma require tessellation
             #pragma vertex TessellationVertexProgram
             #pragma fragment frag
             #pragma hull hull
             #pragma domain domain
     
             // Material Keywords
             #pragma shader_feature_local _NORMALMAP
             #pragma shader_feature_local_fragment _ALPHATEST_ON
             #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON
             #pragma shader_feature_local_fragment _EMISSION
             #pragma shader_feature_local_fragment _METALLICSPECGLOSSMAP
             #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
             #pragma shader_feature_local_fragment _OCCLUSIONMAP
     
             #pragma shader_feature_local_fragment _SPECULARHIGHLIGHTS_OFF
             #pragma shader_feature_local_fragment _ENVIRONMENTREFLECTIONS_OFF
             #pragma shader_feature_local_fragment _SPECULAR_SETUP
             #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
     
             // URP Keywords
             #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
             #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
             // Note, v11 changes these to :
             // #pragma multi_compile _ _MAIN_LIGHT_SHADOWS _MAIN_LIGHT_SHADOWS_CASCADE _MAIN_LIGHT_SHADOWS_SCREEN
     
             #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
             #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
             #pragma multi_compile_fragment _ _SHADOWS_SOFT
             #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION 
             #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING 
             #pragma multi_compile _ SHADOWS_SHADOWMASK 
     
             // Unity Keywords
             #pragma multi_compile _ LIGHTMAP_ON
             #pragma multi_compile _ DIRLIGHTMAP_COMBINED
             #pragma multi_compile_fog
     
             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
     
             struct ControlPoint
             {
                 float4 position : INTERNALTESSPOS;
                 float2 uv     : TEXCOORD0;
                 float4 colour : COLOR;
                 float3 normal : NORMAL;
             };
     
             struct Attributes
             {
                 float4 positionOS	: POSITION;
                 #ifdef _NORMALMAP
                 float4 tangentOS 	: TANGENT;
                 #endif
                 float3 normalOS		: NORMAL;
                 float2 uv		    : TEXCOORD0;
                 float2 lightmapUV	: TEXCOORD1;
                 float4 colour		: COLOR;
             };
     
             struct TessellationFactors
             {
                 float edge[3] : SV_TessFactor;
                 float inside : SV_InsideTessFactor;
             };
     
             struct Varyings
             {
                 float4 positionCS 			    : SV_POSITION;
                 float2 uv		    		    : TEXCOORD0;
                 DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
                 float3 positionWS				: TEXCOORD2;
                 #ifdef _NORMALMAP
                 float3 normalWS					: TEXCOORD3;    // xyz: normal, w: viewDir.x
                 float3 tangentWS					: TEXCOORD4;    // xyz: tangent, w: viewDir.y
                 float3 bitangentWS				: TEXCOORD5;    // xyz: bitangent, w: viewDir.z
                 #else
                 float3 normalWS					: TEXCOORD3;
                 #endif
                 #ifdef _ADDITIONAL_LIGHTS_VERTEX
                 half4 fogFactorAndVertexLight	: TEXCOORD6; // x: fogFactor, yzw: vertex light
                 #else
                 half  fogFactor					: TEXCOORD6;
                 #endif
                 #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                 float4 shadowCoord 				: TEXCOORD7;
                 #endif
                 float4 colour					: COLOR;
             };
     
             #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"
            
             float4 _MainTex_ST;
             float4 _BaseColor;
             float4 _EmissionColor;
             float4 _SpecColor;
             float _Metallic;
             float _Smoothness;
             float _OcclusionStrength;
             float _Cutoff;
             float _BumpScale;
             float _MinTessDistance;
             float _MaxTessDistance;
             float _Tess;
            
             /*..Input vertex shader..*/
     
             ControlPoint TessellationVertexProgram(Attributes vertex)
             {
                 ControlPoint cp;
                 cp.position = vertex.positionOS;
                 cp.normal = vertex.normalOS;
                 cp.uv = vertex.uv;
                 cp.colour = vertex.colour;
                 return cp;
             }
     
             /*..Hull shader stage..*/
     
             [domain("tri")]
             [outputcontrolpoints(3)]
             [outputtopology("triangle_cw")]
             [partitioning("fractional_odd")]
             [patchconstantfunc("patchConstantFunction")]
             ControlPoint hull(InputPatch<ControlPoint, 3> patch, uint id : SV_OutputControlPointID)
             {
                 return patch[id];
             }
     
             float CalcDistanceTessFactor(float4 position, float minDist, float maxDist, float tess)
             {
                 float3 worldPosition = mul(unity_ObjectToWorld, position).xyz;
                 float dist = distance(worldPosition, _WorldSpaceCameraPos);
                 float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
     
                 return f;
             }
     
             TessellationFactors patchConstantFunction(InputPatch<ControlPoint, 3> patch)
             {
                 float minDist = _MinTessDistance;
                 float maxDist = _MaxTessDistance;
     
                 TessellationFactors tf;
     
                 float edge0 = CalcDistanceTessFactor(patch[0].position, minDist, maxDist, _Tess);
                 float edge1 = CalcDistanceTessFactor(patch[1].position, minDist, maxDist, _Tess);
                 float edge2 = CalcDistanceTessFactor(patch[2].position, minDist, maxDist, _Tess);
     
                 // To ensure there is no tearing or gaps we average out the tess factor at edges.
                 tf.edge[0] = (edge1 + edge2) * 0.5f;
                 tf.edge[1] = (edge2 + edge0) * 0.5f;
                 tf.edge[2] = (edge0 + edge1) * 0.5f;
                 tf.inside = (edge0 + edge1 + edge2) / 3;
                 return tf;
             }
     
             Varyings vert(Attributes input)
             {
                 Varyings output = (Varyings)0;
                 output.positionWS = mul(unity_ObjectToWorld, float4(input.positionOS.xyz, 1.0f));
                 output.normalWS = mul(unity_ObjectToWorld, float4(input.normalOS.xyz, 1.0f));
                 output.uv = input.uv;
                 output.colour = input.colour;
                 return output;
             }
     
             [domain("tri")]
             Varyings domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
             {
                 Attributes v = (Attributes)0;
                 //Macro to calculate the new billinear interpolated positions of each attribute.
                 v.positionOS =
                 patch[0].position * barycentricCoordinates.x +
                 patch[1].position * barycentricCoordinates.y +
                 patch[2].position * barycentricCoordinates.z;
     
                 //Transform world space to clip space.
                 v.positionOS = float4(TransformWorldToView(v.positionOS).xyz, 1.0f);
     
                 v.normalOS =
                     patch[0].normal * barycentricCoordinates.x +
                     patch[1].normal * barycentricCoordinates.y +
                     patch[2].normal * barycentricCoordinates.z;
     
                 v.normalOS = mul(v.normalOS, unity_ObjectToWorld);
                 v.normalOS = normalize(v.normalOS);
     
                 v.uv =
                     patch[0].uv * barycentricCoordinates.x +
                     patch[1].uv * barycentricCoordinates.y +
                     patch[2].uv * barycentricCoordinates.z;
     
                 v.colour =
                     patch[0].colour * barycentricCoordinates.x +
                     patch[1].colour * barycentricCoordinates.y +
                     patch[2].colour * barycentricCoordinates.z;
     
                 //DomainCalc(position)
                 //DomainCalc(uv)
                 //DomainCalc(colour)
                 //DomainCalc(normal)
     
                  return vert(v);
              }
     
              half4 SampleMetallicSpecGloss(float2 uv, half albedoAlpha)
              {
                  half4 specGloss;
                  #ifdef _METALLICSPECGLOSSMAP
                  specGloss = SAMPLE_TEXTURE2D(_MetallicSpecGlossMap, sampler_MetallicSpecGlossMap, uv);
                  #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                  specGloss.a = albedoAlpha * _Smoothness;
                  #else
                  specGloss.a *= _Smoothness;
                  #endif
                  #else // _METALLICSPECGLOSSMAP
                  #if _SPECULAR_SETUP
                  specGloss.rgb = _SpecColor.rgb;
                  #else
                  specGloss.rgb = _Metallic.rrr;
                  #endif
                  #ifdef _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
                  specGloss.a = albedoAlpha * _Smoothness;
                  #else
                  specGloss.a = _Smoothness;
                  #endif
                  #endif
                  return specGloss;
              }
     
              half SampleOcclusion(float2 uv)
              {
                  #ifdef _OCCLUSIONMAP
                  #if defined(SHADER_API_GLES)
                  return SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
                  #else
                  half occ = SAMPLE_TEXTURE2D(_OcclusionMap, sampler_OcclusionMap, uv).g;
                  return LerpWhiteTo(occ, _OcclusionStrength);
                  #endif
                  #else
                  return 1.0;
                  #endif
              }
     
              void InitializeSurfaceData(Varyings IN, out SurfaceData surfaceData)
              {
                  surfaceData = (SurfaceData)0;
     
                  half4 albedoAlpha = SampleAlbedoAlpha(IN.uv, TEXTURE2D_ARGS(_BaseMap, sampler_BaseMap));
                  surfaceData.alpha = Alpha(albedoAlpha.a, _BaseColor, _Cutoff);
                  surfaceData.albedo = albedoAlpha.rgb * _BaseColor.rgb * IN.colour.rgb;
     
                  surfaceData.normalTS = SampleNormal(IN.uv, TEXTURE2D_ARGS(_BumpMap, sampler_BumpMap), _BumpScale);
                  surfaceData.emission = SampleEmission(IN.uv, _EmissionColor.rgb, TEXTURE2D_ARGS(_EmissionMap, sampler_EmissionMap));
                  surfaceData.occlusion = SampleOcclusion(IN.uv);
     
                  half4 specGloss = SampleMetallicSpecGloss(IN.uv, albedoAlpha.a);
                  #if _SPECULAR_SETUP
                  surfaceData.metallic = 1.0h;
                  surfaceData.specular = specGloss.rgb;
                  #else
                  surfaceData.metallic = specGloss.r;
                  surfaceData.specular = half3(0.0h, 0.0h, 0.0h);
                  #endif
                  surfaceData.smoothness = specGloss.a;
              }
     
     
              void InitializeInputData(Varyings input, half3 normalTS, out InputData inputData)
              {
                  inputData = (InputData)0; // avoids "not completely initalized" errors
                  inputData.positionWS = input.positionWS;
                  #ifdef _NORMALMAP
                  half3 viewDirWS = half3(input.normalWS.w, input.tangentWS.w, input.bitangentWS.w);
                  inputData.normalWS = TransformTangentToWorld(normalTS, half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));
                  #else
                  half3 viewDirWS = GetWorldSpaceNormalizeViewDir(inputData.positionWS);
                  inputData.normalWS = input.normalWS;
                  #endif
                  inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
                  viewDirWS = SafeNormalize(viewDirWS);
                  inputData.viewDirectionWS = viewDirWS;
                  #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                  inputData.shadowCoord = input.shadowCoord;
                  #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                  inputData.shadowCoord = TransformWorldToShadowCoord(inputData.positionWS);
                  #else
                  inputData.shadowCoord = float4(0, 0, 0, 0);
                  #endif
                  // Fog
                  #ifdef _ADDITIONAL_LIGHTS_VERTEX
                  inputData.fogCoord = input.fogFactorAndVertexLight.x;
                  inputData.vertexLighting = input.fogFactorAndVertexLight.yzw;
                  #else
                  inputData.fogCoord = input.fogFactor;
                  inputData.vertexLighting = half3(0, 0, 0);
                  #endif
                  inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
                  inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionCS);
                  inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
              }
     
     
              half4 frag(Varyings IN) : SV_Target
              {
                 SurfaceData surfaceData;
                 InitializeSurfaceData(IN, surfaceData);
     
                 InputData inputData;
                 InitializeInputData(IN, surfaceData.normalTS, inputData);
     
                 half4 colour = UniversalFragmentPBR(inputData, surfaceData);
     
                 colour.rgb = MixFog(colour.rgb, inputData.fogCoord);
     
                  return half4(1,0,0,1);
              }
     
              ENDHLSL
         }
     }
}

