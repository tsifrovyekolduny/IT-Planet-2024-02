Shader "Custom/TransparentShader1" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags { "RenderType"="Transparent" }
        LOD 200
        
        Blend SrcAlpha OneMinusSrcAlpha
        
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            struct appdata {
                float4 vertex : POSITION;
                float2 uv_MainTex : TEXCOORD0;
            };
            
            struct v2f {
                float2 uv_MainTex : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            fixed4 _Color;
            float _Cutoff;
            
            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_MainTex = v.uv_MainTex;
                return o;
            }

            
            fixed4 frag(v2f i) : SV_Target {
                fixed4 c = tex2D(_MainTex, i.uv_MainTex) * _Color;
                if (c.a < _Cutoff) {
                    discard;
                }
                return c;
            }
            ENDCG
        }
    }
}
