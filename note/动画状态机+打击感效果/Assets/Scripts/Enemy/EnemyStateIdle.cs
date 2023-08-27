using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Idle";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 0);
    }
}
