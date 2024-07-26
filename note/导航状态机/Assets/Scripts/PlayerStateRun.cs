using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStateRun : PlayerStateBase
{
    public override void OnInit()
    {
        base.OnInit();
        aniName = "Run";
    }
    public override void OnEnter()
    {
        ani.Play(aniName);
    }
    public override void OnExcute()
    {
        //跑的时候遇到了网格连接===》判断是哪一种连接，Jump（跳下来）？自定义的Ladder（爬楼梯）？
        if (agent.isOnOffMeshLink)
        {
            //获取agent遇到的网格连接类型，并通过类型判断播放哪种动画
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            //Drop
            if (data.linkType == OffMeshLinkType.LinkTypeDropDown)
            {
                manager.ChangeState<PlayerStateJump>();
            }
            //自定义
            else if(data.linkType==OffMeshLinkType.LinkTypeManual)
            {
                manager.ChangeState<PlayerStateLadder>();
            }
            return;
        }
        //跑到目标点
        if (Vector3.Distance(transform.position,agent.destination)<=0.05f)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
        //正在跑，玩家又按下鼠标左键====>重新设置目标点
        GetMousePos();
    }
}
