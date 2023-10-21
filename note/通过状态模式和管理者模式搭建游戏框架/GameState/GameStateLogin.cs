using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateLogin : GameState
{
    public override void Initialize()
    {

    }

    public override void OnEnter()
    {
        SystemSceneManager.Instance.ChangeSceneByAsync("GameLogin", OnLoadSuccess);
    }

    public void OnLoadSuccess(string sceneName)
    {
        SystemUIManager.Instance.ShowUIModule(UIType.UIPanelLogin);
    }

    public override void OnUpdate()
    {

    }

    public override void OnExit()
    {

    }
}
