//#ifdef CUSTOM_LIT_PASS_INCLUDED
//#define CUSTOM_LIT_PASS_INCLUDED

struct Attributes
{
    float4 position : SV_Position;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD;
};

struct Varyings
{
    float4 position : SV_Position;
    float3 normal : NORMAL;
    float2 uv : TEXCOORD;
};

Varyings LitPassVertex(Attributes input) 
{
    Varyings output = (Varyings)0;
    
    return output;
}

float4 LitPassFragment(Varyings input) : SV_Target
{
    return float4(0.5, 0.5, 0.5, 1.0);
}

//#endif