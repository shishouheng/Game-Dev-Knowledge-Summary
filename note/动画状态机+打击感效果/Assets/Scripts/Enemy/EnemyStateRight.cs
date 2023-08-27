using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateRight : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Right";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 2);
    }
    public override void OnExcute()
    {
        if (!IsCurrentAnimationPlay())
            return;
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
            manager.ChangeState<EnemyStateIdle>();
    }
}
