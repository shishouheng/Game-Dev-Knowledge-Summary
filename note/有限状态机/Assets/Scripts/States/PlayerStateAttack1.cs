using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack1 : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack1;
        aniName = "attack1";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State",2);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;

        if (Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack2>();
            return;
        }
        if(state.normalizedTime>0.9f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
    }
}
