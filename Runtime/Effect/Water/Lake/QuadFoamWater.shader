Shader "SLFramework/Water/Lake/QuadFoamWater"
{
    Properties
    {
    	_DepthShallowColor("Depth Shallow Color", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthDeepColor("Depth Deep Color", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1

        _FoamColor("Foam Color", Color) = (1,1,1,1)
        _FoamMaxDistance("Foam Maximum Distance", Float) = 0.2

        _SurfaceNoise("Surface Noise", 2D) = "white" {}                                     // ����ͼ
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}	                        // ���������Ŷ�
		_SurfaceDistortionAmount("Surface Distortion Amount", Range(0, 1)) = 0.27
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 posOS : POSITION;           // ����ռ�����
                float2 uv : TEXCOORD;               // UV
            };

            struct v2f
            {
                float4 posCS : SV_POSITION;        // �ü��ռ�����
                float2 noiseUV : TEXCOORD0;         // ��������UV
                float2 distortUV : TEXCOORD1;
                float4 posSS : TEXCOORD2;
            };

            sampler2D _SurfaceNoise;
            float4 _SurfaceNoise_ST;
            sampler2D _SurfaceDistortion;
            float4 _SurfaceDistortion_ST;

            v2f vert(appdata v)
            {
                v2f o;
                o.posCS = UnityObjectToClipPos(v.posOS);        // �ü��ռ����꣨MVP�任��
                o.posSS = ComputeScreenPos(o.posCS);            // ��Ļ�ռ�����
                // TRANSFORM_TEX������ͼ��tilling��offsetӦ�õ�uv��
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
                return o;
            }

            sampler2D _CameraDepthTexture;          // �����ͼ

            float4 _DepthShallowColor;              // ��ǳ����ɫ
			float4 _DepthDeepColor;                 // �����ɫ
            float _DepthMaxDistance;                // ˮ��������루���ڵ��������ȵ�ˮ����ɫȡ���

            float4 _FoamColor;
            float _FoamMaxDistance;

            float2 _SurfaceNoiseScroll;
            float _SurfaceNoiseCutoff;
            float _SurfaceDistortionAmount;

            fixed4 frag (v2f i) : SV_Target
            {
                
                // ʵ��ˮ�����ɫ�ǳ����ɫǳ��Ч��

                // ����Ļ�ռ�������������ͼ
                // �õ�[0,1]�����ֵ
                float depthCamToBottom01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.posSS));

                // LinearEyeDepth
                // ����[0,1]�����ֵ������������������[Near, Far]�����ֵ
                // ��������ײ��ľ��룬[Near, Far]
                float depthCamToBottom = LinearEyeDepth(depthCamToBottom01);

                // �������ˮ��ľ��룬[Near, Far]
                float depthCamToQuad = LinearEyeDepth(i.posCS.z);

                // ��ֵ����Ϊˮ�����
                float depthDiff = depthCamToBottom - depthCamToQuad;

                // �任��[0,1]��Χ�£�������lerp
                float depthDiff01 = saturate(depthDiff / _DepthMaxDistance);

                // lerp��õ�ˮ����ɫ������ȱ仯
                float4 waterColor = lerp(_DepthShallowColor, _DepthDeepColor, depthDiff01);
                

                // Խ���CuroffԽ�󣬲���Ư����Ҳ��ԽС
                float foamDiff01 = saturate(depthDiff / _FoamMaxDistance);
                float surfaceNoiseCutoff = foamDiff01 * _SurfaceNoiseCutoff;

                // ��distort����
                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

                // ����UV
                // ��ʱ��仯
                float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x,
                                        (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
                // ��������ͼ����
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

                // ������Ӱ��̶�
                // smoothstep = step + lerp������[0,1]ƽ������ֵ
                // ����С��CutOffʱ�򣬵�0
                // ��������CutOff + 0.1ʱ����1
                // �����������м�ʱ����ֵ��[0,1]��һ������ƽ���ģ�
                float surfaceNoise = smoothstep(surfaceNoiseCutoff, surfaceNoiseCutoff + 0.1, surfaceNoiseSample);

                return lerp(waterColor, _FoamColor, surfaceNoise);
            }
            ENDCG
        }
    }
}
