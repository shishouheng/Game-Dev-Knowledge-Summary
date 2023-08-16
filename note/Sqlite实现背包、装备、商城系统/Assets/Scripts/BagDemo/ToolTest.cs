using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolTest : MonoBehaviour
{
    private void Awake()
    {
        SqlManager.Instance.OpenDataBase("rpgBagdatabase");
    }

    private void Start()
    {
        //System.Object o = true;//object是所有类型的最基类; int float bool ToolTest 枚举 结构体 UnityEngine.Object等
        //UnityEngine.Object o2 = gameObject;

        //o = o2;
        //o2 = (UnityEngine.Object) o;

        //SqlManager.Instance.RunSql("update PlayerTable set Gold=99999 where PlayerName='寒冰射手';");
        //object o = SqlManager.Instance.SelectSingle("select count(*) from EquipTable;");
        //Debug.Log(o);

        //List<ArrayList> datas = SqlManager.Instance.SelectMutiple("select*from EquipTable;");

        // print(SqlManager.Instance.GetEquipLocation("CSM"));
        //SqlManager.Instance.SetEquipLocation("CSM", (int)EquipLocationType.Bag);

        //int c = SqlManager.Instance.GetBagEquipCount();
        //Debug.Log(c);
        // SqlManager.Instance.SellEquip("CSM");
    }
    private void OnDestroy()
    {
        SqlManager.Instance.CloseDataBase();
    }
}
