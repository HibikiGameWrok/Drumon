Shader "Unlit/TornadoShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
	_NoiseTex("Noise Texture", 2D) = "white" {}
	_Color("Color", Color) = (1, 1, 1, 1)

		_Speed ("Speed", Float) = 1

		_TestCutoffVar ("Brightness", Range(0, 1)) = 1
    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100

		Blend OneMinusDstColor One

        Pass
        {

			Cull Front

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

				float YourSin : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _NoiseTex;

			float _Speed;

			float4 _Color;

			float _TestCutoffVar;

            v2f vert (appdata v)
            {
                v2f o;

				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.YourSin = clamp(abs(sin(5 * (44 / 7) * (o.uv.y /*+ _Progress */ + _Speed * _Time.x))) * 0.8f - 0.5f, 0, 1);
				v.vertex.xz *= (1 + 0.5 * o.YourSin);

                o.vertex = UnityObjectToClipPos(v.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = _Color;//tex2D(_MainTex, i.uv);

			col += tex2D(_NoiseTex, i.uv + float2(2 * _Time.x * _Speed, 0));

				col += i.YourSin;

				if (i.uv.y >= 0.5)
				{
					col *= 1 - smoothstep(0.95, 1,i.uv.y);
				}
				else
				{
					col *= smoothstep(0, 0.05, i.uv.y);
				}

				col *= _TestCutoffVar;

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
            ENDCG
        }

		Pass
		{

			Cull Back

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

					float YourSin : TEXCOORD2;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				sampler2D _NoiseTex;

				float _Speed;

				float4 _Color;

				float _TestCutoffVar;

				v2f vert(appdata v)
				{
					v2f o;

					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					o.YourSin = clamp(abs(sin(5 * (44 / 7) * (o.uv.y /*+ _Progress */ + _Speed * _Time.x))) * 0.8f - 0.5f, 0, 1);
					v.vertex.xz *= (1 + 0.5 * o.YourSin);

					o.vertex = UnityObjectToClipPos(v.vertex);

					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					// sample the texture
					fixed4 col = _Color;//tex2D(_MainTex, i.uv);

				col += tex2D(_NoiseTex, i.uv + float2(2 * _Time.x * _Speed, 0));

					col += i.YourSin;

					if (i.uv.y >= 0.5)
					{
						col *= 1 - smoothstep(0.95, 1,i.uv.y);
					}
					else
					{
						col *= smoothstep(0, 0.05, i.uv.y);
					}

					col *= _TestCutoffVar;

					// apply fog
					UNITY_APPLY_FOG(i.fogCoord, col);
					return col;
				}
				ENDCG
			}

    }
}
