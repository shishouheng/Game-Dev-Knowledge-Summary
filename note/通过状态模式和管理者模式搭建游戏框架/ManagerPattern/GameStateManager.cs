using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStateManager : SystemModule
{
    public static GameStateManager Instance
    {
        get { return SystemModuleManager.Instance.GetSystemModule<GameStateManager>(); }
    }
    Dictionary<Type, GameState> stateDic = new Dictionary<Type, GameState>();
    List<GameState> states = new List<GameState>();

    //向外部提供的获取当前状态
    private GameState currentState;
    public GameState CurrentState
    {
        get { return currentState; }
    }

    //添加/进入状态
    public T EnterState<T>()where T:GameState
    {
        Type t = typeof(T);
        GameState state;
        //如果字典中没有该状态则添加该状态，否则就退出当前状态进入下一个状态
        if(!stateDic.TryGetValue(t,out state))
        {
            state = gameObject.AddComponent<T>();
            state.Initialize();
            states.Add(state);
            stateDic.Add(t, state);
        }

        if (currentState != null)
            currentState.OnExit();

        currentState = state;
        currentState.OnEnter();

        return state as T;
    }

    public override void OnUpdate()
    {
        //如果当前状态不为空，执行当前状态
        if (currentState != null)
            currentState.OnUpdate();
    }

    public override void Dispose()
    {
        states.Clear();
        stateDic.Clear();
    }
}
