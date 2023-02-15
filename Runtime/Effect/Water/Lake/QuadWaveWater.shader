Shader "SLFramework/Water/Lake/QuadWaveWater"
{
    Properties
    {
    	_DepthShallowColor("Depth Shallow Color", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthDeepColor("Depth Deep Color", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1

        _WaveTexture("Wave Texture", 2D) = "white"{}
        _WaveColor("Wave Color", Color) = (1,1,1,1)
        _WaveMaxDistance("Wave Maximum Distance", Float) = 0.2
        _WaveCutOff("Wave Cut Off", Range(0,1)) = 0.5
        _WaveStrength("Wave Strength", Float) = 0.2
        _WaveCount("Wave Count", Float) = 3
        _WaveSpeed("Wave Speed", Float) = 3
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
                float2 texcoord : TEXCOORD;         // ����UV
            };

            struct v2f
            {
                float4 posCS : SV_POSITION;        // �ü��ռ�����
                float uv : TEXCOORD0;               // ����UV
                float4 posSS : TEXCOORD1;
            };



            v2f vert(appdata v)
            {
                v2f o;
                o.posCS = UnityObjectToClipPos(v.posOS);        // �ü��ռ����꣨MVP�任��
                o.posSS = ComputeScreenPos(o.posCS);            // ��Ļ�ռ�����
                o.uv = v.texcoord;
                return o;
            }

            sampler2D _CameraDepthTexture;          // �����ͼ

            float4 _DepthShallowColor;              // ��ǳ����ɫ
			float4 _DepthDeepColor;                 // �����ɫ
            float _DepthMaxDistance;                // ˮ��������루���ڵ��������ȵ�ˮ����ɫȡ���

            sampler2D _WaveTexture;                 // ˮ��������ͼ
			float4 _WaveColor;                      // ˮ����ɫ
            float _WaveMaxDistance;                 // С�������ȵĵط��Ż����ˮ��
            float _WaveCutOff;
            float _WaveStrength;                    // ˮ��ǿ��
            float _WaveCount;                       // 
            float _WaveSpeed;

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
                

                // �����ܱ߻����ˮ��

                // �����С��_WaveMaxDistanceʱ�������ˮ��
                // ���Խǳ��ShallowԽ��
                float waveShallow01 = 1 - saturate(depthDiff / _WaveMaxDistance);

                // �����������uv����ʱ��仯��
                float2 uv = float2(i.uv.x, (waveShallow01 + _Time.y * _WaveSpeed) * _WaveCount);

                // ������������
                // ���Խǳ��ֵԽ��
                // ˮ������Խǿ��ֵԽ��
                float wave = tex2D(_WaveTexture, uv).r * waveShallow01 * _WaveStrength;

                wave = step(_WaveCutOff, wave);

                return lerp(waterColor, _WaveColor, waveShallow01 + wave);
            }
            ENDCG
        }
    }
}
