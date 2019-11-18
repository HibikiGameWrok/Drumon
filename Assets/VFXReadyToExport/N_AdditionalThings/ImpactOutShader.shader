Shader "Unlit/ImpactOutShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color0("Color0", Color) = (1, 1, 1, 1)
		_Color1("Color1", Color) = (1, 1, 1, 1)

		_Magnification("Magnification", Float) = 1
		_UVYHeight("UV Y Height", Float) = 0

		_SmoothInterval("Smooth Interval", Range(0, 1)) = 0.5

		_Mod0("Mod 0", Float) = 1.05
		_Mod1("Mod 1", Float) = 3
		_Mod2("Mod 2", Float) = 1.005
		_Mod3("Mod 3", Float) = 30

			_Progress("Progress Debug", Range(0, 1)) = 0

			_Brightness("Brightness", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

		Blend OneMinusDstColor One

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
				float HeightValue : TEXCOORD2;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float _Mod0;
			float _Mod1;
			float _Mod2;
			float _Mod3;

			float _Progress;

            v2f vert (appdata v)
            {
                v2f o;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);

				o.HeightValue = v.vertex.y + 1;

				if (o.HeightValue >= 0.15) v.vertex.xz *= pow(lerp(_Mod0, 1.0, (o.HeightValue - 0.15) / 0.85), _Mod1);
				else v.vertex.xz *= pow(lerp(1.0, _Mod2, o.HeightValue / 0.15), _Mod3);

                o.vertex = UnityObjectToClipPos(v.vertex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

			float4 _Color0;
			float4 _Color1;
			float _Magnification;
			float _UVYHeight;

			float _SmoothInterval;

			float _Brightness;

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                //fixed4 col = tex2D(_MainTex, i.uv);

				fixed4 col = _Color0;

			float MaxHeight = tex2D(_MainTex, float2(i.uv.x, _UVYHeight /*+ 10 * _Time.x*/)).r * _Magnification * abs(sin(3.14 * (_Progress))) /*+ 2 * (0.1 * sin(5 * _Time.z)) - 0.2*/;

			if (i.HeightValue > MaxHeight) clip(-1);
			else
			{
				col = lerp(_Color0, _Color1, smoothstep(MaxHeight * _SmoothInterval - 0.05, MaxHeight * _SmoothInterval + 0.05, i.HeightValue));
			}

			col *= _Brightness;

                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
