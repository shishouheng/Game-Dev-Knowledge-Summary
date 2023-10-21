using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelLogin : UIPanel
{
    public override bool OnShow(params object[] userdatas)
    {
        return base.OnShow(userdatas);
    }

    public override void OnHide(bool hide)
    {
        base.OnHide(hide);
    }

    public void OnClickLogin()
    {
        GameStateManager.Instance.EnterState<GameStateMainCity>();
    }
}
