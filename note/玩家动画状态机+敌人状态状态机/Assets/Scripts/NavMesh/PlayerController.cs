using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    private Animation ani;
    private NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ani = GetComponent<Animation>();
    }
    private void Start()
    {
        agent.autoTraverseOffMeshLink = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo, 1000, 1 << LayerMask.NameToLayer("Ground")))
            {
                agent.SetDestination(hitInfo.point);
                StartCoroutine(MyUpdateState());
            }
        }
    }
    IEnumerator MyUpdateState()
    {
        ani.Play("Run");
        //等待遇到网格连接
        while (!agent.isOnOffMeshLink)
            yield return null;
        //遇到网格连接
        OffMeshLinkData datas = agent.currentOffMeshLinkData;
        Vector3 startPos = datas.startPos;
        Vector3 endPos = datas.endPos;
        Vector3 dir = endPos - startPos;
        dir.y = 0;
        Quaternion qua = Quaternion.LookRotation(dir);

        //更新位置信息和旋转信息
        Quaternion playerQua = transform.rotation;
        Vector3 playerPos = transform.position;
        float blendTime = 1.0f;
        float blendTimer = 0.0f;
        do
        {
            transform.position = Vector3.Lerp(playerPos, startPos, blendTimer / blendTime);
            transform.rotation = Quaternion.Lerp(playerQua, qua, blendTimer / blendTime);
            blendTimer += Time.deltaTime;
            yield return null;
        } while (blendTimer < blendTime);
        ani.Play("Ladder Up");
        do
        {
            transform.rotation = qua;
            yield return null;
        } while (ani["Ladder Up"].normalizedTime < 0.95f);
        //transform.position = endPos;
        ani.Play("Run");
        //需要通知分离网格结束了
        agent.CompleteOffMeshLink();
        agent.isStopped = false;

        while(agent.hasPath)
        {
            yield return null;
        }
        ani.Play("Idle");
    }
}
