# Animation旧版动画系统

## 一、Animation与Animator

在unity的老版本中只有Animation组件，在4.6版本以后则增添了Animator组件，如果只是控制一个动画的播放则用Animaton组件，如果是很多动画之间相互转换则使用Animator组件，它们两者的区别就是Animator有一个动画控制器（俗称动画状态机），使用它来进行动画切换是非常方便的，但缺点是占用内存比Animaton组件大。

## 二、动画片段的分类

- **关节动画：** 将角色分为若干个独立部分，一个部分对应一个网格模型，部分动画连接成一个整体动画，可以用来创建更真实的物理效果

- **单一网格模型动画：** 由一个完整的网格模型构成，在动画序列帧里记录各个时间点的数据信息，然后通过插值运算实现动画效果，角色动画较真实

- **骨骼动画：** 应用最广泛的动画方式，集成了以上两种动画的优先，有关节相连，皮肤作为单一网格蒙在骨骼外。

## 三、Animation常用API

### 3.1 Animation

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animation/Animation.png)

- **Animation:** 默认动画片段，启用自动播放（Play Automatically）时默认动画片段将被播放

- **Animations：** 可以从脚本中访问的动画片段列表

- **Play Automatically：** 自动播放，启动游戏时自动播放的动画

- **Animate Physics：** 动画是否需要进行物理交互（几乎不使用）

- **Culling Type：** 动画在不可见时是否还需要继续播放，默认即可

- **AlwaysAnimate：** 始终为整个角色生成动画，即使物体在屏幕外也播放动画

- **BasedOnRenderers：** 当渲染不可见时禁用动画

### 3.2 AnimationClip

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animation/Wrap%20mode.png)

- **Wrap Mode:** 枚举，用于确定当时间超出AnimationClip或AnimationCurve的关键帧范围时如何处理时间

- **Default:** 从动画剪辑中读取循环模式（默认是Once），项目开发中一般使用Default，即默认使用美术提供的循环方式即可

- **Once：** 当时间播放到末尾的时候停止动画的播放

- **Loop：** 当时间播放到末尾的时候重新从开头进行播放

- **PingPong：** 当时间播放到末尾时，时间将在开头和结尾之间来回弹跳

- **ClampForever：** 播放到结尾的时候，将持续播放最后一帧不停止

**AnimationClip是动画数据的容器，它定义了动画的关键帧和曲线**

### 

### 3.3 AnimationState

AnimationState类表示动画状态机中的单个状态，通过这个类可以在播放任何动画时修改速度、权重、时间和层级，还可以设置动画混合和WrapMode

```c#
            public bool enabled { get; set; }      
    
            public WrapMode wrapMode { get; set; }      
    
            public AnimationClip clip { get; }
    
            public float time { get; set; }//当前动画片段播放到哪里   
    
            public float normalizedTime { get; set; }//标量化的时长    
    
            public float speed { get; set; }//速度
    
            public float normalizedSpeed { get; set; }//标量化的速度
    
            public float length { get; }//动画片段的总时长
    
            //常用于动画混合
            public int layer { get; set; }
    
            public float weight { get; set; }//CrossFade  weight越高 效果越明显
    
            public AnimationBlendMode blendMode { get; set; }   
```

**AnimationState控制着AnimationClip的播放，包括播放速度、权重、时间和层等，因此，可以将AnimationState看作是控制AnimationClip播放的接口**

## API的使用

案例1通过按键控制播放不同的动画

```c#
 public class InputAnimations : MonoBehaviour
    {
        private Animation ani;
        void Awake()
        {
            ani = GetComponent<Animation>();
        }
        void Start()
        {
            //从名为ani的Animation组件中获取名为playerAnimation1的AnimationState，
              并将其存储在名为state的变量中
            AnimationState state = ani["playerAnimation1"];
            state.wrapMode = WrapMode.PingPong;
        }
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ani.Play("PlayerAnimation1");
            if (Input.GetKeyDown(KeyCode.W))
                ani.Play("PlayerAnimation2");
            if (Input.GetKeyDown(KeyCode.E))
                ani.Play("PlayerAnimation3");
        }
    }

```
案例2：如何实现一个动画的倒放

```c#
using UnityEngine;
    public class AnimationTest : MonoBehaviour
    {
        private Animation ani;
        private void Awake()
        {
            ani = GetComponent<Animation>();
        }
        private void Start()
        {
            string clipName = "PlayerAnimation1"；
            //在Animation组件ani中获取名为PlayerAnimation1的AnimationState并存储
              在state变量中
            AnimationState state = ani[clipName];
            //将开始播放动画的时间设置为动画片段的末尾，这样动画的播放是从动画最后一帧
              开始播放         
            state.time = state.length;
            //动画将以正常速度倒序播放         
            state.speed = -1;         
            ani.Play(clipName);
        }
    }
```

案例3：动画帧事件

第一种方式可以在脚本中定义一个方法，这里就是直接输出一句话，然后在AnimationClip中选择一个具体的时间点添加事件，并且在inspector找到需要的方法添加事件

```c#
  public void TestFunc()
        {
            Debug.Log("Player走到路中央了");
        }
```

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animation/%E5%8A%A8%E7%94%BB%E5%B8%A7%E4%BA%8B%E4%BB%B6.jpg)

第二种方式是通过代码手动添加动画帧事件

```c#
public class InputAnimations : MonoBehaviour
    {
        private Animation ani;
        string clipName = "playerAnimation1";
        private void Awake()
        {
            ani = GetComponent<Animation>();
        }
        private void Start()
        {
            AddEvent();
        }
        void AddEvent()
        {
            AnimationClip clip = ani.GetClip(clipName);
            AnimationEvent aEvent = new AnimationEvent();
            aEvent.functionName = "TestFunc2";
            //动画触发的时间为动画播放20%时触发
            aEvent.time = clip.length * 0.2f;
            //传递脚本挂载的游戏对象为参数
            aEvent.objectReferenceParameter = gameObject;          
            clip.AddEvent(aEvent);
        }

```