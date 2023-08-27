using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyStateManager : MonoBehaviour
{
    //存储角色所有状态,键是状态的Type类型，值是PlayerStateBase的子类
    Dictionary<Type, EnemyStateBase> states = new Dictionary<Type, EnemyStateBase>();
    EnemyStateBase currentPlayerState;
    private void Awake()
    {
        AddState<EnemyStateIdle>();
        AddState<EnemyStateLeft>();
        AddState<EnemyStateRight>();
        AddState<EnemyStateDead>();
        AddState<EnemyStateForward>();
    }

    private void Start()
    {
        ChangeState<EnemyStateIdle>();
    }
    private void Update()
    {
        if (currentPlayerState != null)
            currentPlayerState.OnExcute();
    }

    //将对应的状态添加到容器中，并通过泛型限制只能添加继承了PlayerStateBase的类
    void AddState<T>() where T : EnemyStateBase
    {
        //添加一个状态就将该状态的脚本挂载到对象身上
        EnemyStateBase t = gameObject.AddComponent<T>();
        //调用该状态的初始化方法
        t.OnInit();
        if (!states.ContainsKey(t.GetType()))
            //加入容器中
            states.Add(typeof(T), t);
    }

    //改变状态的方法
    public void ChangeState<T>() where T : EnemyStateBase
    {
        //将状态切换为下一个状态并执行进入该状态的方法
        currentPlayerState = states[typeof(T)];
        currentPlayerState.OnEnter();
    }
    //切换到指定受击状态
    public void ChangeDamage(AttackType type)
    {
        switch(type)
        {
            case AttackType.Forward:
                ChangeState<EnemyStateForward>();
                break;
            case AttackType.Left:
                ChangeState<EnemyStateLeft>();
                break;
            case AttackType.Right:
                ChangeState<EnemyStateRight>();
                break;
        }
    }
}
public enum AttackType { Forward,Back,Left,Right,Up,Down}