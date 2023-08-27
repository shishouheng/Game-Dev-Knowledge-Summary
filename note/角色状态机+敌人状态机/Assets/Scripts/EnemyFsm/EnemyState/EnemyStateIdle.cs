using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateIdle : EnemyStateBase
{
    float timer;
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Idle";
    }
    public override void OnEnter()
    {
        timer = 0;
        ani.SetInteger("State", 0);
    }
    public override void OnExcute()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName(aniName))
            return;
        timer += Time.deltaTime;
        //和角色距离小于3的时候进入攻击状态
        float dis = Vector3.Distance(transform.position, playerTrans.position);
        if (dis < 2.0f)
        {
            manager.ChangeState<EnemyStateAttack>();
            return;
        }
        //角色进入敌人10米以内或者敌人保持idle状态3s进入奔跑状态
        else if (dis < 10 || timer >= 3.0f)
        {
            manager.ChangeState<EnemyStateRun>();
            return;
        }
    }
}
