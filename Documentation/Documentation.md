# 管理器

所有需要的Manager都内置于场景Core中，需要将Core场景加入到Build Setting 中的Scene列表中去。

CoreLoader负责读取Core场景，即初始化所有的Manager

## 音频管理器

位置：Runtime/Manager/AudioManager

能够播放两种音频（持续不间断的BGM、短时间的SFX）

调用该单例类的方法即可。

## UI管理器

位置：Runtime/Manager/UIManager

完全通过代码对UI进行管理。

全局只存在一个Canvas（已在CoreLoader中初始化好）。

Canvas下挂载需要使用的UI，每个UI需要挂载一个脚本（继承于UIBase，写法如ExampleUI）。

回调函数依靠UIEventTrigger管理。

## Mono管理器

位置：Runtime/Manager/MonoManager

单例类型的管理器，主要负责
1.调用一些需要Update的Action
2.调用一些协程

防止协程/函数的调用受到原脚本生命周期的干扰。

# 角色控制器

## Controller 3D Base

位置：Runtime/Controller/3DBase

3D角色控制器的基类，基于Root Motion、变量混合树控制的动画系统与Player Input System。
内置一套含Root Motion的行走、奔跑、跳跃Animator、输入系统。

# 其他GamePlay相关系统

## 矩形网格系统（Grid）

位置：Runtime/Gameplay/Grid
命名空间：SLFramework.Gameplay.Grid

跟着Code Monkey实现的泛型矩形网格系统。

## 有限状态机（FSM）

位置：Runtime/Gameplay/FSM
命名空间：SLFramework.Gameplay.FSM

使用前，需要自己定义两个枚举类（一个为状态、一个为事件）

每一个状态都需要一个类（实现IState接口），用于存储该状态所需的数据，实现OnEnter、OnUpdate、OnExit函数等...（公有数据可以存一个blackboard的引用，私有数据可以直接存在类中）

新建状态机后，为其添加状态及转换，在Update中调用fsm.Update()即可，并在恰当的时机调用TriggerEvent来实现状态转换。





# Debug相关