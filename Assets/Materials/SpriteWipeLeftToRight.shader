Shader "Custom/SpriteWipeLeftToRight"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _WipeProgress ("Wipe Progress", Range(0,1)) = 0
        _BorderWidth ("Border Width", Range(0,0.5)) = 0.02
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

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

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _WipeProgress;
            float _BorderWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float threshold = 1.0 - _WipeProgress;
                if (i.uv.x > threshold + _BorderWidth)
                {
                    col.a = 0;
                }
                else if (i.uv.x > threshold)
                {
                    float t = (i.uv.x - threshold) / _BorderWidth;
                    col.a *= lerp(1, 0, t);
                }
                return col;
            }
            ENDCG
        }
    }
}
