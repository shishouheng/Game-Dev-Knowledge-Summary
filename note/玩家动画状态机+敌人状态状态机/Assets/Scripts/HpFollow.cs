using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpFollow : MonoBehaviour
{
    private Image hp;
    public Transform hpPos;
    
    private void Start()
    {
        Instantiate(Resources.Load("Prefab/Hp"), GameObject.FindWithTag("HpRoot").transform);
        hp = GetComponentInChildren<Image>();
    }
    private void Update()
    {
        hp.rectTransform.position = Camera.main.WorldToScreenPoint(hpPos.position);
    }
}
