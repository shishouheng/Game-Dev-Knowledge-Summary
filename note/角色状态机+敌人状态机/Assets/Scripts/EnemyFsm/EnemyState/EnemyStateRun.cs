using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateRun : EnemyStateBase
{
    //是否需要追逐玩家
    private bool isMoveToPlayer;

    //追逐的玩家位置
    private Vector3 target;

    //敌人随机运动的中心点
    private Vector3 centerPos;
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Run";
        centerPos = transform.position;
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 1);
        //如果角色和敌人的距离小于10进入Run状态，
        if (Vector3.Distance(transform.position, playerTrans.position) < 10)
        {
            isMoveToPlayer = true;
            
            target.Set(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        }
        //否则进入巡逻状态
        else
        {
            isMoveToPlayer = false;
            target.Set(centerPos.x+Random.Range(-10,11), transform.position.y, centerPos.z+Random.Range(-10,11));
        }
    }
    public override void OnExcute()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName(aniName))
            return;
        //如果敌人处于追逐状态，玩家角色的位置信息在每一帧都可能发生改变，
        if (isMoveToPlayer)
            target.Set(playerTrans.position.x, transform.position.y, playerTrans.position.z);
        //朝向角色方向并移动
        transform.LookAt(target);
        transform.Translate(Vector3.forward * 0.1f);


        float dis = Vector3.Distance(transform.position, target/*玩家位置信息或随机出来的点位*/);
        //追逐过程中的判断
        if (isMoveToPlayer)
        {
            //距离>10则切换回idle状态
            //如果和角色距离<3先变为idle再转为attack(转为attack的的逻辑在idle中判断）
            if (dis<2.0f||dis>10)
            {
                manager.ChangeState<EnemyStateIdle>();
                return;
            }
        }

        //巡逻过程中的判断
        else
        {
            //和角色距离<3，切换为idle然后转为attack
            if (dis < 2.0f)
            {
                manager.ChangeState<EnemyStateIdle>();
                return;
            }
            //和角色距离<10，转为追逐状态
            else if (Vector3.Distance(transform.position, playerTrans.position) < 10)
                isMoveToPlayer = true;
        }
    }
}
