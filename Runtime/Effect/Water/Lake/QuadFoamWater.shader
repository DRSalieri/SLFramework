Shader "SLFramework/Water/Lake/QuadFoamWater"
{
    Properties
    {
    	_DepthShallowColor("Depth Shallow Color", Color) = (0.325, 0.807, 0.971, 0.725)
		_DepthDeepColor("Depth Deep Color", Color) = (0.086, 0.407, 1, 0.749)
        _DepthMaxDistance("Depth Maximum Distance", Float) = 1

        _FoamColor("Foam Color", Color) = (1,1,1,1)
        _FoamMaxDistance("Foam Maximum Distance", Float) = 0.2

        _SurfaceNoise("Surface Noise", 2D) = "white" {}                                     // 噪声图
		_SurfaceNoiseScroll("Surface Noise Scroll Amount", Vector) = (0.03, 0.03, 0, 0)
		_SurfaceNoiseCutoff("Surface Noise Cutoff", Range(0, 1)) = 0.777
		_SurfaceDistortion("Surface Distortion", 2D) = "white" {}	                        // 对噪声的扰动
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
                float4 posOS : POSITION;           // 对象空间坐标
                float2 uv : TEXCOORD;               // UV
            };

            struct v2f
            {
                float4 posCS : SV_POSITION;        // 裁剪空间坐标
                float2 noiseUV : TEXCOORD0;         // 噪声纹理UV
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
                o.posCS = UnityObjectToClipPos(v.posOS);        // 裁剪空间坐标（MVP变换）
                o.posSS = ComputeScreenPos(o.posCS);            // 屏幕空间坐标
                // TRANSFORM_TEX，将贴图的tilling、offset应用到uv上
                o.noiseUV = TRANSFORM_TEX(v.uv, _SurfaceNoise);
                o.distortUV = TRANSFORM_TEX(v.uv, _SurfaceDistortion);
                return o;
            }

            sampler2D _CameraDepthTexture;          // 深度贴图

            float4 _DepthShallowColor;              // 最浅处颜色
			float4 _DepthDeepColor;                 // 最深处颜色
            float _DepthMaxDistance;                // 水面最深距离（大于等于这个深度的水面颜色取最深）

            float4 _FoamColor;
            float _FoamMaxDistance;

            float2 _SurfaceNoiseScroll;
            float _SurfaceNoiseCutoff;
            float _SurfaceDistortionAmount;

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
                

                // 越深处，Curoff越大，产生漂浮物也就越小
                float foamDiff01 = saturate(depthDiff / _FoamMaxDistance);
                float surfaceNoiseCutoff = foamDiff01 * _SurfaceNoiseCutoff;

                // 对distort采样
                float2 distortSample = (tex2D(_SurfaceDistortion, i.distortUV).xy * 2 - 1) * _SurfaceDistortionAmount;

                // 噪声UV
                // 随时间变化
                float2 noiseUV = float2((i.noiseUV.x + _Time.y * _SurfaceNoiseScroll.x) + distortSample.x,
                                        (i.noiseUV.y + _Time.y * _SurfaceNoiseScroll.y) + distortSample.y);
                // 对噪声贴图采样
                float surfaceNoiseSample = tex2D(_SurfaceNoise, noiseUV).r;

                // 受噪声影响程度
                // smoothstep = step + lerp，生成[0,1]平滑过渡值
                // 采样小于CutOff时候，得0
                // 采样大于CutOff + 0.1时，得1
                // 采样在两者中间时，插值得[0,1]中一个数（平滑的）
                float surfaceNoise = smoothstep(surfaceNoiseCutoff, surfaceNoiseCutoff + 0.1, surfaceNoiseSample);

                return lerp(waterColor, _FoamColor, surfaceNoise);
            }
            ENDCG
        }
    }
}
