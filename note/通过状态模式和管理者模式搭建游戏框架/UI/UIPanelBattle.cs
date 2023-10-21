using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanelBattle : UIPanel
{
    public void OnClickReturn()
    {
        GameStateManager.Instance.EnterState<GameStateMainCity>();
    }

}
