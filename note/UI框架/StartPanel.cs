using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartPanel : UIModule
{
    public Text textCount;
    public Text textTitle;
    public override bool ShowPanel(params object[] datas)
    {
        if (!base.ShowPanel(datas))
            return false;
        textCount.text = datas[0].ToString();
        textTitle.text = datas[1].ToString();
        return true;
    }
    public void OnClickSetButton()
    {
        UIManager.instance.ShowUIModule(UIType.SettingPanel);
    }
}
