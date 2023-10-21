using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelMain : UIPanel
{
    public void OnClickEnterBattle()
    {
        GameStateManager.Instance.EnterState<GameStateBattle>();
    }

}
