using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDamage : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Damage";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 3);
        ani.SetTrigger("Damage");
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if (state.normalizedTime > 0.85f)
        {
            manager.ChangeState<EnemyStateIdle>();
            return;
        }
    }
}
