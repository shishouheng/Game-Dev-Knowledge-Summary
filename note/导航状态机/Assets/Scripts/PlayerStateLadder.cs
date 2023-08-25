using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStateLadder : PlayerStateBase
{
    //网格信息
    OffMeshLinkData data;
    //网格连接起始位置结束位置和方向
    Vector3 startPos, endPos;
    Vector3 dir;
    public override void OnEnter()
    {
        //初始化信息和播放动画
        StartState();
        ani.Play(aniName);
    }
    void StartState()
    {
        //防止代理使用默认的网格连接方式
        agent.isStopped = true;
        //获取当前网格连接的信息
        data = agent.currentOffMeshLinkData;
        startPos = data.startPos;
        endPos = data.endPos;
        //判断角色是上楼梯还是下楼梯
        float centerY = (startPos.y + endPos.y) / 2;
        if (transform.position.y < centerY)
            aniName = "Ladder Up";
        else
            aniName = "Ladder Down";

        transform.position = startPos;
        dir = endPos - startPos;
        dir.y = 0;
        transform.rotation = Quaternion.LookRotation(dir);
    }
    public override void OnExcute()
    {
        //i点到终点
        transform.rotation = Quaternion.LookRotation(dir);
        //等待爬楼梯动画播放完成===>更新位置、通知unity
        if(ani[aniName].normalizedTime>0.95f)
        {
            transform.position = endPos;
            agent.CompleteOffMeshLink();
            agent.isStopped = false;
            manager.ChangeState<PlayerStateRun>();
            return;
        }
    }
}
