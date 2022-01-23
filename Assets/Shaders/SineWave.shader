Shader "Unlit/SineWave2"
{
    Properties
    {
        MainColour ("Main Colour", Color) = (1.0, 1.0, 1.0, 1.0)
        LineThickness ("Line Thickness", Float) = 0.1
        Amplitude ("Amplitude", Float) = 1.0
        Wavelength ("Wavelength", Float) = 1.0
        Speed ("Speed", Range(0.0, 2)) = 0.5
    }

    SubShader
    {
        Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}
        LOD 100
        
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha 

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            #define TWOPI 6.28318530718
            uniform float4 MainColour;
            uniform float LineThickness;
            uniform float Amplitude;
            uniform float Wavelength;
            uniform float Speed;

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
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float diff = abs(sin((i.uv.x + (-Speed * _Time.y)) * TWOPI * Wavelength) - (((i.uv.y - 0.5f) * 2) / Amplitude));
                fixed4 col;
                if (diff <= LineThickness)
                    col = MainColour;
                else
                    col = fixed4(0.0f, 0.0f, 0.0f, 0.0f);

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
