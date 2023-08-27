using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateManager : MonoBehaviour
{
    Dictionary<Type, EnemyStateBase> states = new Dictionary<Type, EnemyStateBase>();
    EnemyStateBase currentEnemyState;
    private void Awake()
    {
        AddState<EnemyStateIdle>();
        AddState<EnemyStateAttack>();
        AddState<EnemyStateRun>();
        AddState<EnemyStateDamage>();
        AddState<EnemyStateDeath>();
    }

    private void Start()
    {
        ChangeState<EnemyStateIdle>();
    }
    private void Update()
    {
        if (currentEnemyState != null)
            currentEnemyState.OnExcute();
    }

    //将对应的状态添加到容器中，并通过泛型限制只能添加继承了PlayerStateBase的类
    void AddState<T>() where T : EnemyStateBase
    {

        //添加一个状态就将该状态的脚本挂载到对象身上
        EnemyStateBase t = gameObject.AddComponent<T>();
        //调用该状态的初始化方法
        t.OnInit();
        //加入容器中
        states.Add(typeof(T), t);
    }

    //改变状态的方法
    public void ChangeState<T>() where T : EnemyStateBase
    {
        if(states.ContainsKey(typeof(T)))
        {
            //将状态切换为下一个状态并执行进入该状态的方法
            currentEnemyState = states[typeof(T)];
            currentEnemyState.OnEnter();
        }
    }
}
