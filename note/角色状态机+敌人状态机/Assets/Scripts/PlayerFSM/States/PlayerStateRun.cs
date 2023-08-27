using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateRun : PlayerStateBase
{
    private CharacterController cc;
    public override void OnInit()
    {
        base.OnInit();
        currentState = PlayerState.Run;
        //获取角色控制器组件
        cc = GetComponent<CharacterController>();
    }
    public override void OnEnter()
    {
        ani.SetInteger("State", 1);
    }
    public override void OnExcute()
    {
        //按下鼠标左键切换到Attack1动画
        if (Input.GetMouseButtonDown(0))
        {
            manager.ChangeState<PlayerStateAttack1>();
            return;
        }
        //通过虚拟轴控制角色移动
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        //如果没有检测到输入切换回Idle状态
        if(dir==Vector3.zero)
        {
            manager.ChangeState<PlayerStateIdle>();
            return;
        }
        //将该向量从世界坐标转换为相机坐标
        dir = Camera.main.transform.TransformDirection(dir);
        dir.y = 0;
        //将角色的面朝方向与移动方向保持一致
        transform.rotation = Quaternion.LookRotation(dir);
        cc.SimpleMove(dir*3.0f);
    }
}
