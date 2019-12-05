Shader "Unlit/WaterGunShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

		_Color ("Color", Color) = (0, 0, 1, 1)

		_WaterNoise ("Water Noise", 2D) = "white" {}

			_IriTex("IriTexture", 2D) = "white" {}
			_IriMag("IriMagnification", Float) = 1

				_Progress("Progress", Float) = 0

				_CutoffFromBottom("Cutoff From Bottom", Range(0, 1)) = 0

				_Speed("Speed", Float) = 1

				_EndSmoothener("End Smoothener", Float) = 0.1

				_StartCompress("Start Compress", Range(0, 1)) = 1
				_EndCompress("End Compress", Range(0, 1)) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        LOD 100
		Blend OneMinusDstColor One
		//Cull Off

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
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _WaterNoise;

			float _Speed;

			float _IriMag;

			float _Progress;

			uniform sampler2D _IriTex; uniform float4 _IriTex_ST;

			float _EndSmoothener;

			float _StartCompress;
			float _EndCompress;

            v2f vert (appdata v)
            {
                v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.YScale = length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y));

				//Value from Start to End of Water Gun : 0 -> 1
				float value = (v.vertex.y + 1) / 2;

				//Compress Start
				v.vertex.y += 1;
				v.vertex.y -= 0.125 * 2;
				if (value > 0.125 && value < 0.9)
				{
					v.vertex.y *= _StartCompress;
				}
				else if (value >= 0.9)
				{
					v.vertex.y -= ((0.9 - 0.125) * 2) * (1 - _StartCompress);
				}
				v.vertex.y += 0.125 * 2;
				v.vertex.y -= 1;

				//not the same as 'value'
				float CompressStartValue = (v.vertex.y + 1) / 2;

				//Bumps
				//o.YourSin = abs(sin(20 * (44 / 7) * (o.uv.y + _Speed * _Time.x))) * 0.2f;
				o.YourSin = abs(sin(5 * (22 / 7) * (CompressStartValue + 6 * _Speed * _Time.x))) * 0.2f;
				v.vertex.xz *= (1 + o.YourSin);

				//Bottlenose Start
				v.vertex.xz *= clamp(8 * value, 0.1, 1);

				//Smooth End
				if (value >= 0.9)
				{
					float magvalue = log2(((1 - value) / 0.1) + 1);
					v.vertex.xz *= magvalue; //1 -> 0
					v.vertex.y -= _EndSmoothener * (1 - magvalue);
				}
				
				//Compress End
				v.vertex.y -= 1;
				v.vertex.y += (1 - 0.9) * 2;
				if (value < 0.9)
				{
					v.vertex.y *= (1 - _EndCompress);
				}
				v.vertex.y -= (1 - 0.9) * 2;
				v.vertex.y += 1;

                o.vertex = UnityObjectToClipPos(v.vertex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);
		
				//o.uv.y *= o.YScale;

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

                UNITY_TRANSFER_FOG(o,o.vertex);
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

			if (i.uv.y <= _CutoffFromBottom) clip(-1);

				fixed4 col = _Color * tex2D(_MainTex, UV);
				
				float NoiseDistort = tex2D(_WaterNoise, UV).r;

				col *= (1 + 0.3f * NoiseDistort);

				float  NDotV = dot(normalize(i.worldNormal), normalize(_WorldSpaceCameraPos.xyz - i.worldPos.xyz));
				float2 NDotV2 = float2(NDotV, NDotV);
				float4 IriColor = tex2D(_IriTex, TRANSFORM_TEX(NDotV2, _IriTex));

				col += i.YourSin * 2.5;

				col += IriColor * _IriMag;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
