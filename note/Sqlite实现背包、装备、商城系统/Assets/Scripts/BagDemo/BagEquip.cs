using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BagEquip : MonoBehaviour
{
    private Button loadBtn;
    private Button sellBtn;
    private string spriteName;//需要挂载  售卖的装备名字

    private void Awake()
    {
        loadBtn = GetComponent<Button>();
        sellBtn = transform.Find("SellButton").GetComponent<Button>();
    }

    void Start()
    {
        spriteName = GetComponent<Image>().sprite.name;

        if (loadBtn != null)
            loadBtn.onClick.AddListener(() =>
            {
                //数据
                SqlManager.Instance.BagToEquipBox(spriteName);
                //刷新UI: PlayerMsg  Bag
                FlushData.instance.FlushAllDatas();
            });
        if (sellBtn != null)
            sellBtn.onClick.AddListener(() =>
            {
                //数据
                SqlManager.Instance.SellEquip(spriteName);
                //刷新UI: PlayerMsg  Bag
                FlushData.instance.FlushPlayerMsg();
                FlushData.instance.FlushBagBox();
            });
    }
}
