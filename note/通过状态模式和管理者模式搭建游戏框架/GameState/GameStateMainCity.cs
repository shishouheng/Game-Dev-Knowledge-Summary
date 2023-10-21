using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateMainCity : GameState
{
    public override void OnEnter()
    {
        SystemSceneManager.Instance.ChangeSceneByAsync("GameMainCity", OnSceneLoad);
    }

    public void OnSceneLoad(string sceneName)
    {
        //播放主城背景音乐
        //显示主界面UI
        SystemUIManager.Instance.ShowUIModule(UIType.UIPanelMain);
    }
}
