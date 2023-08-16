using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquipBoxEquip : MonoBehaviour {
    private Button unLoadBtn;
    private string spriteName;//需要购买的装备名字

    private void Awake()
    {
        unLoadBtn =GetComponent<Button>();
    }

    private void Start()
    {
        spriteName = GetComponent<Image>().sprite.name;
        if (unLoadBtn != null)
            unLoadBtn.onClick.AddListener(() =>
            {
                //数据
                SqlManager.Instance.EquipBoxToBag(spriteName);
                //刷新UI: PlayerMsg  Bag
                FlushData.instance.FlushAllDatas();
            });
    }
}
