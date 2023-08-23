using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateAttack : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Attack";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 2);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if(state.normalizedTime>0.85f)
        {
            manager.ChangeState<EnemyStateIdle>();
            return;
        }
    }
}
