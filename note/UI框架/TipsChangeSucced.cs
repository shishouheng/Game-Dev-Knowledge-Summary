using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TipsChangeSucced : MonoBehaviour
{
    public TipPanel panel;
    public Text text;
    public float fadeDuration = 1f;
    private float elapsedTime = 0f;
    private Color orginalColor;
    private bool fadingOut = false;
    private void Start()
    {
        orginalColor = text.color;
    }
    public void Update()
    {
        DisplayText();
    }
    public void DisplayText()
    {
        if (!panel.isActiveAndEnabled && !fadingOut)
        {
            fadingOut = true;
            elapsedTime = 0f;
            if (fadingOut)
            {
                elapsedTime += Time.deltaTime;
                float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
                text.color = new Color(orginalColor.r, orginalColor.g, orginalColor.b, alpha);
                if(elapsedTime>=fadeDuration)
                {
                    fadingOut = false;
                    text.gameObject.SetActive(false);
                }
            }
        }
    }
}
