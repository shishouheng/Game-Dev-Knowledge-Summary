using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateBattle : GameState
{
    public override void Initialize()
    {
        base.Initialize();
    }

    public override void OnEnter()
    {
        //切换战斗场景
        SystemSceneManager.Instance.ChangeSceneByAsync("GameBattle", OnSceneLoadSuccess);
    }

    public void OnSceneLoadSuccess(string name)
    {
        //显示战斗UI
        SystemUIManager.Instance.ShowUIModule(UIType.UIPanelBattle);
    }
}
