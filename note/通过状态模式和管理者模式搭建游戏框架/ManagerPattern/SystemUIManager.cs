using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIType
{
    UIPanelLogin,
    UIPanelMain,
    UIPanelBattle,
    UIPanelLoading
}

//UI界面数据类
public class UIModuleData
{
    public string prefabName;
    public UIType type;//UI枚举类型
    public bool hideOtherModule;//当UI打开时是否需要隐藏其他界面
    public GameObject uiObj;//UI创建后的游戏物体
    public UIPanel uiclass;//UI类对象

    public UIModuleData(string prefabName,UIType type,bool hideOtherModule)
    {
        this.prefabName = prefabName;
        this.type = type;
        this.hideOtherModule = hideOtherModule;
        uiObj = null;
        uiclass = null;
    }
}
public class SystemUIManager : SystemModule
{
    public static SystemUIManager Instance
    {
        get { return SystemModuleManager.Instance.GetSystemModule<SystemUIManager>(); }
    }

    //存储所有界面信息
    private UIModuleData[] uiModules;
    //所有在场景中打开过的UI界面
    private Dictionary<UIType, UIModuleData> showUIModules = new Dictionary<UIType, UIModuleData>();

    public override bool Initialize()
    {
        //游戏初始化时将所有UI界面注册进数组中
        uiModules = new UIModuleData[]
        {
            new UIModuleData("UIPanelLogin",UIType.UIPanelLogin,true),
            new UIModuleData("UIPanelMain",UIType.UIPanelMain,true),
            new UIModuleData("UIPanelBattle",UIType.UIPanelBattle,true),
        };
        return true;
    }

    private UIModuleData loadingPanelModule;

    //loading界面较为特殊（因为所有的loading都是一个界面只是换了背景图），需要单独管理
    //提供给外部的打开loading界面的接口
    public void ShowLoadingPanel(bool open)
    {
        //如果为空则实例化
        if(loadingPanelModule==null)
        {
            loadingPanelModule = new UIModuleData("UIPanelLoading", UIType.UIPanelLoading, true);
            GameObject uiObj = Instantiate(Resources.Load<GameObject>("UI/" + loadingPanelModule.prefabName));

            loadingPanelModule.uiclass = uiObj.GetComponent<UIPanel>();
            loadingPanelModule.uiObj = uiObj;
            DontDestroyOnLoad(loadingPanelModule.uiObj);
        }
        loadingPanelModule.uiObj.SetActive(open);
        loadingPanelModule.uiclass.OnHide(!open);
    }

    //UI管理类的界面显示方法
    public void ShowUIModule(UIType type,params object[]userDatas)
    {
        UIModuleData data;
        //如果字典中有
        if(showUIModules.TryGetValue(type,out data))
        {
            //显示界面时是否需要隐藏其他界面
            if(data.hideOtherModule)
            {
                HideModules();
            }

            //设置UI为激活状态
            HideUIModule(type, false);
            //UI显示时的回调
            data.uiclass.OnShow(userDatas);
        }
        //字典中没有
        else
        {
            data = GetUIModuleDataByType(type);
            if(data==null)
            {
                Debug.LogError("UIModuleData Missing");
                return;
            }
            //显示界面是否需要隐藏其他界面
            if(data.hideOtherModule)
            {
                HideModules();
            }
            //实例化
            SetUIInfo(data, userDatas);
        }
    }

    //显示和激活UI界面的方法
    public void HideUIModule(UIType type,bool hide)
    {
        UIModuleData data = showUIModules[type];
        if(hide)
        {
            data.uiclass.OnHide(hide);
            data.uiObj.SetActive(!hide);
        }
        else
        {
            data.uiclass.OnHide(!hide);
            data.uiObj.SetActive(hide);
        }
    }

    //获取UI界面的方法
    private UIModuleData GetUIModuleDataByType(UIType type)
    {
        foreach (var item in uiModules)
        {
            if (item.type == type)
                return item;
        }

        return null;
    }

    //实例化UI界面的方法
    GameObject SetUIInfo(UIModuleData data,params object[]userdatas)
    {
        GameObject uiObj = Instantiate(Resources.Load<GameObject>("UI/" + data.prefabName));

        UIPanel module = uiObj.GetComponent<UIPanel>();
        data.uiclass = module;
        data.uiObj = uiObj;

        if (module.OnShow(userdatas) == true)
            showUIModules.Add(data.type, data);

        return uiObj;
    }

    //隐藏所有界面的方法
    void HideModules()
    {
        foreach (var item in showUIModules)
        {
            if (!item.Value.uiclass.DontHide)
                HideUIModule(item.Value.type, true);
        }
    }

    //清空数据
    public override void Dispose()
    {
        showUIModules.Clear();
    }
}
