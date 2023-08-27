using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDead : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Dead";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 4);
    }
    public override void OnExcute()
    {
        if (!IsCurrentAnimationPlay())
            return;
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            manager.ChangeState<EnemyStateIdle>();
    }
}
