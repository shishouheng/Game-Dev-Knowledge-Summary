using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerStateBase : MonoBehaviour
{
    protected Animation ani;
    protected PlayerStateManager manager;
    protected string aniName;
    protected NavMeshAgent agent;

    public virtual void OnInit()
    {
        ani = GetComponent<Animation>();
        manager = GetComponent<PlayerStateManager>();
        agent = GetComponent<NavMeshAgent>();
    }
    public virtual void OnEnter()
    {

    }
    public virtual void OnExcute()
    {

    }
    public virtual void OnExit()
    {

    }
    //鼠标左键按下状态
    protected bool GetMousePos()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray,out hitInfo))
            {
                if(hitInfo.collider.CompareTag("Obstacle"))
                {
                    agent.SetDestination(hitInfo.point);
                    return true;
                }
            }
        }
        return false;
    }
}
