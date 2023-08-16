using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System;

public enum EquipLocationType
{
    None,//既不在背包栏  又不在装备栏
    Bag = 1,//只在背包栏
    Equip,//只在装备栏
    All = 3//都在
}

public enum EquipPropertyType
{
    ADAdd,
    APAdd,
    ARAdd,
    MGAdd
}

public enum PlayerPropertyType
{
    AD, AP, AR, MG
}

public class SqlManager
{
    #region 单例
    private SqlManager() { }
    private static SqlManager instance;
    public static SqlManager Instance
    {
        get
        {
            if (instance == null)
                instance = new SqlManager();
            return instance;
        }
    }
    #endregion

    #region 数据库常用类
    private SqliteConnection con;
    private SqliteCommand command;
    private SqliteDataReader reader;
    #endregion

    #region 数据库常用操作
    //打开数据库
    public void OpenDataBase(string dataBaseName)
    {
        try
        {
            if (!dataBaseName.Contains(".sqlite"))
                dataBaseName += ".sqlite";
            con = new SqliteConnection("Data Source=" + Application.streamingAssetsPath + "/" + dataBaseName);
            command = con.CreateCommand();
            con.Open();
        }
        catch (SqliteException e)
        {
            Debug.LogError(e.Message);
        }
    }

    //增 删 改
    public void RunSql(string sqlStr)
    {
        try
        {
            //command 中的sql语句赋值
            command.CommandText = sqlStr;
            //执行操作
            command.ExecuteNonQuery();
        }
        catch (SqliteException e)
        {
            Debug.LogError(e.Message);
        }
    }

    //查
    //单个查询
    public object SelectSingle(string sqlStr)
    {
        try
        {
            //command 中的sql语句赋值
            command.CommandText = sqlStr;
            //执行操作
            return command.ExecuteScalar();
        }
        catch (SqliteException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    //多个查询
    public List<ArrayList> SelectMutiple(string sqlStr)
    {
        try
        {
            //command 中的sql语句赋值
            command.CommandText = sqlStr;
            //执行操作
            reader = command.ExecuteReader();//代表的是整张表的内容
            List<ArrayList> datas = new List<ArrayList>();
            while (reader.Read())
            {
                ArrayList al = new ArrayList();
                //reader：  当前行的内容
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    al.Add(reader.GetValue(i));
                }
                datas.Add(al);
            }
            reader.Close();
            return datas;
        }
        catch (SqliteException e)
        {
            Debug.LogError(e.Message);
            return null;
        }
    }

    //关闭数据库
    public void CloseDataBase()
    {
        try
        {
            if (con != null)
                con.Clone();
        }
        catch (SqliteException e)
        {
            Debug.LogError(e.Message);
        }
    }
    #endregion

    #region 当前Demo中常用的方法
    #region 买卖装备
    public EquipLocationType GetEquipLocation(string equipName)
    {
        string sqlStr = "select EquipLocation from EquipTable where EquipName='" + equipName + "'";//根据当前装备 查找 装备位置
        object o = SelectSingle(sqlStr);//int ==>object
        int x = Convert.ToInt32(o);
        EquipLocationType type = (EquipLocationType)x;
        return type;
    }

    public void SetEquipLocation(string equipName, int location)
    {
        string sqlStr = "update EquipTable set EquipLocation=" + location + " where EquipName='" + equipName + "'";
        RunSql(sqlStr);//增删改查
    }

    ///<summary>
    /// 获取当前装备的数量
    ///</summary>
    ///<param name="equipName"></param>
    ///<returns></returns>
    public int GetEquipCount(string equipName)
    {
        //查询当前装备数量的sql语句
        string sqlStr = "select EquipCount from EquipTable where EquipName = '" + equipName + "'";
        //执行查询操作
        int count = Convert.ToInt32(SelectSingle(sqlStr));
        //返回
        return count;
    }

    ///<summary>
    /// 设置装备数量
    ///</summary>
    ///<param name="equipName">装备名称</param>
    ///<param name="count">数量</param>
    public void SetEquipCount(string equipName, int count)
    {
        //设置当前 装备数量的sql语句
        string sqlStr = "update EquipTable set EquipCount = " + count + " where EquipName = '" + equipName + "'";
        //执行sql语句
        RunSql(sqlStr);
    }

