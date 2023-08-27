using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateLeft : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Left";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 1);
    }
    public override void OnExcute()
    {
        if (!IsCurrentAnimationPlay())
            return;
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            manager.ChangeState<EnemyStateIdle>();
    }
}
