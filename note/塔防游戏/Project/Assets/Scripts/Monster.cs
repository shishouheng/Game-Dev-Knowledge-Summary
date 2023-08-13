using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Monster : MonoBehaviour
{
    private Transform endPos;
    private NavMeshAgent agent;
    private Animation ani;
    public AnimationClip deathClip;
    public static bool isDead = false;
    private float hp;
    public float Hp
    {
        get { return hp; }
        set
        {
            if (value <= 0)
                hp = 0;
            else
                hp = value;
        }
    }
    public Image lifebar;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        endPos = GameObject.FindWithTag("End").transform;
        ani = GetComponent<Animation>();
    }
    private void Start()
    {
        agent.SetDestination(endPos.position);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("End"))
            //Destroy(gameObject);
            PoolManager.Instance.PutInPool(gameObject);
    }
    public void TakeDamage(float damageVal)
    {
        Hp -= damageVal;
        if (Hp == 0)
        {
            isDead = true;
            MonsterDead();
        }
    }
    public Action monsterDeadEvent;
    void MonsterDead()
    {
        agent.isStopped = true;
        ani.Play("Dead");

        //Destroy(gameObject,1.5f);
        PoolManager.Instance.PutInPool(gameObject,1.5f);
        monsterDeadEvent();
    }
}
