using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform player;
    private Vector3 offest;
    private Vector3 newPos;
    private float moveSpeed = 10;
    private float rotateSpeed = 5;
    private void Awake()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Start()
    {
        offest = transform.position - player.position;
    }
    private void Update()
    {
        Vector3 standardPos = offest + player.position;
        Vector3 abovePos = player.position + Vector3.up * offest.magnitude;
        Vector3[] checkPoints = new Vector3[6];
        checkPoints[0] = standardPos;
        checkPoints[1] = Vector3.Lerp(standardPos, abovePos, 0.2f);
        checkPoints[2] = Vector3.Lerp(standardPos, abovePos, 0.4f);
        checkPoints[3] = Vector3.Lerp(standardPos, abovePos, 0.6f);
        checkPoints[4] = Vector3.Lerp(standardPos, abovePos, 0.8f);
        checkPoints[5] = abovePos;
        for (int i = 0; i < checkPoints.Length; i++)
        {
            if (CheckPoint(checkPoints[i]))
                break;
        }
        transform.position = Vector3.Lerp(transform.position, newPos, Time.deltaTime * moveSpeed);
        LookAtSmooth();
    }
    void LookAtSmooth()
    {
        Vector3 dir = player.position - transform.position;
        Quaternion qua = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * rotateSpeed);
    }
    bool CheckPoint(Vector3 point)
    {
        RaycastHit hitInfo;
        if(Physics.Raycast(point,player.position-point,out hitInfo))
        {
            if (hitInfo.transform != player)
                return false;
            newPos = point;
        }
        return true;
    }
}
