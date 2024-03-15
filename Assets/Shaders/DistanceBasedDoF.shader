Shader "Hidden/Custom/DistanceBasedDoF" {
    Properties{
        _MainTex("Base (RGB)", 2D) = "white" {}
        _DepthTex("Depth (R)", 2D) = "white" {}
        _BlurStrength("Blur Strength", Float) = 10.0
        _FocusDistance("Focus Distance", Float) = 10.0
        _FocusRange("Focus Range", Float) = 5.0
    }
        SubShader{
            Tags { "RenderType" = "Opaque" }
            LOD 100

            Pass {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag

                #include "UnityCG.cginc"

                struct appdata {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f {
                    float2 uv : TEXCOORD0;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                sampler2D _DepthTex;
                float _BlurStrength;
                float _FocusDistance;
                float _FocusRange;

                v2f vert(appdata v) {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = v.uv;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target{
                    float depth = LinearEyeDepth(tex2D(_DepthTex, i.uv).r);
                    float dist = abs(_WorldSpaceCameraPos.z - depth);
                    float blur = smoothstep(_FocusDistance - _FocusRange, _FocusDistance + _FocusRange, dist) * _BlurStrength;
                    fixed4 col = tex2D(_MainTex, i.uv);

                    return col * (1 - blur) + blur * fixed4(1, 1, 1, 1);
                }
                ENDCG
            }
        }
}
