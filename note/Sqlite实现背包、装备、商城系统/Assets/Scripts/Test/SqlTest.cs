using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class SqlTest : MonoBehaviour
{
    SqliteConnection con;
    SqliteCommand command6;
    SqliteDataReader reader;
    private void Awake()
    {
        //打开数据库 
        con = new SqliteConnection("Data Source=" + Application.dataPath + "/Datas/mysqlitetes1.sqlite");
        con.Open();
    }
    private void Start()
    {
        //增
        ////构建sql语句 (如果当前数据库文件中没有此table，创建；有 不创建；)
        ////primary key autoincrement 主键自增： 主要修饰字段的 索引 唯一性；
        //string sqlStr = "create table if not exists ItemTable(itemId integer primary key autoincrement,itemName text,itemCount integer)";
        ////调用api  执行sql语句
        //SqliteCommand commond = new SqliteCommand(sqlStr, con);
        //commond.ExecuteNonQuery();

        //string sqlStr2 = "insert into ItemTable(itemName,itemCount) values('PBWZZR',1)";
        //SqliteCommand commond2 = new SqliteCommand(sqlStr2, con);
        //commond2.ExecuteNonQuery();

        //删
        //string sqlStr3 = "delete from ItemTable where itemId=10010";
        //SqliteCommand commond3 = new SqliteCommand(sqlStr3, con);
        //commond3.ExecuteNonQuery();

        //改
        //string sqlStr4 = "update ItemTable set itemCount=5 where itemId=10013";
        //SqliteCommand commond4 = new SqliteCommand(sqlStr4, con);
        //commond4.ExecuteNonQuery();

        //查
        //string sqlStr5 = "select count(*) from ItemTable;";
        //SqliteCommand command5 = new SqliteCommand(sqlStr5, con);
        //object o = command5.ExecuteScalar();
        //Debug.Log(o);

        string sqlStr6 = "select*from ItemTable;";
        command6 = new SqliteCommand(sqlStr6, con);
        reader = command6.ExecuteReader();
        while (reader.Read())
        {
            Debug.Log(string.Format("id:{0},name:{1},count:{2}", reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2)));
        }

    }

    private void OnDestroy()
    {
        if (command6 != null)
            command6.Dispose();
        if (reader != null)
            reader.Close();
        if (con != null)
            con.Close();
    }
}
