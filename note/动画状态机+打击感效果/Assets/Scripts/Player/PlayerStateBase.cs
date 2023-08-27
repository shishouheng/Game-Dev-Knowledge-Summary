using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public enum PlayerState { None, Idle, Run, Attack01, Attack02 }


public class PlayerStateBase : MonoBehaviour
{
    protected EnemyStateManager enemyManager;//通过角色状态切换敌人状态
    public  PlayerState currentState;//玩家当前状态
    protected PlayerState nextState;//当前状态的下一个状态
    protected Animator ani;
    protected PlayerStateManager manager;
    protected string aniName;
    protected NavMeshAgent agent;
    protected CameraShake cc;
    protected Transform target;

    public  float attackDistance;//不同攻击状态的攻击距离
    public virtual void OnInit()
    {
        ani = GetComponent<Animator>();
        manager = GetComponent<PlayerStateManager>();
        agent = GetComponent<NavMeshAgent>();
        target = GameObject.FindWithTag("Target").transform;
        cc = Camera.main.GetComponent<CameraShake>();
        enemyManager = target.GetComponent<EnemyStateManager>();
    }
    public virtual void OnEnter()
    {

    }
    public virtual void OnEnter(PlayerState nextState)
    {

    }
    public virtual void OnExcute()
    {

    }
    public virtual void OnExit()
    {

    }
    //敌人是否在玩家攻击范围
    protected bool IsCanAttack()
    {
        return Vector3.Distance(transform.position, target.position) < attackDistance;
    }
    //动画是否切换回来
    protected bool IsCurrentAnimationPlay()
    {
        if (!ani.GetCurrentAnimatorStateInfo(0).IsName(aniName))
            return false;
        return true;
    }
}
