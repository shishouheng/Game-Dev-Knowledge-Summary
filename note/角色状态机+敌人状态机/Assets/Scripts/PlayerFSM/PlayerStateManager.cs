using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    //存储角色所有状态,键是状态的Type类型，值是PlayerStateBase的子类
    Dictionary<Type, PlayerStateBase> states = new Dictionary<Type, PlayerStateBase>();
    public PlayerStateBase currentPlayerState;
    private void Awake()
    {
        AddState<PlayerStateIdle>();
        AddState<PlayerStateRun>();
        AddState<PlayerStateAttack1>();
        AddState<PlayerStateAttack2>();
        AddState<PlayerStateAttack3>();
        AddState<PlayerStateJump>();
    }

    private void Start()
    {
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
