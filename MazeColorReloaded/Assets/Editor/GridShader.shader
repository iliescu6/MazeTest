Shader "Amber/GridShader"
{
    Properties
    {
		_MainTex("Main Texture", 2D) = "white"{}
		_gridSize("Grid Size", Range(0, 10000)) = 1
		_gridWidth("Grid Width", Range(0, 30000)) = 1
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType"="Transparent" }
		//Lighting Off
		//Fog{ Mode Off }
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		//Cull Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float3 uv : TEXCOORD0;
				float2 uv2 : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

			float _gridSize;
			float _gridWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv2 = TRANSFORM_TEX(v.uv, _MainTex);
				o.uv = v.vertex.xyz;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
				fixed3 coord = i.uv.xyz;
//				fixed2 grid = abs(frac((coord - 0.5) * _gridSize) - 0.5) * 30;// / (fwidth(coord) * _gridWidth);
				fixed2 grid = abs(frac(coord * _gridSize) - 0.5) / (fwidth(coord) * _gridWidth);
				float lin = min(grid.x, grid.y);
				//float lin = abs(frac(coord.z * 20) - 0.2) / (abs(ddx(coord)) + abs(ddy(coord))) * 0.1f;


				fixed4 col = fixed4( 1 - min(lin, 1.0), 1 - min(lin, 1.0), 1 - min(lin, 1.0), 1 - min(lin, 1.0));
				//col += tex2D(_MainTex, i.uv2);
                return col;
            }
            ENDCG
        }
    }
}