    ///<summary>
    /// 当前装备价格
    ///</summary>
    ///<param name="equipName">装备名</param>
    ///<returns>装备价格</returns>
    public int GetEquipCost(string equipName)
    {
        //查询装备价格sql语句
        string sqlStr = "select EquipCost from EquipTable where EquipName = '" + equipName + "'";
        //返回查询价格结果
        return Convert.ToInt32(SelectSingle(sqlStr));
    }

    ///<summary>
    ///获取玩家金币数量
    ///</summary>
    ///<returns></returns>
    public int GetPlayerGold(string playerName = "寒冰射手")
    {
        string sqlStr = "select Gold from PlayerTable where PlayerName = '" + playerName + "'";
        return Convert.ToInt32(SelectSingle(sqlStr));
    }

    ///<summary>
    /// 设置玩家金币数量
    ///</summary>
    ///<param name="count">金币数量</param>
    ///<param name="playerName">玩家名称</param>
    public void SetPlayerGold(int count, string playerName = "寒冰射手")
    {
        string sqlStr = "update PlayerTable set Gold = " + count + " where PlayerName = '" + playerName + "'";
        RunSql(sqlStr);
    }

    ///<summary>
    /// 玩家当前是否能够购买当前装备
    ///</summary>
    ///<param name="equipName">装备名</param>
    ///<returns>bool</returns>
    public bool IsCanBuyEquip(string equipName, string playerName = "寒冰射手")
    {
        if (GetPlayerGold(playerName) >= GetEquipCost(equipName))
        {
            //玩家当前金币大于当前装备价格   可以购买
            return true;
        }
        return false;
    }

    ///<summary>
    /// 获取当前背包总的装备数量
    ///</summary>
    ///<returns></returns>
    public int GetBagEquipCount()
    {
        //取出  数据库中所有装备的EquipCount
        string sqlStr = "select EquipCount from EquipTable where EquipLocation=1 or EquipLocation=3";//not and  or
        //用集合接下
        List<ArrayList> data = SelectMutiple(sqlStr);
        //声明一个变量  用来存储装备数量
        int count = 0;
        //遍历集合
        for (int i = 0; i < data.Count; i++)
        {
            //把数据中的count加一起
            //data[i] 当前行  动态数组的对象； 
            count += Convert.ToInt32(data[i][0]);
        }
        return count;
        //string sqlStr = "select sum(EquipCount) from EquipTable where EquipLocation = 1 or EquipLocation = 3;";
        //object o = SelectSingle(sqlStr);
        //if (o == null)//""
        //    return 0;
        ////返回
        //return Convert.ToInt32(o);
    }

    /// <summary>
    /// 买装备
    /// </summary>
    public void BuyEquip(string equipName, string playerName = "寒冰射手")
    {
        if (!IsCanBuyEquip(equipName, playerName) || GetBagEquipCount() >= 18)
            return;
        //扣钱
        SetPlayerGold(GetPlayerGold() - GetEquipCost(equipName), playerName);

        //装备位置 数量
        switch (GetEquipLocation(equipName))
        {
            //None :Bag;1;
            case EquipLocationType.None:
                SetEquipLocation(equipName, (int)EquipLocationType.Bag);
                SetEquipCount(equipName, 1);
                break;
            //Equip:All;1;
            case EquipLocationType.Equip:
                SetEquipLocation(equipName, (int)EquipLocationType.All);
                SetEquipCount(equipName, 1);
                break;
            //Bag:  Bag ,count++;
            //All: All, count++;
            case EquipLocationType.Bag:
            case EquipLocationType.All:
                int currentCount = GetEquipCount(equipName);
                currentCount++;
                SetEquipCount(equipName, currentCount);
                break;
        }
    }

