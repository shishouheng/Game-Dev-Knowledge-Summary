using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;

public class SqlTest : MonoBehaviour
{
    SqliteConnection con = new SqliteConnection();
    private void Awake()
    {
        //con.Open();
    }
    private void Start()
    {
        //string sqlStr = "create table if not exists ItemTable(itemId integer primary key autoincrement,itemName text,intemCount integar)";
        //SqliteCommand command = new SqliteCommand(sqlStr, con);
        //command.ExecuteNonQuery();
        ConfigsManager.Instance.AddConfig<QuestionConfig>();
    }
    private void OnDestroy()
    {
        //con.Close();
    }
}
