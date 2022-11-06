# 管理器

所有需要的Manager都内置于场景Core中，需要将Core场景加入到Build Setting 中的Scene列表中去。

CoreLoader负责读取Core场景，即初始化所有的Manager

## 音频管理

位置：Runtime/Manager/AudioManager

能够播放两种音频（持续不间断的BGM、短时间的SFX）

调用该单例类的方法即可。

## UI管理

# 角色控制器

## Controller 3D Base

位置：Runtime/Controller/3DBase

3D角色控制器的基类，基于Root Motion、变量混合树控制的动画系统与Player Input System。
内置一套含Root Motion的行走、奔跑、跳跃Animator、输入系统。