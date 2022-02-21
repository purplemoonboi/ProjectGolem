Shader "Unlit/NewSlicerShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        sliceNormal("normal", Vector) = (0,0,0,0)
        sliceCentre("centre", Vector) = (0,0,0,0)
    }
    SubShader
    {
        //Tags { "RenderType"="Opaque" }
        Tags { "Queue" = "Geometry" "IgnoreProjector" = "True"  "RenderType" = "Geometry" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 sliceCentre;
            float3 sliceNormal;

            v2f vert (appdata v)
            {
                v2f o;
                o.worldPos = mul(v.vertex, unity_ObjectToWorld).xyz;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float sliceSide = dot(sliceNormal, i.worldPos - sliceCentre);
                clip(-sliceSide);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col = fixed4(0.0, 1.0, 0.0, 1.0);
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
