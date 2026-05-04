Shader "Custom/SnowStamp"
{
    Properties
    {
        _MainTex ("Brush", 2D) = "white" {}
        _PrevTex ("Brush", 2D) = "black" {}
        _Strength ("Strength",Range(0,1)) =1

        _StampCenter("Stamp Center",Vector) = (0.5,0.5,0 ,0)
        _StampSize("Stamp Size",Float) = 0.1

        // _Color ("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Quene"="Transparent" }
        Blend One Zero
        ZWrite Off
        ZTest Always
        Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _PrevTex;
            // fixed4 _Color;
            float _Strength;
            float4 _StampCenter;
            float _StampSize;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            // fixed4 frag (v2f i) : SV_Target
            // {
            //     /// v3
            //     float2 uv = i.uv;
            //     float oldValue = tex2D(_PrevTex,uv).r;// 旧内容，直接将整张RT的全局UV采样
            //     float halfSize = _StampSize * 0.5;
            //     // float localUV = (uv-(_StampCenter.xy-halfSize))/_StampSize;
            //       float2 localUV = (uv - (_StampCenter.xy - halfSize)) / _StampSize;

            //     // 不在范围内保持旧的值
            //     // 不在脚印范围内，保持旧值
            //     if (localUV.x < 0 || localUV.x > 1 || localUV.y < 0 || localUV.y > 1)
            //     {
            //         return fixed4(oldValue, oldValue, oldValue, 1);
            //     }

            //     float brushValue = tex2D(_MainTex,localUV).r * _Strength;
            //     float result = max(oldValue,brushValue);
            //     return fixed4(result,result,result,1);

            // }

            fixed4 frag(v2f i) : SV_Target
{
    float2 uv = i.uv;
    float oldValue = tex2D(_PrevTex, uv).r;

    float halfSize = _StampSize * 0.5;
    float2 localUV = (uv - (_StampCenter.xy - halfSize)) / _StampSize;

    if (localUV.x < 0 || localUV.x > 1 || localUV.y < 0 || localUV.y > 1)
    {
        return fixed4(oldValue, oldValue, oldValue, 1);
    }

    return fixed4(1, 1, 1, 1);
}
            ENDCG
        }
    }
}