    public void SellEquip(string equipName, string playerName = "寒冰射手")
    {
        //加钱
        SetPlayerGold(GetPlayerGold(playerName) + GetEquipCost(equipName) / 2, playerName);

        //数量  位置
        SetEquipCount(equipName, GetEquipCount(equipName) - 1);
        if (GetEquipCount(equipName) == 0)
        {
            //Bag==> None
            if (GetEquipLocation(equipName) == EquipLocationType.Bag)
            {
                SetEquipLocation(equipName, (int)EquipLocationType.None);
            }
            //All==>Equip
            else if (GetEquipLocation(equipName) == EquipLocationType.All)
            {
                SetEquipLocation(equipName, (int)EquipLocationType.Equip);
            }
        }
    }

    #endregion

    #region 装备卸载和挂载
    ///<summary>
    /// 获取某个装备 某个属性的加成
    ///</summary>
    ///<param name="equipName"></param>
    ///<returns></returns>
    public int GetEquipPropertyAdd(string equipName, EquipPropertyType type)
    {
        string sqlStr = "select " + type.ToString() + " from EquipTable where EquipName = '" + equipName + "'";
        //查询单个数据 object--->int  返回
        return Convert.ToInt32(SelectSingle(sqlStr));
    }

    ///<summary>
    /// 查询玩家某个属性的属性值
    ///</summary>
    ///<param name="type"></param>
    ///<param name="playerName"></param>
    ///<returns></returns>
    public int GetPlayerProperty(PlayerPropertyType type, string playerName = "寒冰射手")
    {
        string sqlStr = "select " + type.ToString() + " from PlayerTable where PlayerName = '" + playerName + "'";
        return Convert.ToInt32(SelectSingle(sqlStr));
    }

    ///<summary>
    /// 增加玩家属性的方法
    ///</summary>
    ///<param name="equipName"></param>
    ///<param name="playerName"></param>
    public void AddEquipProperty(string equipName, string playerName = "寒冰射手")
    {
        int ADSet = GetPlayerProperty(PlayerPropertyType.AD, playerName) + GetEquipPropertyAdd(equipName, EquipPropertyType.ADAdd);
        int APSet = GetPlayerProperty(PlayerPropertyType.AP, playerName) + GetEquipPropertyAdd(equipName, EquipPropertyType.APAdd);
        int ARSet = GetPlayerProperty(PlayerPropertyType.AR, playerName) + GetEquipPropertyAdd(equipName, EquipPropertyType.ARAdd);
        int MGSet = GetPlayerProperty(PlayerPropertyType.MG, playerName) + GetEquipPropertyAdd(equipName, EquipPropertyType.MGAdd);
        string sqlStr = String.Format("update PlayerTable set AD = {0},AP = {1},AR = {2},MG = {3} where PlayerName = '" + playerName + "'", ADSet, APSet, ARSet, MGSet);
        RunSql(sqlStr);
    }

    ///<summary>
    /// 移除装备属性的时候，对应减少==》移除玩家属性的方法
    ///</summary>
    ///<param name="equipName"></param>
    ///<param name="playerName"></param>
    public void RemoveEquipProperty(string equipName, string playerName = "寒冰射手")
    {
        int ADSet = GetPlayerProperty(PlayerPropertyType.AD, playerName) - GetEquipPropertyAdd(equipName, EquipPropertyType.ADAdd);
        int APSet = GetPlayerProperty(PlayerPropertyType.AP, playerName) - GetEquipPropertyAdd(equipName, EquipPropertyType.APAdd);
        int ARSet = GetPlayerProperty(PlayerPropertyType.AR, playerName) - GetEquipPropertyAdd(equipName, EquipPropertyType.ARAdd);
        int MGSet = GetPlayerProperty(PlayerPropertyType.MG, playerName) - GetEquipPropertyAdd(equipName, EquipPropertyType.MGAdd);
        string sqlStr = String.Format("update PlayerTable set AD = {0},AP = {1},AR = {2},MG = {3} where PlayerName = '" + playerName + "'", ADSet, APSet, ARSet, MGSet);
        RunSql(sqlStr);
    }

