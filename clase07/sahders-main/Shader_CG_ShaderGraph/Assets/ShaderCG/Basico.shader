Shader "Unlit/Basico"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Transparency ("Transparency", Range(0,1)) = 1
        _WaveAmplitude ("Wave Amplitude", Float) = 0.1
        _WaveFrequency ("Wave Frequency", Float) = 2
        _WaveSpeed ("Wave Speed", Float) = 2
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST;
            float4 _Color;
            float _Transparency;
            float _WaveAmplitude;
            float _WaveFrequency;
            float _WaveSpeed;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD1;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float wave = sin(_Time.y * _WaveSpeed + v.vertex.x * _WaveFrequency) * _WaveAmplitude;
                v.vertex.y += wave;

                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldNormal = normalize(UnityObjectToWorldNormal(v.normal));
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 finalColor = texColor * _Color;

                // Agregar normal map sin tangentes
                float3 normalTex = tex2D(_NormalMap, i.uv).rgb * 2 - 1;
                float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
                float3 fakeNormal = normalize(i.worldNormal + normalTex * 0.2); // mezcla simple

                float light = saturate(dot(fakeNormal, lightDir));
                finalColor.rgb *= light;
                finalColor.a *= _Transparency;

                return finalColor;
            }
            ENDCG
        }
    }

    FallBack "Unlit/Transparent"
}