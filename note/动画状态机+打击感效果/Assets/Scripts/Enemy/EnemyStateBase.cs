using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateBase : MonoBehaviour
{
    protected Animator ani;
    protected EnemyStateManager manager;
    protected string aniName;

    public float attackDistance;//不同攻击状态的攻击距离
    public virtual void OnInit()
    {
        ani = GetComponent<Animator>();
        manager = GetComponent<EnemyStateManager>();
    }
    public virtual void OnEnter()
    {

    }
    public virtual void OnExcute()
    {

    }
    //动画是否切换回来
    protected bool IsCurrentAnimationPlay()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName(aniName))
            return false;
        return true;
    }
}
