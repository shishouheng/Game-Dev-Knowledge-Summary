using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateAttack02 : PlayerStateBase
{
    float height;
    Vector3 pos;

    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Attack02;
        attackDistance = 3.0f;
        aniName = "Attack02";
    }
    public override void OnEnter()
    {
        //敌人是否在玩家攻击范围内
        if (!IsCanAttack())
        {
            manager.ChangeState<PlayerStateRun>(PlayerState.Attack02);
            return;
        }
        height = ani.bodyPosition.y;//记录玩家初始的y值
        pos = transform.position;//记录玩家初始位置
        ani.SetInteger("State", 3);
    }
    public override void OnExcute()
    {
        if (!IsCurrentAnimationPlay())
            return;
        if (isCanFreezeFrame)
        {
            freezeCount--;
            if (freezeCount <= 0)//顿帧完成
            {
                ani.speed = 1;
                isCanFreezeFrame = false;
            }
        }
        UpdateEnemyPos();
        if (ani.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.95f)
        {
            manager.ChangeState<PlayerStateIdle>(0);
            return;
        }
    }
    void UpdateEnemyPos()
    {
        float currentY = ani.bodyPosition.y;
        Vector3 currentPos = transform.position;

        //现在玩家位置与初始玩家位置的偏移量
        Vector3 offset = currentPos - pos;
        target.position = new Vector3(0,Mathf.Clamp(currentY-height,0,2),(target.position+offset).z);
        pos = currentPos;

    }
    //动画帧时间（顿帧效果）
    bool isCanFreezeFrame;//是否开始顿帧
    int freezeCount;//顿多少帧
    public void Attack21()
    {
        isCanFreezeFrame = true;
        ani.speed = 0;
        freezeCount = 5;
        cc.ShakeCamera();
        enemyManager.ChangeDamage(AttackType.Right);
    }

    public void Attack22()
    {
        isCanFreezeFrame = true;
        ani.speed = 0;
        freezeCount = 5;
        cc.ShakeCamera();
        enemyManager.ChangeDamage(AttackType.Left);
    }

    public void Attack23()
    {
        isCanFreezeFrame = true;
        ani.speed = 0;
        freezeCount = 8;
        cc.ShakeCamera();
        enemyManager.ChangeState<EnemyStateDead>();
    }
}
