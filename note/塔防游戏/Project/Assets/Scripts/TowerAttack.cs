using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerAttack : MonoBehaviour
{
    public Queue<Transform> attackQueue;
    private Transform turret;
    private GameObject attackParticle;
    public float rotateSpeed = 10;
    public float damageValue = 10;
    private Transform preMonster;
    private void Awake()
    {
        attackQueue = new Queue<Transform>();
        turret = transform.Find("Base/Turret");
        attackParticle = transform.Find("Base/Turret/Barrel/Muzzle_1").gameObject;
    }
    private void Update()
    {
        if (attackQueue.Count <= 0)
        {
            attackParticle.SetActive(false);
            return;
        }
        attackParticle.SetActive(true);
        Transform targetMonster = attackQueue.Peek();
        if(targetMonster==null)
        {
            attackQueue.Dequeue();
            return;
        }
        Vector3 dir = targetMonster.position - turret.position;
        Quaternion qua = Quaternion.LookRotation(dir);
        turret.rotation = Quaternion.Lerp(turret.rotation, qua, Time.deltaTime * rotateSpeed);
        Monster monster = targetMonster.GetComponent<Monster>();
        monster.TakeDamage(damageValue);
        if (preMonster != targetMonster)
            monster.monsterDeadEvent = DeQueueMonster;
        preMonster = targetMonster;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            attackQueue.Enqueue(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            DeQueueMonster();
        }
    }
    public void RemoveMonster(Transform monster)
    {
        attackQueue.Dequeue();
    }
    void DeQueueMonster()
    {
        if (attackQueue.Count > 0)
            attackQueue.Dequeue();
    }
}
