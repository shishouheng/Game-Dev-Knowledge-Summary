在Unity中，状态机是一种用于管理游戏对象状态和行为的模式，它基于状态的概念，将游戏对象的行为划分为不同的状态，并根据不同的条件来切换这些状态。

状态机由状态、转换条件和行为组成，每个状态代表了游戏对象的一种行为或状态，转换条件定义了从一个状态切换到另一个状态的条件，行为则是在特定状态下执行的动作或逻辑。

通过状态机的使用可以更好的组织和管理游戏对象的行为，使代码易于理解和维护。


如在Unity中一个玩家具有待机、奔跑、攻击、受击、死亡等动画，在Animator 中设置好可切换的状态以及切换条件后就可以通过代码来实现在特定的情况下进行状态切换

对于一个状态机，最简单的方式就是通过Switch case语句来进行状态切换，如：
```c#
enum PlayerState{Idle,Run,Attack......}
PlayerState currentState; 
swich（currentState） 
{ 
     case PlayerState.Idle: 
        IdleFunc(); 
        .....
     break; 
     case PlayerState.Run: 
        RunFunc() 
        ....
     break; 
     case PlayerState.Attack: 
        AttackFunc(); 
        ....
     break;
     .....
} 
      void IdleFunc() { } 
      void RunFunc() { } 
      void AttackFunc() { }
```
这种写法确实很简单也易于理解，但存在着很大的问题，随着后续角色状态的增加，这个switch语句会变得过于臃肿，而且耦合性过强直观性也会随着状态的增加逐渐变弱，对于后期的维护和扩展都变得较为困难




因此可以使用另一种思路来进行状态的切换：
首先假设一个角色具有待机、奔跑、跳跃、攻击1、攻击2、攻击3状态，在每一个状态时都需要进行更新（更新动画）和改变（改变逻辑）以及监听（监听是否需要进入下一个状态）。

例如在角色移动的过程中，需要在每一帧更新动画以及改变角色的位置信息并时刻监听是否需要切换到下一个状态。

在每一个状态中都包含
- 初始化状态
- 进入状态
- 执行状态
- 结束状态

对于玩家的每一个状态都需要具有这四个步骤，因此可以定义一个状态的父类PlayerStateBase，具有OnInit，OnEnter、OnExcute、OnExit这四个虚方法（因为不同的状态需要执行的逻辑可能不同），对于每个状态都有一个单独的类来表示，并且都需要继承该父类并重写这些方法，同时需要定义一个枚举PlayerState来代表不同的状态。最后还需要一个管理者的脚本来管理和存储所有状态，并根据不同条件来切换状态以及执行相关状态的代码

案例：角色具有idle、run、attack1、attack2、attack3和jump六个状态，并且只有在attack1的过程中才能切换到attack2，在attack2的过程中才能切换到attack3

首先定义枚举类PlayerState
```c#
enum PlayerState{Idle,Run,Attack1,Attack2,Attack3,Jump}
```


接着定义所有状态的父类PlayerStateBase,
```c#
public class PlayerStateBase : MonoBehaviour
{
    //存储当前状态
    public PlayerState currentState;
    public Animator ani;
    public PlayerStateManager manager;
    public string aniName;
    //初始化
    public virtual void OnInit()
    {
        //获取Animator组件和PlayerStateBase脚本
        ani = GetComponent<Animator>();
        manager = GetComponent<PlayerStateManager>();
    }
    //进入
    public virtual void OnEnter()
    {

    }
    //更新
    public virtual void OnExcute()
    {
        
    }
    //退出
    public virtual void OnExit()
    {
        
    }
}
```

然后给每个状态定义一个类，并且都继承PlayerStateBase。

