﻿// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Unlit/GrassShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (0, 0, 1, 1)
		_TextureScale("TextureScale", Range(0, 1)) = 0
		_WindStrength("WindStrength", Range(0, 10)) = 5
		_PlayerRange("PlayerRange", Range(0.001, 2)) = 0.5
		_PlayerStrength("PlayerStrength", Range(0, 20)) = 2
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" }
			LOD 100

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"

				float4 _Color;
				float WindOffset;
				float _TextureScale;

				float _WindStrength;
				float3 _PlayerPos = float3(9999, 9999, 9999);
				float _PlayerRange;
				float _PlayerStrength;

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


				float4 recvCol : TEXCOORD1;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			
			v2f vert(appdata v)
			{
				v2f o;

				o.recvCol = 0;

				float3 worldVertexPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				float3 worldPos = mul(unity_ObjectToWorld, float4(0.0, 0.0, 0.0, 1.0)).xyz;
				if (worldVertexPos.y > worldPos.y)
				{
					float4 col = tex2Dlod(_MainTex, float4(_TextureScale * worldPos.x, _TextureScale * (worldPos.z + WindOffset), 0, 0));

					v.vertex.z += _WindStrength * col.r;
					o.recvCol = col;

					float dist = pow(worldPos.x - _PlayerPos.x, 2) + pow(worldPos.z - _PlayerPos.z, 2);
					if (dist <= _PlayerRange * _PlayerRange)
					{
						v.vertex.xz += (worldPos.xz - _PlayerPos.xz) * _PlayerStrength * ((_PlayerRange - sqrt(dist))/_PlayerRange);
					}
				}

				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);
				// apply fog
				//UNITY_APPLY_FOG(i.fogCoord, col);
				return _Color + 0.4f * i.recvCol;
			}
			ENDCG
		}
	}
}
