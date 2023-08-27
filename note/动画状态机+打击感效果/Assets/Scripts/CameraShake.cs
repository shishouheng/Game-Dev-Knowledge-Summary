using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    int shakeCount;
    Vector3 startPos;//相机开始位置
    public void ShakeCamera()
    {
        shakeCount = Random.Range(5, 11);//随机抖动次数
        startPos = transform.position;
    }
    private void Update()
    {
        if(shakeCount>0)
        {
            shakeCount--;
            transform.position = startPos + Random.insideUnitSphere*0.05f;
            if (shakeCount == 0)
                transform.position = startPos;
        }
    }
}
