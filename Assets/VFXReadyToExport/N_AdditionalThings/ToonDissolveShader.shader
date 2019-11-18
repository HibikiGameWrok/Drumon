Shader "Unlit/ToonDissolveShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
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

		_ShadowTier2Threshold("Shadow Tier 2 Threshold", Range(-1, 0)) = -0.3
		_ShadowTier3Threshold("Shadow Tier 3 Threshold", Range(-1, 0)) = -0.6

		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveProgress("Dissolve Progress", Range(0, 1)) = 0

		_DissolveColor("Dissolve Color", Color) = (1, 0.5, 0.5, 1)

	}
		SubShader
		{
			Tags
	{
		"LightMode" = "ForwardBase"
		"PassFlags" = "OnlyDirectional"
	}
			LOD 100
			Cull Off

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma multi_compile_fwdbase
				// make fog work
				#pragma multi_compile_fog

				#include "UnityCG.cginc"
				#include "Lighting.cginc"
				#include "AutoLight.cginc"

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
					float4 pos : SV_POSITION;

					float3 worldNormal : NORMAL;

					float3 viewDir : TEXCOORD4;

					SHADOW_COORDS(5)
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _Color;
				float4 _DissolveColor;

				float4 _AmbientColor;

				float _Glossiness;
				float4 _SpecularColor;

				float4 _RimColor;
				float _RimAmount;
				float _RimThreshold;

				float _ShadowTier2Threshold;
				float _ShadowTier3Threshold;

				sampler2D _DissolveTex;
				float _DissolveProgress;

				v2f vert(appdata v)
				{
					v2f o;
					o.pos = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					//UNITY_TRANSFER_FOG(o,o.vertex);

					o.worldNormal = UnityObjectToWorldNormal(v.normal);

					o.viewDir = WorldSpaceViewDir(v.vertex);

					TRANSFER_SHADOW(o)

					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float dissolvevalue = tex2D(_DissolveTex, i.uv).r - _DissolveProgress;
					clip(dissolvevalue);
					float LineTransitionAmount = 0;

					LineTransitionAmount = 1 - smoothstep(0.03, 0.08, dissolvevalue);

					//if (dissolvevalue >= 0.03)
					//{
					//	LineTransitionAmount = 1 - smoothstep(0.03, 0.08, dissolvevalue);
					//}
					//else
					//{
					//	LineTransitionAmount = smoothstep(-0.02, 0.03, dissolvevalue);
					//}

					//// sample the texture
					fixed4 col = tex2D(_MainTex, i.uv);

					float3 normal = normalize(i.worldNormal);

					float NdotL = dot(_WorldSpaceLightPos0, normal);

					float shadow = SHADOW_ATTENUATION(i);

					//float lightIntensity = NdotL > 0 ? 1 : 0;
					float lightIntensity = smoothstep(0, 0.01, NdotL * shadow);
					if (NdotL < _ShadowTier3Threshold)
					{
						lightIntensity = 0;
					}
					else if (NdotL < _ShadowTier2Threshold)
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

					//// apply fog
					//UNITY_APPLY_FOG(i.fogCoord, col);
					//return col;

					col *= lerp(_Color, _DissolveColor, LineTransitionAmount);

					return /*_Color **/ col * (_AmbientColor + light + specular + rim);
				}
				ENDCG
			}


			UsePass "Legacy Shaders/VertexLit/SHADOWCASTER"
		}
}
