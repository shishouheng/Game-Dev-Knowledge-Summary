using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRun : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Run;
        aniName = "Run";
    }
    public override void OnEnter()
    {
        agent.SetDestination(target.position);
        agent.isStopped = false;
        ani.SetInteger("State", 1);
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
        //没有下一个状态===》跑到终点并转换为idle
        if (nextState == PlayerState.None)
        {
            if (!agent.hasPath)
            {
                agent.isStopped = true;
                manager.ChangeState<PlayerStateIdle>();
            }
        }

        //有下一个状态=====》和目标点的距离是否小于攻击状态的攻击距离
        else
        {
            //剩余距离小于攻击距离
            if(agent.remainingDistance<manager.GetPlayerStateBaseByPlayerState(nextState).attackDistance)
            {
                //转换到idle并将nextstate传递
                agent.isStopped = true;
                manager.ChangeState<PlayerStateIdle>(nextState);
            }
        }
    }
}
