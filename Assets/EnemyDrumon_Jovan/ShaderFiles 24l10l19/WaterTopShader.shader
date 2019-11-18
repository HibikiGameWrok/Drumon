Shader "Unlit/WaterTopShader"
{
	Properties
	{
		_Color("Tint", Color) = (1, 1, 1, .5)
		_FoamC("Foam", Color) = (1, 1, 1, .5)
		_MainTex("Main Texture", 2D) = "white" {}
		_TextureDistort("Texture Wobble", range(0,1)) = 0.1
		_NoiseTex("Extra Wave Noise", 2D) = "white" {}
		_Speed("Wave Speed", Range(0,1)) = 0.5
		_Amount("Wave Amount", Range(0,1)) = 0.6
		_Scale("Scale", Range(0,1)) = 0.5
		_Height("Wave Height", Range(0,1)) = 0.1
		_Foam("Foamline Thickness", Range(0,10)) = 8

		_RippleSpread("Ripple Spread", Range(0, 100)) = 10
		_RippleHeightScale("Ripple Height Scale", Range(0, 3)) = 1

		//_MyNoise0("the2", 2D) = "white" {}
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque"  "Queue" = "Transparent" }
			LOD 100
			Blend OneMinusDstColor One
			Cull Off

			GrabPass{
				Name "BASE"
				Tags{ "LightMode" = "Always" }
					}
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
					float2 uv : TEXCOORD3;
					UNITY_FOG_COORDS(1)
					float4 vertex : SV_POSITION;
					float4 scrPos : TEXCOORD2;//
					float4 worldPos : TEXCOORD4;//

					float tDist : TEXCOORD0;
					float addCol : TEXCOORD1;
				};

				float _TextureDistort;
				float4 _Color;
				sampler2D _CameraDepthTexture; //Depth Texture
				sampler2D _MainTex, _NoiseTex;//
				float4 _MainTex_ST;
				float _Speed, _Amount, _Height, _Foam, _Scale;//
				float4 _FoamC;

				float4 Ripples[70];
				int RipplesAmount = 0;

				sampler2D _MyNoise0, _MyNoise1;
				float _MyNoiseProgress;
				float2 _MyNoiseOffset0, _MyNoiseOffset1;

				float _RippleSpread;
				float _RippleHeightScale;

				fixed4 triplanar(float3 blendNormal, float4 texturex, float4 texturey, float4 texturez)
				{
					float4 triplanartexture = texturez;
					triplanartexture = lerp(triplanartexture, texturex, blendNormal.x);
					triplanartexture = lerp(triplanartexture, texturey, blendNormal.y);
					return triplanartexture;
				}

				v2f vert(appdata v)
				{
					v2f o;
					UNITY_INITIALIZE_OUTPUT(v2f, o);
					float4 tex = tex2Dlod(_NoiseTex, float4(v.uv.xy, 0, 0));//extra noise tex

					//if (mul(unity_ObjectToWorld, v.vertex).x > 1) v.vertex.y += 10000;

					v.vertex.y += sin(_Time.z * _Speed + 10 * (v.vertex.x * v.vertex.z * _Amount * tex)) * _Height;//movement
					o.worldPos = mul(unity_ObjectToWorld, v.vertex);

					o.tDist = 999;
					o.addCol = 0;

					for (int i = 0; i < RipplesAmount; ++i)
					{
						if (Ripples[i].z <= 0) continue;

						float xDist = (Ripples[i].x - o.worldPos.x);
						float zDist = (Ripples[i].y - o.worldPos.z);
						o.tDist = sqrt(pow(xDist, 2) + pow(zDist, 2));
						if (o.tDist < _RippleSpread)
						{
							float OppositeTime = Ripples[i].w - Ripples[i].z;
							float SmallRippleTime = 1.3;
							float SmallRippleInterval = 0.25;

							float RecentStartInterval = (float)((int)floor(OppositeTime / SmallRippleInterval)) * SmallRippleInterval;

							//RecentStartInterval = 0;

							//while (RecentStartInterval + SmallRippleTime > OppositeTime)
							while (RecentStartInterval + SmallRippleTime > Ripples[i].w)
							{

								RecentStartInterval -= SmallRippleInterval;
								
								if (RecentStartInterval <= 0.0)
								{
									RecentStartInterval = 0;
									break;
								}
							}

							int debugcount = 0;
							while (RecentStartInterval + SmallRippleTime >= OppositeTime && RecentStartInterval >= 0)
							{
								debugcount++;

								float SubsequentWaveMagnifier = 1;
								if (RecentStartInterval > /*0.0*/ SmallRippleInterval) //If RecentStartInterval is not the first wave
								{
									SubsequentWaveMagnifier *= 0.5;
								}

								if (RecentStartInterval <= 0.0)
								{
									break;
								}

								//This lerp determines each small ripple's spread (e.g "-10,1" means it will be 1/11 of the entire ripple's radius)
								//Only adjust the "-10", not the "1"
									float RippleCol = lerp(-15 - 10 * (1 - SubsequentWaveMagnifier), 1, 1 - abs((o.tDist / _RippleSpread) - (OppositeTime - RecentStartInterval) / SmallRippleTime));
									float Val = lerp(0, RippleCol, (SmallRippleTime - (OppositeTime - RecentStartInterval)) / SmallRippleTime);
									
									//Prevents reducing additional small ripples
									if (Val <= 0) Val = 0;

									//Adjusts the small ripple height
									Val *= SubsequentWaveMagnifier;

									//Blend near the end
									if (o.tDist >= 0.9 * _RippleSpread && Val > 0)
									{
										o.addCol += _RippleHeightScale * lerp(-Val, Val, (_RippleSpread - o.tDist) / (0.1 * _RippleSpread));
									}
									else
									{
										o.addCol += _RippleHeightScale * Val;
									}

								//if (debugcount == 1)
								//{
								//	if (o.tDist < _RippleSpread * 0.5)
								//	{
								//		o.addCol = 1;
								//	}
								//}
								//else if (debugcount == 2)
								//{
								//	if (o.tDist > _RippleSpread * 0.8)
								//	{
								//		o.addCol = 1;
								//	}
								//}


								RecentStartInterval -= SmallRippleInterval;
							}

							//{
							//	float RippleCol = lerp(-3, 1, 1 - abs((o.tDist / _RippleSpread) - (Ripples[i].w - Ripples[i].z) / SmallRippleTime));
							//	float Val = lerp(0, RippleCol, Ripples[i].z / Ripples[i].w);

							//	if (o.tDist >= 0.9 * _RippleSpread && Val > 0)
							//	{
							//		o.addCol += lerp(-Val, Val, (_RippleSpread - o.tDist) / (0.1 * _RippleSpread));
							//	}
							//	else
							//	{
							//		o.addCol += Val;
							//	}
							//}

							//if (o.addCol > 1) o.addCol = 1;

							//if (o.addCol <= 0)
							//{
							//	o.addCol = 0;
							//}
							//else
							//{
							//	v.vertex.y += o.addCol * 0.5;
							//}
						}
					}

					if (o.addCol > 1) o.addCol = 1;

					if (o.addCol <= 0)
					{
						o.addCol = 0;
					}
					else
					{
						v.vertex.y += o.addCol;
					}

					o.vertex = UnityObjectToClipPos(v.vertex);

					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.scrPos = ComputeScreenPos(o.vertex);
					UNITY_TRANSFER_FOG(o,o.vertex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					float3 CamForward = mul((float3x3)unity_CameraToWorld, float3(0,0,1));
					//float3 CamForward = distance(UNITY_PROJ_COORD(i.scrPos), float3(0, 0, 0));

					//SelfNote: This calculates the depth distance of the vertex (screen position) from the next surface behind it, in the direction of where your camera is facing
					//This depth value has to be low enough for the foamline to appear.
					//I still don't know how the w value works but its necessary for this.
					half depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.scrPos))); // depth
					depth -= i.scrPos.w;

					half4 col, colBelow;

					//colBelow = tex2D(_MainTex, ((depth * CamForward) + i.worldPos.xz));

					fixed distortx = tex2D(_NoiseTex, (i.worldPos.xz * _Scale) + (_Time.x * 2)).r;// distortion alpha
					distortx += tex2D(_MyNoise0, ((i.worldPos.xz + _MyNoiseOffset0) * _Scale) + (_Time.x * 2)).r * ((4 - _MyNoiseProgress)/4) + (float)tex2D(_MyNoise1, ((i.worldPos.xz + _MyNoiseOffset1) * _Scale) + (_Time.x * 2)).r * (_MyNoiseProgress/4);
					distortx /= 2;

					col = tex2D(_MainTex, (i.worldPos.xz * _Scale) - (distortx * _TextureDistort));// texture times tint;        				

					float tempscale = _Scale;
					float3 thePos = (depth * CamForward) + i.worldPos + 0.4f * (i.worldPos - _WorldSpaceCameraPos);
					float3 blendNormal = saturate(pow(-CamForward * 1.4, 4));
					float4 xc = tex2D(_MainTex, float2((thePos.z ) * tempscale, (thePos.y) * (tempscale / 4)) + (distortx)/2);
					float4 zc = tex2D(_MainTex, float2((thePos.x ) * tempscale, (thePos.y) * (tempscale / 4)) + (distortx)/2);
					float4 yc = tex2D(_MainTex, (float2(thePos.x, thePos.z ))* tempscale + (distortx)/2);
					colBelow = triplanar(blendNormal, xc, yc, zc);

					tempscale = _Scale * 0.5;
					xc = tex2D(_MainTex, float2((thePos.z) * tempscale, (thePos.y) * (tempscale / 4)) - (distortx) / 2);
					zc = tex2D(_MainTex, float2((thePos.x) * tempscale, (thePos.y) * (tempscale / 4)) - (distortx) / 2);
					yc = tex2D(_MainTex, (float2(thePos.x, thePos.z))* tempscale - (distortx) / 2);
					colBelow *= triplanar(blendNormal, xc, yc, zc);
					//colBelow /= 2;

					colBelow += float4(0, 0, 0.3, 0);

					if (col.r > 0.95 && col.g > 0.95 && col.b > 0.95)
					{
						col *= _Color;
					}
					else
					{
						col *= _Color;
						col = (col + col + colBelow) / 3;
					}

					//SelfNote: The higher the saturate value is, the more strict it is.
					half4 foamLine = 1 - saturate(_Foam * depth);// foam line by comparing depth and screenposition
					//SelfNote: step function will return 0 if foamline is below 0.4 * distortx, else return 1
					col += (step(0.4 * distortx,foamLine) * _FoamC); // add the foam line and tint to the texture				

					col += i.addCol * 0.4f;

					col = saturate(col) * col.a;

					//col = tex2D(_RenderTexture, i.uv);

					return col;
				}

				ENDCG
			}
		}
}
