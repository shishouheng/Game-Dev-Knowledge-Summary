using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private CharacterController cc;
    private void Awake()
    {
        cc = GetComponent<CharacterController>();
    }
    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        Vector3 dir = new Vector3(h, 0, v);
        Debug.Log("世界坐标系："+dir.ToString());
        dir = Camera.main.transform.TransformDirection(dir);
        Debug.Log("相机坐标系:"+dir.ToString());
        transform.rotation = Quaternion.LookRotation(dir);

        cc.SimpleMove(dir * 3f);
    }
}
