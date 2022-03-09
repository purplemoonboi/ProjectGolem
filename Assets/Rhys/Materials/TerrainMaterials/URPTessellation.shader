Shader "Custom/URPTessellation"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" "RenderPipeline" = "UniversalRenderPipeline" }
        LOD 200

        pass
        {
            //Include URP lighting functions.
            HLSLINCLUDE
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            ENDHLSL

            //Core shader code
            HLSLPROGRAM
            
            
            #pragma require tessellation
            #pragma vertex TessellationVertexProgram
            #pragma fragment frag
            #pragma hull hull
            #pragma domain domain
            
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT


             struct ControlPoint
             {
                float4 position : INTERNALTESSPOS;
                float2 uv     : TEXCOORD0;
                float4 colour : COLOR;
                float3 normal : NORMAL;
             };

            struct Attributes
            {
                float4 position : POSITION;
                float3 normal   : NORMAL;
                float2 uv       : TEXCOORD0;
                float4 colour   : COLOR;
            };

            struct TessellationFactors
            {
                float edge[3] : SV_TessFactor;
                float inside : SV_InsideTessFactor;
            };

            struct Varyings
            {
                float4 position : POSITION;
                float3 normal   : NORMAL;
                float2 uv       : TEXCOORD0;
                float4 colour   : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _MinTessDistance;
            float _MaxTessDistance;
            float _Tess;

            /*..Input vertex shader..*/

            ControlPoint TessellationVertexProgram(Attributes vertex)
            {
                ControlPoint cp;

                cp.position = vertex.position;
                cp.normal = vertex.normal;
                cp.uv = vertex.uv;
                cp.colour = vertex.colour;

                return cp;
            }

            /*..Hull shader stage..*/

            [UNITY_domain("tri")]
            [UNITY_outputcontrolpoints(3)]
            [UNITY_outputtopology("triangle_cw")]
            [UNITY_partitioning("pow2")]
            [UNITY_patchconstantfunc("patchConstantFunction")]
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
                tf.inside = (edge0 + edge1 + edge2) * 0.3333333f;
                return tf;
            }

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionsInputs posIn = GetVertexPositionInputs(input.position.xyz);
                output.position = posIn.positionsWS;
                output.normal = input.normal;
                output.uv = input.uv;
                output.colour = input.colour;
                return output;
            }

            [UNITY_domain("tri")]
            Varyings domain(TessellationFactors factors, OutputPatch<ControlPoint, 3> patch, float3 barycentricCoordinates : SV_DomainLocation)
            {
                Attributes v;
                //Macro to calculate the new billinear interpolated positions of each attribute.
                v.position =
                patch[0].position * barycentricCoordinates.x +
                patch[1].position * barycentricCoordinates.y +
                patch[2].position * barycentricCoordinates.z;

                v.normal =
                    patch[0].normal * barycentricCoordinates.x +
                    patch[1].normal * barycentricCoordinates.y +
                    patch[2].normal * barycentricCoordinates.z;

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

             half4 frag(Varyings IN) : SV_Target
             {
                 return IN.colour;
             }

            ENDHLSL
        }
    }
    FallBack "Diffuse"
}
