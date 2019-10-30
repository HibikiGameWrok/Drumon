Shader "Unlit/VFX_ImpactAftermath"
{
	Properties
	{
		_Color ("Color", Color) = (1, 1, 1, 1)
		_MainTex("Texture", 2D) = "white" {}

		_Progress("Progress", Range(0, 1)) = 0

		_SelfMoveSpeed("Speed", Float) = 0
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

				Cull Off

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
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				float _Progress;

				float4 _Color;

				float _SelfMoveSpeed;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					//o.uv.x *= 2;
					//o.uv.x += 0.5;
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					//fixed4 col = tex2D(_MainTex, i.uv);
					fixed4 noiseCol = tex2D(_MainTex, float2(i.uv.x, 1) /* i.uv*/);

				fixed4 col = _Color;

				_Progress += _SelfMoveSpeed * _Time.x;

				_Progress = fmod(_Progress, 1);

				float UVProgress = (1 - i.uv.y) + noiseCol.r * 0.25;

				if (!(1.05 * _Progress * _Progress - 0.05 < UVProgress && UVProgress < _Progress))
				{
					clip(-1);			
				}

				return col;
			}
			ENDCG
		}
		}
}
