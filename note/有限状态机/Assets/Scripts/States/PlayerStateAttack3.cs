using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack3 : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack3;
        aniName = "attack3";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State",4);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if (state.normalizedTime > 0.8f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
    }
}
