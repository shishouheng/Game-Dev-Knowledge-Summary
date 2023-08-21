using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateJump : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Jump;
        aniName = "jump";
    }
    //跳跃的开始位置和目标位置
    Vector3 startPos, endPos;
    bool isJump;
    public override void OnEnter()
    {
        ani.SetInteger("State", 5);
        startPos = transform.position;
        isJump = true;
        //设置跳跃的距离是角色前方三个单位
        endPos = transform.position + Vector3.forward*3;
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if(state.normalizedTime>0.9f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
        //跳跃动作执行30%的时候开始产生位移
        else if(state.normalizedTime>0.3f&&isJump)
        {
            //如果不减0.3f那么在跳跃开始的时候会瞬移30%的距离
            transform.position = Vector3.Lerp(startPos, endPos, state.normalizedTime-0.3f);
            //跳跃动作执行60%的时候停止位移
            if (state.normalizedTime > 0.6f)
                isJump = false;
        }
    }
}
