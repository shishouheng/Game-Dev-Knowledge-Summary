using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack2 : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack2;
        aniName = "attack2";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State",3);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if (Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack3>();
            return;
        }
        if (state.normalizedTime > 0.9f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
    }
}
