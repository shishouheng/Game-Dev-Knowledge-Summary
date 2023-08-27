using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Idle;
        aniName = "Idle";
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 0);
    }
    public override void OnEnter(PlayerState nextState)
    {
        this.nextState = nextState;
        this.OnEnter();
    }
    public override void OnExcute()
    {
        if (!IsCurrentAnimationPlay())
            return;
        if (nextState == PlayerState.Attack01)
            manager.ChangeState<PlayerStateAttack01>();
        else if (nextState == PlayerState.Attack02)
            manager.ChangeState<PlayerStateAttack02>();

    }
    public override void OnExit()
    {
        nextState = PlayerState.None;
    }
}
