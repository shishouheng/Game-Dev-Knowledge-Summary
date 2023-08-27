using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateForward : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Forward";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 3);
    }
    public override void OnExcute()
    {
        if (!IsCurrentAnimationPlay())
            return;
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            manager.ChangeState<EnemyStateIdle>();
    }
}
