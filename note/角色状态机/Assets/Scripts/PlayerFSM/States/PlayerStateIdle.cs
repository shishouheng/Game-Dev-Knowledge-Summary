using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateIdle : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Idle;
    }
    public override void OnEnter()
    {
        //将Animator里的参数设为0
        ani.SetInteger("State", 0);
    }
    public override void OnExcute()
    {
        //按下鼠标左键切换到Attack1动画
        if(Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack1>();
            return;
        }
        //按下空格切换到Jump动画
        if(Input.GetKeyDown(KeyCode.Space))
        {
            manager.ChangeState<PlayerStateJump>();
            return;
        }
        //发生位移时切换到Run动画
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        if(h!=0||v!=0)
        {
            manager.ChangeState<PlayerStateRun>();
            return;
        }

    }
}