    /// <summary>
    /// 获取装备栏 中装备类型（背包栏中当前需要挂载的装备类型） 对应的旧的装备名称；
    /// </summary>
    /// <returns></returns>
    public string GetCurEquipName(string equipName)//equipName 需要挂载的装备名
    {
        //1 通过装备名 查找装备类型
        string sqlStr1 = "select EquipType from EquipTable where EquipName='" + equipName + "'";
        object obj = SelectSingle(sqlStr1);
        string type = obj.ToString();

        //2 通过（类型） 以及 （位置在 Equip or All ） ==》 查找对应 装备名
        string sqlStr2 = "select EquipName from EquipTable where EquipType='" + type + "' and (EquipLocation=2 or EquipLocation=3)";
        object oldEquipName = SelectSingle(sqlStr2);
        if (oldEquipName == null)
        {
            return string.Empty;
        }
        else
        {
            return oldEquipName.ToString();
        }
    }

    //卸载
    public void EquipBoxToBag(string equipName, string playerName = "寒冰射手")
    {
        //保证背包栏还有空余的槽位
        if (GetBagEquipCount() >= 18)
            return;
        //卸载
        //更新装备数量
        SetEquipCount(equipName, GetEquipCount(equipName) + 1);
        //位置
        SetEquipLocation(equipName, (int)EquipLocationType.Bag);
        //移除属性加成
        RemoveEquipProperty(equipName, playerName);
    }

    //挂载
    public void BagToEquipBox(string equipName, string playerName = "寒冰射手")
    {
        //当前需要挂载的装备  对应的类型  在装备栏对应类型的槽位上 是否存在其他的装备
        string oldEquip = GetCurEquipName(equipName);
        if (oldEquip != string.Empty)
        {
            //先卸载oldEquip
            EquipBoxToBag(oldEquip, playerName);
        }
        //挂载==> Bag  All
        //装备数量
        SetEquipCount(equipName, GetEquipCount(equipName) - 1);

        //位置
        if (GetEquipCount(equipName) == 0)
        {
            //背包栏中没有当前装备 ==》 Equip；
            SetEquipLocation(equipName, (int)EquipLocationType.Equip);
        }
        else
        {
            //背包栏中还有当前装备 ==》 All；
            SetEquipLocation(equipName, (int)EquipLocationType.All);
        }

        //增加 属性加成
        AddEquipProperty(equipName, playerName);
    }
    #endregion

    #region 刷新专用
    /// <summary>
    /// 获取装备栏中所有的装备
    /// </summary>
    /// <returns></returns>
    public string GetEquipBoxEquip()
    {
        string sqlStr = "select EquipName,EquipType from EquipTable where EquipLocation=2 or EquipLocation=3";
        List<ArrayList> datas = SelectMutiple(sqlStr);
        //使用分隔符 将所有数据拼接起来
        string result = string.Empty;
        if (datas.Count > 0)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                //datas[i][0] datas[i][1]
                if (i == datas.Count - 1)
                    result += datas[i][0] + "|" + datas[i][1];
                else
                    result += datas[i][0] + "|" + datas[i][1] + "-";
            }
        }
        return result;
    }

    /// <summary>
    /// 获取背包栏中所有得装备
    /// </summary>
    /// <returns></returns>
    public string GetBagBoxEquip()
    {
        string sqlStr = "select EquipName,EquipCount from EquipTable where EquipLocation=1 or EquipLocation=3";
        List<ArrayList> datas = SelectMutiple(sqlStr);
        //使用分隔符 将所有数据拼接起来
        string result = string.Empty;
        if (datas.Count > 0)
        {
            for (int i = 0; i < datas.Count; i++)
            {
                //datas[i][0] datas[i][1]
                if (i == datas.Count - 1)
                    result += datas[i][0] + "|" + datas[i][1];
                else
                    result += datas[i][0] + "|" + datas[i][1] + "-";
            }
        }
        return result;
    }

    /// <summary>
    /// 获取玩家信息栏的数据
    /// </summary>
    /// <param name="playerName"></param>
    /// <returns></returns>
    public List<ArrayList> GetPlayerMsg(string playerName = "寒冰射手")
    {
        string sqlStr = "select*from PlayerTable where PlayerName='" + playerName + "'";
        return SelectMutiple(sqlStr);
    }
    #endregion
    #endregion
}
