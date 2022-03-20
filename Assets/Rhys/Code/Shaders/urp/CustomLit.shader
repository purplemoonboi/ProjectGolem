Shader "Custom/CustomLit"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BaseColour("Colour", Color) = (0.5, 0.5, 0.5, 1.0)
    }
    SubShader
    {
        Pass
        {
            Tags
            {
                "LightMode" = "CustomLit"
            }

            HLSLPROGRAM
            #pragma vertex LitPassVertex
            #pragma fragment LitPassFragment
            #include "../hlsl/LitPass.hlsl"
            ENDHLSL
        }
    }
}
