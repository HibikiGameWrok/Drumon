Shader "Unlit/CatchAnimSihoul"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}

		_Color("Color", Color) = (1, 1, 1, 1)

		_Progress ("Progress", Range(0, 1)) = 0

		_TargetPos ("Target Position", Vector) = (1, 1, 1)
		_CurvedDistortion ("Curved Distortion", Vector) = (1, 1, 1)

		_DissolveTex("Dissolve Texture", 2D) = "white" {}
		_DissolveProgress("Dissolve Progress", Range(0, 1)) = 0

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

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
					float4 pos : SV_POSITION;

				float3 worldNormal : NORMAL;

				float3 viewDir : TEXCOORD2;

				float3 worldPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			sampler2D _NoiseTex;

			float _Progress;
			float3 _TargetPos;
			float3 _CurvedDistortion;

            v2f vert (appdata v)
            {
                v2f o;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.worldNormal = UnityObjectToWorldNormal(v.normal);

				o.viewDir = WorldSpaceViewDir(v.vertex);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				//_Progress = fmod(_Time.x * 40, 1);

				float TrueProgress = clamp(_Progress + _Progress * tex2Dlod(_NoiseTex, float4(o.uv.xy, 0.0, 0.0)).r, 0, 1);
				float3 targetpos = _TargetPos;
				targetpos += _CurvedDistortion * (1 - TrueProgress);
				float3 difference = targetpos - o.worldPos;	
				difference *= TrueProgress;
				v.vertex += mul(unity_WorldToObject, difference);

				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.pos = UnityObjectToClipPos(v.vertex);

                UNITY_TRANSFER_FOG(o,o.pos);

                return o;
            }

			sampler2D _DissolveTex;
			float _DissolveProgress;

			float4 _Color;
			
            fixed4 frag (v2f i) : SV_Target
            {
				float dissolvevalue = tex2D(_DissolveTex, i.uv).r - _DissolveProgress;
				clip(dissolvevalue);

                // sample the texture
                fixed4 col = _Color;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
