
Shader "Unlit/ToonMetalSmearShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Smear Noise Texture", 2D) = "white" {}

		_Color("Color", Color) = (1,0,0,1)

		[HDR]
		_AmbientColor("Ambient Color", Color) = (0.4,0.4,0.4,1)

		[HDR]
		_SpecularColor("Specular Color", Color) = (0.9,0.9,0.9,1)
		_Glossiness("Glossiness", Float) = 32

		[HDR]
		_RimColor("Rim Color", Color) = (1,1,1,1)
		_RimAmount("Rim Amount", Range(0, 1)) = 0.716
		_RimThreshold("Rim Threshold", Range(0, 1)) = 0.1

		[HDR]
		_HighlightColor("Highlight Color", Color) = (1,0.5,0.5,1)
		_HighlightVar("Highlight Variable", Range(-1, 1)) = 0.5

			[HideInInspector] _Cutoff("", Float) = 0.5
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
				#pragma shader_feature_local _ _ALPHATEST_ON 

				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				#include "AutoLight.cginc"
				#define _ALPHATEST_ON

				struct appdata
				{
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;

					float3 normal : NORMAL;
				};

				struct v2f
				{
					float2 uv : TEXCOORD0;
					float4 pos : SV_POSITION;

					float3 worldNormal : NORMAL;

					float3 viewDir : TEXCOORD4;

					SHADOW_COORDS(5)

						float3 lightDir : TEXCOORD6;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				sampler2D _NoiseTex;

				fixed4 _PrevPosition;
				fixed4 _Position;

				float4 _Color;

				float4 _AmbientColor;

				float _Glossiness;
				float4 _SpecularColor;

				float4 _RimColor;
				float _RimAmount;
				float _RimThreshold;

				float4 _HighlightColor;
				float _HighlightVar;

				v2f vert(appdata v)
				{
					v2f o;
					//o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);

					fixed4 worldPos = mul(unity_ObjectToWorld, v.vertex);

					fixed3 worldOffset = _Position.xyz - _PrevPosition.xyz; // -5
					fixed3 localOffset = worldPos.xyz - _Position.xyz; // -5

					// World offset should only be behind swing
					float dirDot = dot(normalize(worldOffset), normalize(localOffset));
					fixed3 unitVec = fixed3(1, 1, 1) * 1.3;
					worldOffset = clamp(worldOffset, unitVec * -1, unitVec);
					worldOffset *= -clamp(dirDot, -1, 0) * lerp(1, 0, step(length(worldOffset), 0));

					fixed3 smearOffset = -worldOffset.xyz * lerp(1, tex2Dlod(_NoiseTex, float4(worldPos.xy, 0, 0)), step(0, 15));
					worldPos.xyz += smearOffset;
					v.vertex = mul(unity_WorldToObject, worldPos);
					o.pos = UnityObjectToClipPos(v.vertex);

					o.worldNormal = UnityObjectToWorldNormal(v.normal);

					o.viewDir = WorldSpaceViewDir(v.vertex);
					o.lightDir = WorldSpaceLightDir(v.vertex);

					TRANSFER_SHADOW(o)

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					fixed4 col = tex2D(_MainTex, i.uv);

					if (col.a < 0.5) discard;

					float3 normal = normalize(i.worldNormal);

					float NdotL = dot(_WorldSpaceLightPos0, normal);

					float shadow = SHADOW_ATTENUATION(i);

					float lightIntensity;
					float NormalValToCheck = NdotL * shadow;
					lightIntensity = smoothstep(0, 0.01, NormalValToCheck) * 0.8 + 0.2;

					if (NdotL < -0.6)
					{
						lightIntensity = 0;
					}
					else if (NdotL < -0.3)
					{
						lightIntensity /= 2;
					}

					float4 light = lightIntensity * _LightColor0;

					float3 viewDir = normalize(i.viewDir);
					float3 halfVector = normalize(_WorldSpaceLightPos0 + viewDir);
					float NdotH = dot(normal, halfVector);
					float specularIntensity = pow(NdotH * lightIntensity, _Glossiness * _Glossiness);
					float specularIntensitySmooth = smoothstep(0.005, 0.01, specularIntensity);
					float4 specular = specularIntensitySmooth * _SpecularColor;

					float4 rimDot = 1 - dot(viewDir, normal);
					//float rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimDot);
					float rimIntensity = rimDot * pow(NdotL, _RimThreshold);//* NdotL;
					rimIntensity = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, rimIntensity);
					float4 rim = rimIntensity * _RimColor;

					float hiLi = dot(i.lightDir, normal);
					float4 highlightAlbedo = step(_HighlightVar, hiLi) * _HighlightColor;

					return _Color * col * (_AmbientColor + light + specular + rim + highlightAlbedo);
				}
				ENDCG
			}
		}
			Fallback "Legacy Shaders/Transparent/Cutout/VertexLit"
}
