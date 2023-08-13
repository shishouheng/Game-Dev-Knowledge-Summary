using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class InitMonster : MonoBehaviour
{
    WaveInfo[] waveInfos;
    public int waveCount = 10;
    private float waveTimer;//波数计时器
    private float monsterTimer;//怪物计时器
    private int waveIndex;//波数计数
    private int monsterCount;//当前波怪物计数
    public GameObject monsterPrefab;
    private bool isCanInit = true;//是否可以继续生成
    public Transform endpos;
    private void Awake()
    {
        waveInfos = new WaveInfo[waveCount];
        for (int i = 0; i < waveCount; i++)
        {
            waveInfos[i] = new WaveInfo();
        }
    }
    private void Start()
    {
        OnInitWaveInfos();
    }
    private void Update()
    {
        MonsterInit();
    }
    void MonsterInit()
    {
        if (!isCanInit)
            return;
        waveTimer += Time.deltaTime;
        if (waveTimer >= waveInfos[waveIndex].waveInterval)
        {
            if (monsterCount < waveInfos[waveIndex].monsterCount)
            {
                monsterTimer += Time.deltaTime;
                if (monsterTimer >= waveInfos[waveIndex].monsterInterval)
                {
                    GameObject monster = PoolManager.Instance.GetOutObj("mon_orcWarrior", transform);
                    NavMeshAgent agent = monster.GetComponent<NavMeshAgent>();
                    agent.Warp(transform.position);
                    agent.speed = waveInfos[waveIndex].monsterSpeed;
                    agent.SetDestination(new Vector3(34.07f, 0.472f, 7.26f));
                    monster.GetComponent<Monster>().Hp = waveInfos[waveIndex].monsterHp;
                    
                    monsterCount++;
                    monsterTimer = 0;
                }
            }
            else
            {
                waveIndex++;
                monsterCount = 0;
                waveTimer = 0;
                if (waveIndex >= waveCount)
                    isCanInit = false;
            }
        }
    }
    private void OnInitWaveInfos()
    {
        waveInfos[0].monsterCount = 1;
        waveInfos[0].monsterHp = 100f;
        waveInfos[0].monsterInterval = 2.0f;
        waveInfos[0].monsterSpeed = 5.0f;
        waveInfos[0].waveInterval = 3.0f;
        for (int i = 1; i < waveCount; i++)
        {
            waveInfos[i].monsterCount = waveInfos[0].monsterCount + i;
            waveInfos[i].monsterHp = waveInfos[0].monsterHp + 10 * i;
            waveInfos[i].monsterInterval = waveInfos[0].monsterInterval - i * 0.1f;
            waveInfos[i].monsterSpeed = waveInfos[0].monsterSpeed + i;
            waveInfos[i].waveInterval = waveInfos[0].waveInterval - i * 0.1f;
        }
    }
}

