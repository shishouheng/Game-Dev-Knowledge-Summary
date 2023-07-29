using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    //容器1：存储当前项目中的所有界面信息（需要定义一个界面类来存储界面的名字、界面类型、实例化的界面等信息）
    UIModuleData[] uiModules;
    //容器2：存储打开过的界面，避免过度Destroy或者Instanitate新界面
    Dictionary<UIType, UIModuleData> showUIModules = new Dictionary<UIType, UIModuleData>();
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
        OnInitModules();
    }
    private void Start()
    {
        ShowUIModule(UIType.StartPanel, "100", "Role");
    }
    //用来在游戏开始的时候将所有的界面存储到容器1中
    void OnInitModules()
    {
        uiModules = new UIModuleData[]
        {
            new UIModuleData("StartPanel",UIType.StartPanel,true),
            new UIModuleData("SettingPanel",UIType.SettingPanel,true),
            new UIModuleData("TipPanel",UIType.TipPanel,false),
        };
        
    }
    #region //对外提供给每个UI脚本的功能（打开界面、关闭界面）
    
    public void ShowUIModule(UIType type,params object[] datas)
    {
        //先去字典中查看，判断该界面是否打开过（如果打开过，则已被隐藏，重新激活该界面；未打开则实例化该界面）
        UIModuleData data;
        if(showUIModules.TryGetValue(type,out data))//有
        {
            if (data.isHideOtherModule)
                //如果是普通界面则隐藏其他所有界面
                HideAllOtherModule();
            else//说明是提示框，则要让该界面始终位于视图最前方
                data.uiMyPrefab.transform.SetAsLastSibling();
            data.uiMyPrefab.SetActive(true);
            data.uiClass.ShowPanel(datas);
        }
        else//没有
        {
            data=GetUIModule(type);
            if(data==null)
            {
                Debug.LogError("PanelData...............Missing.........!");
                return;
            }
            //判断当前界面是普通界面还是提示框
            if (data.isHideOtherModule)
                //如果是普通界面则隐藏其他所有界面
                HideAllOtherModule();
            //实例化界面
            InstantiateUIPanel(data,datas);
            if (!data.isHideOtherModule)//是提示框
                data.uiMyPrefab.transform.SetAsLastSibling();
        }
    }
    void InstantiateUIPanel(UIModuleData data,params object[] datas)
    {
        //实例化出来的UI界面
        GameObject uiObj=Instantiate(Resources.Load<GameObject>("UI/"+data.prefabName));
        //更新实例化出来的UI界面的父物体、位置信息
        uiObj.transform.SetParent(GameObject.FindWithTag("Canvas").transform);
        uiObj.GetComponent<RectTransform>().offsetMin = Vector2.zero;
        uiObj.GetComponent<RectTransform>().offsetMax= Vector2.zero;
        //UI界面信息进行赋值
        data.uiMyPrefab = uiObj;
        data.uiClass = uiObj.GetComponent<UIModule>();
        //ShowPanel
        data.uiClass.ShowPanel(datas);
        //存到字典中
        showUIModules.Add(data.uiType, data);
    }
    //根据UIType获取对应的UIData
    UIModuleData GetUIModule(UIType type)
    {
        foreach (var item in uiModules)
        {
            if (item.uiType == type)
                return item;
        }
        Debug.Log("该界面未注册到数组中");
        return null;
    }
    public void HideUIModule(UIType type)
    {
        UIModuleData data = showUIModules[type];
        data.uiMyPrefab.SetActive(false);
        data.uiClass.HidePanel();
    }
    public void HideAllOtherModule()
    {
        foreach (var item in showUIModules.Values)
        {
            HideUIModule(item.uiType);
        }
    }
    #endregion
}
//枚举，表示界面的类型
public enum UIType
{
    StartPanel,SettingPanel,TipPanel
}
//界面类，用来存储每个界面所具有的信息
public class UIModuleData
{
    public string prefabName;
    public bool isHideOtherModule;//打开界面的时候是否需要隐藏其他界面（判断该界面是普通界面还是提示框）
    public UIType uiType;
    //当前实例化出来的界面的信息
    public GameObject uiMyPrefab;
    public UIModule uiClass;
    public UIModuleData(string name,UIType type,bool isHide)
    {
        prefabName = name;
        uiType = type;
        isHideOtherModule = isHide;
    }
}