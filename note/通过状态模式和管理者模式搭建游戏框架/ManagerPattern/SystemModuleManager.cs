using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//后续增加模块都需要通过这个类提供的api来实现
public class SystemModuleManager
{
    //单例
    private static SystemModuleManager instance;
    public static SystemModuleManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SystemModuleManager();
            return instance;
        }
    }

    private SystemModuleManager() { }

    //整个游戏的根物体，挂载入口脚本
    private GameObject rootGameObject;
    //整个游戏的初始化函数
    public void Initialize(GameObject rootGameObject)
    {
        this.rootGameObject = rootGameObject;
    }

    //所有的管理者对象集合
    private Dictionary<Type, SystemModule> type_modules = new Dictionary<Type, SystemModule>();
    //用来遍历获取的集合
    public List<SystemModule> modules = new List<SystemModule>();

    /// <summary>
    /// 添加系统模块的方法，游戏初始化时调用
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T AddSystemModule<T>()where T:SystemModule
    {
        Type t = typeof(T);

        if (type_modules.ContainsKey(t))
            return type_modules[t] as T;

        SystemModule module = rootGameObject.AddComponent<T>();

        if(module.Initialize())
        {
            modules.Add(module);
            type_modules.Add(t, module);
            return module as T;
        }
        return null;
    }

    /// <summary>
    /// 查找获取系统模块
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetSystemModule<T>()where T:SystemModule
    {
        Type t = typeof(T);

        if(!type_modules.ContainsKey(t))
        {
            for(int i=0;i<modules.Count;i++)
            {
                if (modules[i].GetType().IsSubclassOf(t))
                    return modules[i] as T;
            }
            return null;
        }
        return type_modules[t] as T;
    }


    public void OnUpdate()
    {
        for(int i=0;i<modules.Count;i++)
        {
            var module = modules[i];
            if (module != null)
                module.OnUpdate();
        }
    }
}
