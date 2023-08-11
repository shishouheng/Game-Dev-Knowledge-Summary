using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class AnswerPanel : MonoBehaviour
{
    public Image progress;
    public Text progressText;

    public Text scoreText;
    public Text questionText;

    public List<Text> optionsText;
    public List<Toggle> optionsToggle;

    private int currentCount = 1;
    private int questionCount;
    private int score;

    public GameObject tipPanel;
    public Text tipText;
    public Button button;
    bool isEnd = false;

    QuestionConfig qc;
    private void Start()
    {
        ConfigsManager.Instance.AddConfig<QuestionConfig>();
        Invoke("GetGameData", 0.2f);
    }
    void GetGameData()
    {
        qc = ConfigsManager.Instance.GetGameConfig<QuestionConfig>();
        ChangeUIData();
        ChangeQuestion();
    }
    void ChangeUIData()
    {
        questionCount = qc.GetQuestionCount();
        float x = (float)(currentCount - 1) / questionCount;
        progress.fillAmount = x;
        progressText.text = string.Format("进度：{0}%", x * 100);
        scoreText.text = score.ToString();
    }
    void ChangeQuestion()
    {
        ResetTog();
        questionText.text = string.Format("第<Color=red>{0}</Color>题:{1}", currentCount, qc.GetDescByIndex((int)(currentCount - 1)));
        for(int i=0;i<optionsText.Count;i++)
        {
            optionsText[i].text = qc.GetListStringIndex((int)(currentCount - 1))[i];
        }
    }
    void ResetTog()
    {
        for(int i=0;i<optionsToggle.Count;i++)
        {
            if (optionsToggle[i].isOn)
                optionsToggle[i].isOn = false;
        }
    }
    public void OnClickConfirmButton()
    {
        for(int i=0;i<optionsToggle.Count;i++)
        {
            if(optionsToggle[i].isOn)
            {
                if((i+1)==qc.GetRightKeyByIndex((int)(currentCount-1)))
                {
                    score += 10;
                    tipText.text = "恭喜你答对了  当前分数：" + score;
                    tipPanel.SetActive(true);
                }
                else
                {
                    tipText.text = "很遗憾答错了  当前分数：" + score;
                    tipPanel.SetActive(true);
                }
                break;
            }
        }
    }
    public void OnClickCloseButton()
    {
        currentCount++;
        if (currentCount > questionCount)
        {
            button.interactable= false;
            tipText.text = "恭喜你完成了所有题目，你的得分是：" + score;
            Invoke("SetPanelActive", 1);
            if(isEnd)
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
            }
        }
        tipPanel.SetActive(false);
        if(currentCount<=questionCount)
        {
            ChangeUIData();
            ChangeQuestion();
        }
    }
    private void SetPanelActive()
    {
        tipPanel.SetActive(true);
        isEnd = true;
    }
}
