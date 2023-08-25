using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Idle";
    }
    public override void OnEnter()
    {
        ani.Play(aniName);
    }
    public override void OnExcute()
    {
        if(GetMousePos())
        {
            manager.ChangeState<PlayerStateRun>();
            return;
        }
    }
}