定义PlayerStateManager状态管理类
```c#
public class PlayerStateManager : MonoBehaviour
{
    //存储角色所有状态,键是状态的Type类型，值是PlayerStateBase的子类
    Dictionary<Type, PlayerStateBase> states = new Dictionary<Type, PlayerStateBase>();
    public PlayerStateBase currentPlayerState;
    private void Awake()
    {
        //游戏运行时将所有状态的脚本挂载并注册到容器中
        AddState<PlayerStateIdle>();
        AddState<PlayerStateRun>();
        AddState<PlayerStateAttack1>();
        AddState<PlayerStateAttack2>();
        AddState<PlayerStateAttack3>();
        AddState<PlayerStateJump>();
    }

    private void Start()
    {
        //设置初始状态为Idle状态
        ChangeState<PlayerStateIdle>();
    }
    private void Update()
    {
        if (currentPlayerState != null)
            currentPlayerState.OnExcute();
    }

    //将对应的状态添加到容器中，并通过泛型限制只能添加继承了PlayerStateBase的类
    void AddState<T>() where T : PlayerStateBase
    {
        //添加一个状态就将该状态的脚本挂载到对象身上
        PlayerStateBase t = gameObject.AddComponent<T>();
        //调用该状态的初始化方法
        t.OnInit();
        //加入容器中
        states.Add(typeof(T), t);
    }

    //改变状态的方法
    public void ChangeState<T>() where T : PlayerStateBase
    {
        //如果当前状态不为空，则执行状态的退出方法
        if (currentPlayerState != null)
            currentPlayerState.OnExit();
        //将状态切换为下一个状态并执行进入该状态的方法
        currentPlayerState = states[typeof(T)];
        currentPlayerState.OnEnter();
    }
   
}
```
设置每个状态的脚本
Idle状态：
```c#
public class PlayerStateIdle : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Idle;
    }
    public override void OnEnter()
    {
        //将Animator里的参数设为0
        ani.SetInteger("State", 0);
    }
    public override void OnExcute()
    {
        //按下鼠标左键切换到Attack1动画
        if(Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack1>();
            return;
        }
        //按下空格切换到Jump动画
        if(Input.GetKeyDown(KeyCode.Space))
        {
            manager.ChangeState<PlayerStateJump>();
            return;
        }
        //发生位移时切换到Run动画
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if(h!=0||v!=0)
        {
            manager.ChangeState<PlayerStateRun>();
            return;
        }

    }
}
```

Run状态：
```c#
public class PlayerStateRun : PlayerStateBase
{
    private CharacterController cc;
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Run;
        //获取角色控制器组件
        cc = GetComponent<CharacterController>();
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 1);
    }
    public override void OnExcute()
    {
        //按下鼠标左键切换到Attack1动画
        if (Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack1>();
            return;
        }
        //通过虚拟轴控制角色移动
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        //如果没有检测到输入切换回Idle状态
        if(dir==Vector3.zero)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
        //将该向量从世界坐标转换为相机坐标
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        //将角色的面朝方向与移动方向保持一致
        transform.rotation = Quaternion.LookRotation(dir);
        cc.SimpleMove(dir*3.0f);
    }
}
```

Attack1状态：
```c#
public class PlayerStateAttack1 : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack1;
        aniName = "attack1";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State",2);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if (Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack2>();
            return;
        }
        if(state.normalizedTime>0.9f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
    }
}

```

Attack2状态：
```c#
public class PlayerStateAttack2 : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack2;
        aniName = "attack2";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State",3);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if (Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack3>();
            return;
        }
        if (state.normalizedTime > 0.9f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
    }
}
```

Attack3状态：
```c#
public class PlayerStateAttack3 : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack3;
        aniName = "attack3";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State",4);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if (state.normalizedTime > 0.8f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
    }
}
```

Jump状态：
```c#
public class PlayerStateJump : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Jump;
        aniName = "jump";
    }
    //跳跃的开始位置和目标位置
    Vector3 startPos, endPos;
    bool isJump;
    public override void OnEnter()
    {
        ani.SetInteger("State", 5);
        startPos = transform.position;
        isJump = true;
        //设置跳跃的距离是角色前方三个单位
        endPos = transform.position + Vector3.forward*3;
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if(state.normalizedTime>0.9f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
        //跳跃动作执行30%的时候开始产生位移
        else if(state.normalizedTime>0.3f&&isJump)
        {
            //如果不减0.3f那么在跳跃开始的时候会瞬移30%的距离
            transform.position = Vector3.Lerp(startPos, endPos, state.normalizedTime-0.3f);
            //跳跃动作执行60%的时候停止位移
            if (state.normalizedTime > 0.6f)
                isJump = false;
        }
    }
}
```
[完整工程]((https://github.com/shishouheng/Unity-learning/tree/main/note/%E6%9C%89%E9%99%90%E7%8A%B6%E6%80%81%E6%9C%BA)