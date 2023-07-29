using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterBasePanel : MonoBehaviour
{
    public GameObject[] monsters;
    private GameObject lastMonsters;//当前在木板上展示的怪物
    private void Start()
    {
        lastMonsters = monsters[0];
    }
    public void OnClickIcon(int index)
    {
        //当前下标对应的怪物激活
        //monsters[index].SetActive(true);
        ////其他怪物隐藏
        //for(int i=0;i<monsters.Length;i++)
        //{
        //    if (i != index)
        //        monsters[i].SetActive(false);
        //}


        //当前展示的怪物隐藏
        lastMonsters.SetActive(false);
        //激活当前怪物
        monsters[index].SetActive(true);
        //更新当前怪物
        lastMonsters = monsters[index];
    }
    public void OnClickChangeSkin(int index)
    {
        lastMonsters.GetComponent<Monster>().ChangeSkin(index);
    }
    public void OnClickChangeAni(string aniName)
    {
        lastMonsters.GetComponent<Monster>().ChangeAni(aniName);
    }
    public void MonsterRotate()
    {
        if (Input.GetMouseButton(1))
            lastMonsters.transform.rotation *= Quaternion.Euler(0, -Input.GetAxis("Mouse X"), 0);

    }
    private void Update()
    {
        MonsterRotate();
    }
    public void OnClickMonsterWeapon(int index)
    {
        lastMonsters.GetComponent<Monster>().ChangeWeapon(index);
    }
}
