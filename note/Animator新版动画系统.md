# Animator新版动画系统

在Animation旧版动画系统中动画的切换是通过Animations数组来进行切换的，而在Animator中可以通过Animator Controller（动画控制器）来更方便的进行动画切换。

## 一、Animator组件介绍

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animator/animator.png)

- **Controller:** 动画控制器，功能与Animation类相同，主要控制动画的播放、切换、暂停

- **Avator：** 阿凡达，使用的骨骼文件，如果本身模型有自带的动画，那么这个数据不需要进行赋值

- **Apply Root Motion：** 使用根节点运动；即用动画来控制物体的位移，但这样难以控制，一般都是通过代码控制位移，所以一般不勾选

- **UpdateMode：** 使用默认Normal动画器标准更新，表示使用update来进行动画的更新

- **Animate Physics：** 表示使用FixedUpdate进行更新（一般用在与物体有物理交互的情况下）

- **UnscaleTime：** 表示无视TimeScale进行更新（一般用在UI动画中）

- **CullingMode：** 剔除模式

- **Always Animate：** 即使位于摄像机视野之外仍然进行动画播放的更新

- **Cull Update Transform：** 位于摄像机视野之外时停止动画播放，但是位置会继续更新

- **Cull Completely：** 位于摄像机视野之外时停止动画的所有更新

## 二、动画帧事件

### 2.1通过代码添加动画帧事件

    public class testScript : MonoBehaviour
    {
        Animator ani;
        private void Awake()
        {
            ani = GetComponent<Animator>();
        }
        private void Start()
        {
            //通过runtimeAnimatorController.animationclips获取到所有动画片段数组
            AnimationClip[] clips = ani.runtimeAnimatorController.animationClips;
            //遍历数组，为名字为testAnimation的动画片段添加事件
            foreach (AnimationClip clip in clips)
            {
                if (clip.name.Equals("testAnimation"))
                {
                    AnimationEvent aEvent = new AnimationEvent();
                    aEvent.functionName = "TestFunc";
                    aEvent.time = clip.length / 2;
                    clip.AddEvent(aEvent);
                }
            }
        }
        void TestFunc()
        {
            Debug.Log("testfunc");
        }
    }

**注！在Animator中，所有的动画片段都由Controller控制，只能通过RuntimeAnimatorController.animationClips来获取所有的动画片段，而不是像Animation中直接通过选择Animation Clip来添加动画片段**

### 2.2通过编辑器添加动画帧事件

首先在需要添加动画帧事件的动画片段中添加一个帧事件

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animator/%E5%8A%A8%E7%94%BB%E5%B8%A7%E4%BA%8B%E4%BB%B6.png)

然后点击这个图标并输入需要调用的方法名

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animator/%E8%BE%93%E5%85%A5%E6%96%B9%E6%B3%95%E5%90%8D.png)

运行游戏时在特定的时机就会触发该事件

## 三、按键控制动画切换

新版动画系统中主要通过Animator Controller中的State Machine（状态机）来控制动画的切换，状态机由State和Transition组成，State代表一个状态（动画片段），Transition表示切换状态的条件，当满足某个条件时就会从当前状态转为另一个状态

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animator/State%20Machine.png)

这里设置了四个状态，分别是idle、walk、run、jump，他们之间的切换条件是：

- idle——walk：state=1

- walk——run：state=2

- run——walk：state=1

- run——jump：trigger条件满足时

- Any State（任何状态）——idle：state=0

Entry进来时的动画指向idle，所以在游戏运行时默认播放idle动画，现在通过代码来修改条件实现动画之间的切换

    public class ChangeStateTest : MonoBehaviour
    {
        private Animator ani;
        private void Awake()
        {
            ani = GetComponent<Animator>();
        }
        // Use this for initialization
        void Start ()
        {
    
        }
    
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ani.SetInteger("State", 1);
            if (Input.GetKeyDown(KeyCode.W))
                ani.SetInteger("State", 2);
            if (Input.GetMouseButtonDown(0))
                ani.SetInteger("State", 0);
            if (Input.GetKeyDown(KeyCode.E))
                ani.SetTrigger("Jump");
        }
    }

在按下Q键时，将State的值设置为1，此时可由idle状态切换为walk状态

在按下W键时，将State的值设置为2，此时可由walk状态切换为run状态

在按下鼠标左键时，将State的值设置为0，此时可由任何状态切换至idle状态

在按下E键时，设置Jump被触发，此时可由run状态切换为Jump状态

