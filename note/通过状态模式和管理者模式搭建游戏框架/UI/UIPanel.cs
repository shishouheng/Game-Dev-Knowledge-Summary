using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIPanel : MonoBehaviour
{
    public bool DontHide;//不会隐藏
    public bool dialog;//对话框

    private GameObject uiPrefab;//ui预制体

    //打开UI时调用的回调
    public virtual bool OnShow(params object[] userdatas) { return true; }

    //隐藏UI
    public virtual void OnHide(bool hide) { }
}
