using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubPool
{
    //创建一个容器，这个容器既存储池子内未激活的物体，也包含已激活的物体
    List<GameObject> objects = new List<GameObject>();
    //当前池中存放的道具的预制体
    GameObject prefab;
    //每个池子都具有name属性，方便后续通过name调用相应池子
    public string name { get { return prefab.name; } }
    //父物体
    Transform parent;
    //构造函数，在创建对象时就需要传入父物体和需要加载的物体
    public SubPool(Transform parent, GameObject go)
    {
        prefab = go;
        this.parent = parent;
    }
    //从池中取出物体的方法
    public GameObject GetOutObject()
    {
        //定义一个result用来充当返回出来的物体，默认为null
        GameObject result = null;
        //遍历容器，如果存在未激活的物体就将该物体赋给result并返回出去
        foreach (var item in objects)
        {
            if (!item.activeSelf)
            {
                item.SetActive(true);
                result = item;
                break;
            }
        }
        //如果没有未激活的物体，则自己实例化一个物体，并设置其父物体然后添加进容器中
        if (result == null)
        {
            result = GameObject.Instantiate(prefab);
            result.transform.SetParent(parent);
            objects.Add(result);
        }
        return result;
    }
    //判断容器中是否存在该物体
    public bool MyContains(GameObject obj)
    {
        return objects.Contains(obj);
    }
    //如果传入的对象属于这个集合，则将其设为隐藏放回池中
    public void PutInPool(GameObject go)
    {
        if (MyContains(go))
        {
            go.SetActive(false);
        }
    }
    //遍历容器中的所有元素并设置为隐藏放回池中
    public void PutAllInPool()
    {
        foreach (var item in objects)
        {
            if (item.activeSelf)
            {
                PutInPool(item);
            }
        }
    }
}