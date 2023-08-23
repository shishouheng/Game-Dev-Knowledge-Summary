using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateDeath : EnemyStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Death";
    }
    //实现敌人抛物线形式的被击飞的起点和终点
    Vector3 startPos, endPos;
    public override void OnEnter()
    {
        //让敌人始终面向角色
        transform.LookAt(playerTrans);
        //起点是敌人当前位置
        startPos = transform.position;
        //终点是敌人身后三个单位的距离
        endPos = transform.position - transform.forward*3f;
        ani.SetInteger("State", 4);
    }
    public override void OnExcute()
    {
        AnimatorStateInfo state = ani.GetCurrentAnimatorStateInfo(0);
        if (!state.IsName(aniName))
            return;
        if (state.normalizedTime < 1.0f)
        {
            Vector3 pos = Vector3.Lerp(startPos, endPos, state.normalizedTime);
            //被击飞除了水平方向的变化还需要有y轴上的变化
            //Π乘一个在0-1范围内的数字，得到的就是一个抛物线
            pos.y += Mathf.Sin(Mathf.PI * state.normalizedTime) * 3;
            transform.position = pos;
        }
        else
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
