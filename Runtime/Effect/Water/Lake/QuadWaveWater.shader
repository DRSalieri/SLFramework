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
                float4 posOS : POSITION;           // 对象空间坐标
                float2 texcoord : TEXCOORD;         // 纹理UV
            };

            struct v2f
            {
                float4 posCS : SV_POSITION;        // 裁剪空间坐标
                float uv : TEXCOORD0;               // 纹理UV
                float4 posSS : TEXCOORD1;
            };



            v2f vert(appdata v)
            {
                v2f o;
                o.posCS = UnityObjectToClipPos(v.posOS);        // 裁剪空间坐标（MVP变换）
                o.posSS = ComputeScreenPos(o.posCS);            // 屏幕空间坐标
                o.uv = v.texcoord;
                return o;
            }

            sampler2D _CameraDepthTexture;          // 深度贴图

            float4 _DepthShallowColor;              // 最浅处颜色
			float4 _DepthDeepColor;                 // 最深处颜色
            float _DepthMaxDistance;                // 水面最深距离（大于等于这个深度的水面颜色取最深）

            sampler2D _WaveTexture;                 // 水波噪声贴图
			float4 _WaveColor;                      // 水波颜色
            float _WaveMaxDistance;                 // 小于这个深度的地方才会产生水波
            float _WaveCutOff;
            float _WaveStrength;                    // 水波强度
            float _WaveCount;                       // 
            float _WaveSpeed;

            fixed4 frag (v2f i) : SV_Target
            {
                
                // 实现水面深处颜色深，浅处颜色浅的效果

                // 用屏幕空间坐标采样深度贴图
                // 得到[0,1]的深度值
                float depthCamToBottom01 = tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.posSS));

                // LinearEyeDepth
                // 输入[0,1]的深度值（采样深度纹理），输出[Near, Far]的深度值
                // 摄像机到底部的距离，[Near, Far]
                float depthCamToBottom = LinearEyeDepth(depthCamToBottom01);

                // 摄像机到水面的距离，[Near, Far]
                float depthCamToQuad = LinearEyeDepth(i.posCS.z);

                // 插值，即为水面深度
                float depthDiff = depthCamToBottom - depthCamToQuad;

                // 变换到[0,1]范围下，方便做lerp
                float depthDiff01 = saturate(depthDiff / _DepthMaxDistance);

                // lerp后得到水面颜色，随深度变化
                float4 waterColor = lerp(_DepthShallowColor, _DepthDeepColor, depthDiff01);
                

                // 物体周边会产生水波

                // 当深度小于_WaveMaxDistance时，会产生水波
                // 深度越浅，Shallow越大
                float waveShallow01 = 1 - saturate(depthDiff / _WaveMaxDistance);

                // 求噪声纹理的uv，随时间变化的
                float2 uv = float2(i.uv.x, (waveShallow01 + _Time.y * _WaveSpeed) * _WaveCount);

                // 采样噪声纹理
                // 深度越浅，值越大
                // 水波力度越强，值越大
                float wave = tex2D(_WaveTexture, uv).r * waveShallow01 * _WaveStrength;

                wave = step(_WaveCutOff, wave);

                return lerp(waterColor, _WaveColor, waveShallow01 + wave);
            }
            ENDCG
        }
    }
}
