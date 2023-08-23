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
    public void AttackCallBack2()
    {
        //获取玩家周围的敌人
        Collider[] colliders = Physics.OverlapSphere(transform.position, 3.5f, 1 << LayerMask.NameToLayer("Enemy"));
        foreach (var item in colliders)
        {
            //角色前方的向量
            Vector3 forward = transform.forward;
            //玩家到敌人的向量
            Vector3 dir = item.transform.position - transform.position;
            //设置角色攻击范围为前方160°扇形区域
            if (Vector3.Angle(forward, dir) < 80)
            {
                item.GetComponent<EnemyStateManager>().ChangeState<EnemyStateDamage>();
            }

        }
    }
}
