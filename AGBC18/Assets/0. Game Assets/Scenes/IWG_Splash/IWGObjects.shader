Shader "IWG/Splash/Objects" {

	Properties {
		_MainTex("Sprite Texture", 2D) = "white" { }
		_LightPosition("Light Position", Vector) = (0.5, 0.5, 0, 0)
		_DarkColor("Dark Color", Color) = (0,0,0,1)
		_LightColor("Light Color", Color) = (1,1,1,1)
		_MaxRadius("Max Radius", Float) = 1
		_MinRadius("Min Radius", Float) = 0
	}

	SubShader {

		Tags {
			"QUEUE" = "Transparent"
			"IGNOREPROJECTOR" = "true"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
		}

		Pass {

			ZWrite Off
			Cull Off
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			struct appdata {
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f {
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD1;
				float4 color : COLOR;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			float4 _LightPosition;
			float _MaxRadius, _MinRadius;
			float4 _DarkColor, _LightColor;

			v2f vert(appdata v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.screenPos = ComputeScreenPos(o.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.color = v.color;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target {
				fixed4 col = tex2D(_MainTex, i.uv) * i.color;
				
				float falloff = saturate(1 - 
					(distance(i.screenPos.xy, _LightPosition.xy) - _MinRadius) / _MaxRadius
				);

				col.rgb *= lerp(_DarkColor, _LightColor, falloff).rgb;

				return col;
			}

			ENDCG
		}
	}
}
