Shader "Unlit/WaterRushShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_StreaksTex("Streaks Texture", 2D) = "white" {}

			_Color("Color", Color) = (0, 0, 1, 1)

		_WaterNoise("Water Noise", 2D) = "white" {}

			_IriTex("IriTexture", 2D) = "white" {}
			_IriMag("IriMagnification", Float) = 1

				_Progress("Progress", Float) = 0

				_CutoffFromBottom("Cutoff From Bottom", Range(0, 1)) = 0

				_Speed("Speed", Float) = 1

				_EndSmoothener("End Smoothener", Float) = 0.1

				_WhereToCurveEnd("Where To Curve End", Range(0, 1)) = 0.9

		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		_DissolveTex("Dissolve Tex", 2D) = "white" {}
		_DissolveAmount("Dissolve Amount", Range(0, 1)) = 0


		_TestVariable("Test Variable", Range(0, 1)) = 0 

    }
    SubShader
    {
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Blend SrcAlpha One

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
				float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD2;
				float YScale : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				float YourSin : TEXCOORD5;
				float3 viewDir : TEXCOORD6;
				float additional : TEXCOORD7;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _StreaksTex;

			sampler2D _WaterNoise;

			sampler2D _DissolveTex;
			float _DissolveAmount;

			float _Speed;

			float _IriMag;

			float _Progress;

			float _WhereToCurveEnd;

			float _TestVariable;

			uniform sampler2D _IriTex; uniform float4 _IriTex_ST;

			float _EndSmoothener;

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;


            v2f vert (appdata v)
            {
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.YScale = length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y));

				o.YourSin = clamp(abs(sin(20 * (44 / 7) * (o.uv.y /*+ _Progress */ + _Speed * _Time.x))) * 0.8f - 0.7f, 0, 1);
				v.vertex.xz *= (1 + 0.5 * o.YourSin);

				float value = (v.vertex.y + 1) / 2;
				o.additional = value;
				//v.vertex.xz *= clamp(8 * value, 0.1, 1);

				//if (value >= _WhereToCurveEnd)
				{
					float magvalue = log2(((1 - value) / (1 - _WhereToCurveEnd)) + 1);
					v.vertex.xz *= magvalue; //1 -> 0
					v.vertex.y -= _EndSmoothener * (1 - magvalue);
				}

				o.vertex = UnityObjectToClipPos(v.vertex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				//o.uv.y *= o.YScale;

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				o.viewDir = WorldSpaceViewDir(v.vertex);

				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
            }

			float4 _Color;

			float _CutoffFromBottom;

            fixed4 frag (v2f i) : SV_Target
            {
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);

			float2 UV = i.uv;
			UV.y *= i.YScale;
			UV.y += /*_Progress*/ 8 * _Speed * _Time.x;

			if (i.additional <= _CutoffFromBottom) clip(-1);

				fixed4 col = tex2D(_MainTex, UV);

				float NoiseDistort = tex2D(_WaterNoise, UV).r;

				//col *= (1 + 0.3f * NoiseDistort);

				//col += 1.5f * clamp((i.additional - 0.85) / 0.15, 0, 1);
				//col += 0.6 * tex2D(_StreaksTex, i.uv).r;

				float  NDotV = dot(normalize(i.worldNormal), normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz));
				float2 NDotV2 = float2(NDotV, NDotV);
				float4 IriColor = tex2D(_IriTex, TRANSFORM_TEX(NDotV2, _IriTex));


				float Additional2 = i.additional;

				//if (i.additional > _TestVariable) clip(-1);

				//Additional2 = lerp(1 - i.additional, _TestVariable * 4 - 2, Additional2);

				float dissolvevalue = tex2D(_DissolveTex, UV).r - _DissolveAmount + Additional2;
				clip(dissolvevalue);
				float LineTransitionAmount = 0;
				if (dissolvevalue >= 0.03)
				{
					LineTransitionAmount = 1 - smoothstep(0.03, 0.08, dissolvevalue);
				}
				else
				{
					LineTransitionAmount = smoothstep(-0.02, 0.03, dissolvevalue);
				}
				//col *= lerp(1, 2, LineTransitionAmount);

				clip((1 - Additional2) - _TestVariable);
				if ((1 - Additional2) - _TestVariable <= 0.03)
				{
					LineTransitionAmount += smoothstep(-0.02, 0.03, (1 - Additional2) - _TestVariable);
				}
				else if ((1 - Additional2) - _TestVariable <= 0.08)
				{
					LineTransitionAmount += 1 - smoothstep(0.03, 0.08, (1 - Additional2) - _TestVariable);
				}


				col *= 1 - clamp(10 * (0.1 - i.additional), 0, 1);
				col *= clamp(((1 - Additional2) - _TestVariable) / 0.08, 0, 1);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return _Color * col * (0.3f * NoiseDistort + 1.5f * clamp((i.additional - 0.85) / 0.15, 0, 1) + lerp(0, 1, LineTransitionAmount) + IriColor * _IriMag + i.YourSin * 5 + 0.6 * tex2D(_StreaksTex, i.uv).r);
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
				float3 normal : NORMAL;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float4 worldPos : TEXCOORD2;
				float YScale : TEXCOORD3;
				float3 worldNormal : TEXCOORD4;
				float YourSin : TEXCOORD5;
				float3 viewDir : TEXCOORD6;
				float additional : TEXCOORD7;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;

			sampler2D _StreaksTex;

			sampler2D _WaterNoise;

			sampler2D _DissolveTex;
			float _DissolveAmount;

			float _Speed;

			float _IriMag;

			float _Progress;

			float _WhereToCurveEnd;

			float _TestVariable;

			uniform sampler2D _IriTex; uniform float4 _IriTex_ST;

			float _EndSmoothener;

			float4 _RimColor;
			float _RimAmount;
			float _RimThreshold;


			v2f vert(appdata v)
			{
				v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.YScale = length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y));

				o.YourSin = clamp(abs(sin(20 * (44 / 7) * (o.uv.y /*+ _Progress */ + _Speed * _Time.x))) * 0.8f - 0.7f, 0, 1);
				v.vertex.xz *= (1 + 0.5 * o.YourSin);

				float value = (v.vertex.y + 1) / 2;
				o.additional = value;
				//v.vertex.xz *= clamp(8 * value, 0.1, 1);

				//if (value >= _WhereToCurveEnd)
				{
					float magvalue = log2(((1 - value) / (1 - _WhereToCurveEnd)) + 1);
					v.vertex.xz *= magvalue; //1 -> 0
					v.vertex.y -= _EndSmoothener * (1 - magvalue);
				}

				o.vertex = UnityObjectToClipPos(v.vertex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				//o.uv.y *= o.YScale;

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				o.viewDir = WorldSpaceViewDir(v.vertex);

				UNITY_TRANSFER_FOG(o, o.vertex);
				return o;
			}

			float4 _Color;

			float _CutoffFromBottom;

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				//fixed4 col = tex2D(_MainTex, i.uv);

			float2 UV = i.uv;
			UV.y *= i.YScale;
			UV.y += /*_Progress*/ 8 * _Speed * _Time.x;

			if (i.additional <= _CutoffFromBottom) clip(-1);

				fixed4 col = tex2D(_MainTex, UV);

				float NoiseDistort = tex2D(_WaterNoise, UV).r;

				//col *= (1 + 0.3f * NoiseDistort);

				//col += 1.5f * clamp((i.additional - 0.85) / 0.15, 0, 1);
				//col += 0.6 * tex2D(_StreaksTex, i.uv).r;

				float  NDotV = dot(normalize(i.worldNormal), normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz));
				float2 NDotV2 = float2(NDotV, NDotV);
				float4 IriColor = tex2D(_IriTex, TRANSFORM_TEX(NDotV2, _IriTex));


				//col += rim;


				float Additional2 = i.additional;

				//if (i.additional > _TestVariable) clip(-1);

				//Additional2 = lerp(1 - i.additional, _TestVariable * 4 - 2, Additional2);

				float dissolvevalue = tex2D(_DissolveTex, UV).r - _DissolveAmount + Additional2;
				clip(dissolvevalue);
				float LineTransitionAmount = 0;
				if (dissolvevalue >= 0.03)
				{
					LineTransitionAmount = 1 - smoothstep(0.03, 0.08, dissolvevalue);
				}
				else
				{
					LineTransitionAmount = smoothstep(-0.02, 0.03, dissolvevalue);
				}
					//col *= lerp(1, 2, LineTransitionAmount);

				clip((1 - Additional2) - _TestVariable);
				if ((1 - Additional2) - _TestVariable <= 0.03)
				{
					LineTransitionAmount += smoothstep(-0.02, 0.03, (1 - Additional2) - _TestVariable);
				}
				else if ((1 - Additional2) - _TestVariable <= 0.08)
				{
					LineTransitionAmount += 1 - smoothstep(0.03, 0.08, (1 - Additional2) - _TestVariable);
				}


				col *= 1 - clamp(10 * (0.1 - i.additional), 0, 1);
				col *= clamp(((1 - Additional2) - _TestVariable) / 0.08, 0, 1);

				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return  _Color * col * (0.3f * NoiseDistort + 1.5f * clamp((i.additional - 0.85) / 0.15, 0, 1) + lerp(0, 1, LineTransitionAmount) + IriColor * _IriMag + i.YourSin * 5 + 0.6 * tex2D(_StreaksTex, i.uv).r);
			}
			ENDCG
		}

    }
}
