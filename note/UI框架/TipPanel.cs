using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipPanel : UIModule
{
    //private  Text text;
    private void Awake()
    {
    }
    private void Start()
    {
        //text = Resources.Load<Text>("UI/Text");
    }
    public void OnClickCancelButton()
    {
        UIManager.instance.HideUIModule(UIType.TipPanel);
    }
    public void OnClickSureButton()
    {
        UIManager.instance.HideUIModule(UIType.TipPanel);
        //text.gameObject.SetActive(true);
    }
}
