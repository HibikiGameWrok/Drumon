Shader "Unlit/VFX_Peck_WindCylinder"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

		_Color("Color", Color) = (1,1,1,1)

		_Brighter("Brighter", Float) = 1
		
		_Progress ("Progress", Float) = 0

		_TimesToLoop ("Times To Loop", Float) = 1

		_UvXScale("UV X Scale", Float) = 1

		_UvYScale("UV Y Scale", Float) = 1

		_SelfMoveSpeed("Speed", Float) = 0

		_ChangeToScaleBool("Change To Scale Bool", Float) = 1

		_ProgressAffectUVXBool("Progress Affect UV X Bool", Float) = 0

		_GeneralAlpha("General Alpha", Range(0, 1)) = 1

    }
    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Opaque" }
        LOD 100

		Cull Off

			 Blend SrcAlpha OneMinusSrcAlpha

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
			float _TimesToLoop;
			float _UvXScale;
			float _UvYScale;
			float _ChangeToScaleBool;
			float _ProgressAffectUVXBool;

			float4 _Color;
			float _Brighter;
			float _SelfMoveSpeed;
			float _GeneralAlpha;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				float YScale = length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y));

				o.uv.x *= _UvXScale;
				o.uv.y *= _UvYScale;

				if (_ChangeToScaleBool >= 1)
					o.uv.y *= YScale;

                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col;

			if (_Progress < i.uv.y && i.uv.y - _Progress <= 1 + _TimesToLoop)
			{
				float2 uv = i.uv;
				uv.y = fmod(i.uv.y, 1);

				col = tex2D(_MainTex, uv + fmod(_SelfMoveSpeed * float2(0, _Time.x), 1));
			}

				clip(col.r - 0.5);
                
				col *= _Color;

				col *= _Brighter;

				col.a = _GeneralAlpha * smoothstep(0, 0.05, i.uv.y);

                return col;
            }
            ENDCG
        }
    }
}
