using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStateJump : PlayerStateBase
{
    //网格信息
    OffMeshLinkData data;
    //角色在改网格运动过程中的起始位置、结束位置和方向
    Vector3 startPos,endPos,dir;
    public override void OnInit()
    {
        base.OnInit();
        aniName = "RunJump";
    }
    public override void OnEnter()
    {
        //初始化数据并播放jump动画
        StartState();
        ani.Play(aniName);
    }
    public override void OnExcute()
    {
        //再次使角色朝向终点位置，防止意外
        transform.rotation = Quaternion.LookRotation(dir);
        float norTime = ani[aniName].normalizedTime;
        //动画播放过程中的操作
        if(norTime<0.95f)
        {
            //平滑更新角色从起点到终点的移动，并实现抛物线形式的竖直方向运动
            Vector3 newPos = Vector3.Lerp(startPos, endPos, norTime);
            newPos.y += Mathf.Sin(Mathf.PI * norTime) * 2.0f;
            transform.position = newPos;
        }
        //动画播放完成
        else
        {
            //允许运动
            agent.isStopped = false;
            //通知导航移动网格连接已经完成
            agent.CompleteOffMeshLink();
            //切换奔跑动画
            manager.ChangeState<PlayerStateRun>();
            return;
        }
    }
    void StartState()
    {
        //禁用agent的运动，防止agent按照默认方式移动
        agent.isStopped = true;
        //获取agent所在网格连接的信息
        data = agent.currentOffMeshLinkData;
        //将角色位置修改为起始位置并让角色朝向结束位置
        startPos = data.startPos;
        endPos = data.endPos;

        transform.position = startPos;
        dir = endPos - startPos;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }
}
