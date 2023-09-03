using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase : MonoBehaviour
{
    protected Animator ani;
    protected EnemyStateManager manager;
    protected Transform playerTrans;
    protected string aniName;
    protected AnimatorStateInfo state;
    //初始化
    public virtual void OnInit()
    {
        //获取Animator组件和PlayerStateBase脚本
        ani = GetComponent<Animator>();
        manager = GetComponent<EnemyStateManager>();
        playerTrans = GameObject.FindWithTag("Player").transform;
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
    public bool Check()
    {
        if (!state.IsName(aniName))
            return false;
        return true;
    }
}
