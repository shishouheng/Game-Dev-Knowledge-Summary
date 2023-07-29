using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIModule : MonoBehaviour
{
    //传递任意类型任意数量的参数,在正规游戏开发过程中打开一个界面可能需要进行其他操作，因此最好传递一个可变参数的数组
    public virtual bool ShowPanel(params object[] datas)
    {
        return true;
    }
    public virtual void HidePanel()
    {

    }
}