但此时会出现一个bug，当切换为Jump状态时动画播放完成后会卡住，只有按下鼠标左键才会自动切换到idle状态，这是由于当State为0时才会切换到idle状态，而在run切换到Jump时，只设置了trigger，并没有设置State，所以在Jump状态时的State为2，而Jump又只通过Any State指向了idle状态，所以会出现动画卡住的情况，所以需要通过代码实现当进入Jump状态时，在Jump的动画快播放完毕时，把State设置为0，这样在播放完Jump动画时就会自动的切换到idle状态

    public class ChangeStateTest : MonoBehaviour
    {
        private Animator ani;
        private void Awake()
        {
            ani = GetComponent<Animator>();
        }
        // Use this for initialization
        void Start ()
        {
    
        }
    
        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
                ani.SetInteger("State", 1);
            if (Input.GetKeyDown(KeyCode.W))
                ani.SetInteger("State", 2);
            if (Input.GetMouseButtonDown(0))
                ani.SetInteger("State", 0);
            if (Input.GetKeyDown(KeyCode.E))
                ani.SetTrigger("Jump");
            AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
            if (state.IsName("Jump"))
            {
                if (state.normalizedTime > 0.95f)
                {
                    ani.SetInteger("State", 0);
                }
            }
    
        }
    }

通过AnimatorStateInfo来获取到层级为0的状态信息，如果状态的名字是Jump，在它播放的时长到达95%时将State设置为0，这样可以在动画结束时直接切换到idle状态

**案例1：默认播放idle动画，按下R键切换到Run动画，抬起R键切换回Idle动画，在按下R键的同时按下J，切换到Jump动画**

首先根据需求设置好各个动画之间的转换

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animator/Snipaste_2023-07-19_11-26-37.png)

    public class ChangeStateTest : MonoBehaviour
    {
        private Animator ani;
        private void Awake()
        {
            ani = GetComponent<Animator>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                ani.SetBool("isRun", true);
            if (Input.GetKeyUp(KeyCode.R))
                ani.SetBool("isRun", false);
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("run") && Input.GetKeyDown(KeyCode.J))
                ani.SetTrigger("Jump");
        }
    }

z这里的条件设置为bool类型，当按下R键时，设置为true切换到Run动画，当抬起R键时，设置为false，切换回indle动画，然后通过判断在播放Run动画的同时是否按下了J键，如果是则播放Jump动画

## 四、实现动画的同时播放

实现动画的同时播放主要通过将状态放在同一个动画控制器中的不同层级实现的（放在一个层级中只能实现动画的切换，同一时间只能播放一个动画），通过点击+号创建一个新的层级

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animator/Layer.png)

 

然后可以在第二个层级中加入一个nod（点头）动画，此时如果运行游戏，会发现在Animator中可以看到两个动画确实是同时播放的，但是在游戏视图却看不到点头的动画，这是因为层级的权重问题，默认的第二个层级的权重值为0，这样导致动画播放的时候主要展示的是第一层级中的动画，因此需要将第二个层级的权重也设置为最大

![](https://github.com/shishouheng/Unity-learning/blob/main/images/Animator/weight.png)

此时运行游戏就可以看到在idle播放的同时会有nod的动画了

### 如何实现按下某个键再同时播放动画？

如果直接将nod动画放入第二个层级中，那进入的时候就会默认播放nod，因此不能将进入时播放的默认动画设置为nod，此时可右键创建一个empty（空动画），使默认播放的是这个空动画，然后通过代码设置按下某个键的时候再播放nod动画即可实现

    public class ChangeStateTest : MonoBehaviour
    {
        private Animator ani;
        private void Awake()
        {
            ani = GetComponent<Animator>();
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
                ani.SetBool("isRun", true);
            if (Input.GetKeyUp(KeyCode.R))
                ani.SetBool("isRun", false);
            if (ani.GetCurrentAnimatorStateInfo(0).IsName("run") && Input.GetKeyDown(KeyCode.J))
                ani.SetTrigger("Jump");
            if (Input.GetKeyDown(KeyCode.Space))
                ani.SetTrigger("nod");
        }
    }

## 五、新旧版动画系统区别

- 旧版动画回随着状态的增多，代码的复杂度会变大，而新版动画以可视化的编辑方式，直接展示出了各个动画之间的切换关系

- 新版动画引入了Avatar，指定了模型的哪些部分对应于腿、手臂、头和身体。并由于不同人形角色之间骨骼结构的相似性，可以将一个人形动画映射到另一个人形角色身上，从而实现重定向和反向动力学

- Animator有一个动画控制器，使用它进行动画切换很方便，但缺点是占用内存比Animation大
