using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerInit : MonoBehaviour
{
    public GameObject towerPrefab;
    public float offsetY = 2.7f;
    private void Update()
    {
        Init();
    }
    private void Init()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            if(Physics.Raycast(ray,out hitInfo,1000,1<<LayerMask.NameToLayer("Tower")))
            {
                if(hitInfo.transform.childCount==0)
                {
                    GameObject tower = Instantiate(towerPrefab);
                    tower.transform.SetParent(hitInfo.transform);
                    tower.transform.localPosition = Vector3.up * offsetY;
                }
            }
        }
    }
}
