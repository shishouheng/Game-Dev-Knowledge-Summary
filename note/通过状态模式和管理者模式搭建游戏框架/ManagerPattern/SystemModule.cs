using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SystemModule : MonoBehaviour
{
    //系统模块初始化
    public virtual bool Initialize() { return true; }
    //系统模块执行
    public virtual void Run(object userdata) { }
    //系统模块更新
    public virtual void OnUpdate() { }
    //系统模块释放
    public virtual void Dispose() { }
}
