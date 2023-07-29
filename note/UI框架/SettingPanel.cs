using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingPanel : UIModule
{
    public void OnClickSettingDeclineButton()
    {
        UIManager.instance.ShowUIModule(UIType.StartPanel,"300","test");
    }
    public void IsSure()
    {
        UIManager.instance.ShowUIModule(UIType.TipPanel);
    }
}
