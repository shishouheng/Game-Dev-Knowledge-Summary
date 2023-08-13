using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : SingleTemplate<PoolManager>
{
    Dictionary<string, SubPool> pools = new Dictionary<string, SubPool>();
    private void RegisterNewPool(string name, Transform trans)
    {
        GameObject prefab = Resources.Load<GameObject>("GamePrefab/" + name);
        SubPool pool = new SubPool(trans, prefab);
        pools.Add(name, pool);
    }
    
    public GameObject GetOutObj(string name, Transform trans)
    {
        if (!pools.ContainsKey(name))
        {
            RegisterNewPool(name, trans);
        }
        GameObject obj = pools[name].GetOutObject();
        return obj;
    }
    
    public void PutInPool(GameObject obj)
    {
        foreach (SubPool pool in pools.Values)
        {
            if (pool.MyContains(obj))
            {
                pool.PutInPool(obj);
                break;
            }
        }
    }
    public void PutInPool(GameObject obj,float delay)
    {
        StartCoroutine(PutInPoolWithDelay(obj, delay));
    }
    private IEnumerator PutInPoolWithDelay(GameObject obj,float delay)
    {
        yield return new WaitForSeconds(delay);
        PutInPool(obj);
    }
    //将所有池子返回容器的方法
    public void PutAllInPool()
    {
        foreach (SubPool pool in pools.Values)
        {
            pool.PutAllInPool();
        }
    }
}
