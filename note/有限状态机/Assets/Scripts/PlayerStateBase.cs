using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Idle,Run,Attack1,Attack2,Attack3,Jump
}
public class PlayerStateBase : MonoBehaviour
{
    //存储当前状态
    public PlayerState currentState;
    public Animator ani;
    public PlayerStateManager manager;
    public string aniName;
    public AnimatorStateInfo state;
    //初始化
    public virtual void OnInit()
    {
        //获取Animator组件和PlayerStateBase脚本
        ani = GetComponent<Animator>();
        manager = GetComponent<PlayerStateManager>();
        state = ani.GetCurrentAnimatorStateInfo(0);
    }
    //进入
    public virtual void OnEnter()
    {

    }
    //更新
    public virtual void OnExcute()
    {
        

    }
    public virtual bool check()
    {
        if (!state.IsName(aniName))
            return false;
        return true;
    }
    //退出
    public virtual void OnExit()
    {
        
    }
}
