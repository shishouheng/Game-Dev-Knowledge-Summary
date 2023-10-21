using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateInitialize : GameState
{
    public override void Initialize()
    {
        //初始化UI系统
        SystemModuleManager.Instance.AddSystemModule<SystemUIManager>();
        //初始化场景管理系统
        SystemModuleManager.Instance.AddSystemModule<SystemSceneManager>();
        //初始化配置文件管理系统
        SystemModuleManager.Instance.AddSystemModule<ConfigDatabase>();
    }

    public override void OnEnter()
    {
        //初始化完成后就进入登录状态（场景）
        GameStateManager.Instance.EnterState<GameStateLogin>();
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
