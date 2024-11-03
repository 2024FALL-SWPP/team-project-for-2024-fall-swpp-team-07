// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "LiteParticleEffect/Effect Add"
{
	Properties
	{
		_MainTex ("Base (RGB) Trans (A)", 2D) = "white" {}
		_CutTex ("Cutout (A)", 2D) = "white" {}
		_Cutoff ("Alpha cutoff", Range(0,1)) = 0.0
		_UVScrollX("Main UV X Scroll",Float) = 0.0
		_UVScrollY("Main UV Y Scroll",Float) = 0.0
		_UVCutScrollX("Cut UV X Scroll",Float) = 0.0
		_UVCutScrollY("Cut UV Y Scroll",FLoat) = 0.0
		_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
	}
	SubShader 
	{
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		Blend SrcAlpha One
		Cull Off Lighting Off ZWrite Off
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_particles
			#pragma multi_compile_fog

			#include "UnityCG.cginc"
			sampler2D _MainTex;
			sampler2D _CutTex;
			float4 _MainTex_ST;
			float4 _CutTex_ST;
			float _Cutoff;
			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
			};
			struct v2f
			{
				half4 vertex : SV_POSITION;
				fixed4 color : COLOR;
				half2 uv_MainTex: TEXCOORD0;				
				half2 uv_CutOut : TEXCOORD1;
				UNITY_FOG_COORDS(2)
#ifdef SOFTPARTICLES_ON
				float4 projPos : TEXCOORD3;
#endif
				
			};


			float _UVScrollX;
			float _UVScrollY;
			float _UVCutScrollX;
			float _UVCutScrollY;

			v2f vert (appdata v)
			{
				v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				o.vertex = UnityObjectToClipPos(v.vertex);
#ifdef SOFTPARTICLES_ON
				o.projPos = ComputeScreenPos (o.vertex);
				COMPUTE_EYEDEPTH(o.projPos.z);
#endif
				o.color = v.color;
				float2 scroll = float2(_UVScrollX, _UVScrollY) * _Time.y;
				o.uv_MainTex = (v.uv + scroll) * _MainTex_ST.xy + _MainTex_ST.zw;

				scroll = float2(_UVCutScrollX, _UVCutScrollY) * _Time.y;
				o.uv_CutOut = (v.uv + scroll) * _CutTex_ST.xy + _CutTex_ST.zw;

				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			sampler2D_float _CameraDepthTexture;
			float _InvFade;

			half4 frag(v2f i) : SV_Target
			{
#ifdef SOFTPARTICLES_ON
				float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
				float partZ = i.projPos.z;
				float fade = saturate (_InvFade * (sceneZ-partZ));
				i.color.a *= fade;
#endif
				fixed4 c = tex2D(_MainTex, i.uv_MainTex);
				 c *=  i.color  ;

				fixed ca = tex2D(_CutTex, i.uv_CutOut).a;
				c.a *= ca;
				c.a = saturate((ca - _Cutoff)*100)*c.a;

				UNITY_APPLY_FOG_COLOR(i.fogCoord, c, fixed4(0,0,0,0)); // fog towards black due to our blend mode

				return c;
			}
			ENDCG
		}
	}
	Fallback "Transparent/VertexLit"
}