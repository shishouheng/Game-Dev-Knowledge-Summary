using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlushData : MonoBehaviour
{
    public static FlushData instance;
    private Text nameText;
    private Text levelText;
    private Text goldText;
    private Text adText;
    private Text apText;
    private Text arText;
    private Text mgText;

    //刷新背包栏时  需要用到的对象
    private GameObject bagEquipPrefab;
    private Transform slotParent_Bag;

    //刷新装备栏时  需要用到的对象
    private GameObject equipBoxEquipPrefab;
    private Transform slotParent_Equip;

    private void Awake()
    {
        instance = this;
        InitCom();
    }

    void InitCom()
    {
        nameText = transform.Find("RoleMsg/name").GetComponent<Text>();
        levelText = transform.Find("RoleMsg/level/Text").GetComponent<Text>();
        goldText = transform.Find("RoleMsg/gold").GetComponent<Text>();
        adText = transform.Find("RoleMsg/Property/Grid/AD/Text").GetComponent<Text>();
        apText = transform.Find("RoleMsg/Property/Grid/AP/Text").GetComponent<Text>();
        arText = transform.Find("RoleMsg/Property/Grid/AR/Text").GetComponent<Text>();
        mgText = transform.Find("RoleMsg/Property/Grid/MG/Text").GetComponent<Text>();

        bagEquipPrefab = Resources.Load<GameObject>("BagEquip");
        slotParent_Bag = transform.Find("BagWindow/grid");

        slotParent_Equip = transform.Find("EquipWindow/Grid").transform;
        equipBoxEquipPrefab = Resources.Load<GameObject>("EquipBoxEquip");
    }

    private void Start()
    {
        FlushAllDatas();
    }

    public void FlushAllDatas()
    {
        FlushPlayerMsg();
        FlushBagBox();
        FlushEquipBox();
    }

    public void FlushPlayerMsg()
    {
        List<ArrayList> datas = SqlManager.Instance.GetPlayerMsg();//datas[0][]
        nameText.text = datas[0][0].ToString();
        levelText.text = datas[0][1].ToString();
        goldText.text = datas[0][3].ToString();
        adText.text = datas[0][4].ToString();
        apText.text = datas[0][5].ToString();
        arText.text = datas[0][6].ToString();
        mgText.text = datas[0][7].ToString();
    }

    public void FlushBagBox()
    {
        ClearBagDatas();
        // CSM|2-XSZR|1......
        string data = SqlManager.Instance.GetBagBoxEquip();
        if (data == string.Empty)
            return;
        string[] equips = data.Split('-');
        for (int i = 0; i < equips.Length; i++)
        {
            //Debug.Log(equips[i]);// CSM|2
            string[] equip = equips[i].Split('|');
            //Debug.Log(equip[0]+equip[1]);
            LoadEquipToBagBoxUI(equip[0], int.Parse(equip[1]));
        }
    }

    void ClearBagDatas()
    {
        for (int i = 0; i < slotParent_Bag.childCount; i++)
        {
            if (slotParent_Bag.GetChild(i).childCount != 0)
                DestroyImmediate(slotParent_Bag.GetChild(i).GetChild(0).gameObject);
        }
    }

    void LoadEquipToBagBoxUI(string equipName, int equipCount)
    {
        for (int i = 0; i < equipCount; i++)
        {
            //实例化
            GameObject curEquip = Instantiate(bagEquipPrefab);//Awake
            //更新图片
            Sprite pic = Resources.Load<Sprite>("Equips/" + equipName);
            curEquip.GetComponent<Image>().sprite = pic;
            //更新父物体 位置
            int index = GetBagEquipIndex();
            if (index == -1)
                return;
            curEquip.transform.SetParent(slotParent_Bag.GetChild(index), false);
            curEquip.transform.localPosition = Vector3.zero;
        }
    }

    int GetBagEquipIndex()//slotParent_Bag.GetChild(index)
    {
        int index = -1;
        if (slotParent_Bag == null)
            return index;
        for (int i = 0; i < slotParent_Bag.childCount; i++)
        {
            if (slotParent_Bag.GetChild(i).childCount == 0)
            {
                index = i;//空白的槽位的下标
                break;
            }
        }
        return index;
    }

    public void FlushEquipBox()
    {
        ClearEquipDatas();
        //获取装备栏 装备
        string data = SqlManager.Instance.GetEquipBoxEquip();
        if (data == string.Empty)
        {
            return;
        }

        //分割字符串
        string[] equips = data.Split('-');
        for (int i = 0; i < equips.Length; i++)
        {
            string[] equip = equips[i].Split('|');
            //拿到了装备的属性和  装备名
            //加载到UI上
            LoadEquipToEquipBox(equip[0], equip[1]);
        }
    }

    void ClearEquipDatas()
    {
        for (int i = 0; i < slotParent_Equip.childCount; i++)
        {
            if (slotParent_Equip.GetChild(i).childCount != 0)
                DestroyImmediate(slotParent_Equip.GetChild(i).GetChild(0).gameObject);
        }
    }
    ///<summary>
    /// 根据装备名 和装备类型 加载装备到UI上
    ///</summary>
    ///<param name="equipName"></param>
    ///<param name="equipType"></param>
    private void LoadEquipToEquipBox(string equipName, string equipType)
    {
        //1.根据模板  赋值一个装备
        GameObject current = Instantiate(equipBoxEquipPrefab);
        //2.获取该装备的贴图
        Sprite curPic = Resources.Load<Sprite>("Equips/" + equipName);
        //3.贴图赋值给装备上的Image组件
        current.transform.GetComponent<Image>().sprite = curPic;

        //设置位置
        //通过装备类型  获取子物体的index
        int index = GetEquipTypeIndex(equipType);
        //通过index 获取grid的子物体  为装备的父物体
        Transform parent = slotParent_Equip.GetChild(index);
        //设置父物体
        current.transform.SetParent(parent, false);
        //位置处于父物体的中心
        current.transform.localPosition = Vector3.zero;
    }

    ///<summary>
    /// 传入装备类型返回子物体相对父物体的index
    ///</summary>
    ///<param name="equipType"></param>
    ///<returns></returns>
    private int GetEquipTypeIndex(string equipType)
    {
        switch (equipType)
        {
            case "AD":
                return 0;
            case "AP":
                return 1;
            case "AR":
                return 2;
            case "MG":
                return 3;
            default:
                return -1;
        }

    }
}
