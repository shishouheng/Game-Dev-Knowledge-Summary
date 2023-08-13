using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;


public class LoadingManager :MonoBehaviour
{
    public GameObject UI_tips;
    public GameObject UI_loading;
    public GameObject UI_loginAndregister;
    public Slider progressBar;
    public Text progressText;

    // Start is called before the first frame update
    void Start()
    {
        UI_tips.SetActive(true);
        UI_loading.SetActive(false);
        UI_loginAndregister.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
