using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack01 : PlayerStateBase
{
    float height;
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack01;
        attackDistance = 2.5f;
        aniName = "Attack01";
    }
    public override void OnEnter()
    {
        //敌人是否在玩家攻击范围内
        if(!IsCanAttack())
        {
            manager.ChangeState<PlayerStateRun>(PlayerState.Attack01);
            return;
        }
        ani.SetInteger("State", 2);
        height = ani.bodyPosition.y;//记录此状态时角色的y（身体质量中心）
    }
    public override void OnExcute()
    {
        if (!IsCurrentAnimationPlay())
            return;
        if(isCanFreezeFrame)
        {
            freezeCount--;
            if(freezeCount<=0)//顿帧完成
            {
                ani.speed = 1;
                isCanFreezeFrame = false;
            }
        }
        UpdateEnemyY();
        if(ani.GetCurrentAnimatorStateInfo(0).normalizedTime>0.95f)
        {
            manager.ChangeState<PlayerStateIdle>(0);
            return;
        }
    }

    void UpdateEnemyY()
    {
        float currentY = ani.bodyPosition.y;
        target.position = new Vector3(target.position.x,Mathf.Clamp(currentY - height,0,1), target.position.z);
    }
    //动画帧事件（顿帧效果）
    bool isCanFreezeFrame;//是否开始顿帧
    int freezeCount;//顿多少帧
    public void Attack11()
    {
        isCanFreezeFrame = true;
        ani.speed = 0;
        freezeCount = 5;
        cc.ShakeCamera();
        enemyManager.ChangeDamage(AttackType.Right);
    }

    public void Attack12()
    {
        isCanFreezeFrame = true;
        ani.speed = 0;
        freezeCount = 5;
        cc.ShakeCamera();
        enemyManager.ChangeDamage(AttackType.Left);
    }

    public void Attack13()
    {
        isCanFreezeFrame = true;
        ani.speed = 0;
        freezeCount = 3;
        cc.ShakeCamera();
        enemyManager.ChangeDamage(AttackType.Forward);
    }

    public void Attack14()
    {
        isCanFreezeFrame = true;
        ani.speed = 0;
        freezeCount = 3;
        cc.ShakeCamera();
        enemyManager.ChangeState<EnemyStateDead>();
    }
}
