using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopEquip : MonoBehaviour
{
    private Button buyBtn;
    private string spriteName;//需要购买的装备名字

    private void Awake()
    {
        buyBtn = transform.GetComponentInChildren<Button>();
        spriteName = GetComponent<Image>().sprite.name;
    }

    private void Start()
    {
        if (buyBtn != null)
            buyBtn.onClick.AddListener(() =>
            {
                //数据
                SqlManager.Instance.BuyEquip(spriteName);
                //刷新UI: PlayerMsg  Bag
                FlushData.instance.FlushPlayerMsg();
                FlushData.instance.FlushBagBox();
            });
    }
}
