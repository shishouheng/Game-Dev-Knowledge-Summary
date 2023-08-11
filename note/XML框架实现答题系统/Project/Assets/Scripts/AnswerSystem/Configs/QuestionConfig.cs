using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;

public class QuestionConfig : Config
{
    private List<QuestionData> questions = new List<QuestionData>();
    public override bool ReadLoad(string xmlText)
    {
        if (!base.ReadLoad(xmlText))
            return false;
        XmlDocument xml = new XmlDocument();
        xml.LoadXml(xmlText);
        XmlElement root = xml.DocumentElement;

        //将每个Question存入数组中
        foreach (XmlElement item in root.ChildNodes)
        {
            QuestionData questionData = new QuestionData(item);
            questions.Add(questionData);
        }
        return true;
    }
    public int GetQuestionCount()
    {
        return questions.Count; 
    }
    public string GetDescByIndex(int index)
    {
        return questions[index].description;
    }
    public List<string> GetListStringIndex(int index)
    {
        return questions[index].options;
    }
    public int GetRightKeyByIndex(int index)
    {
        return questions[index].rightKey;
    }
}
public class QuestionData
{
    public string description;
    public int rightKey;
    //用来存储每个Question的四个选项
    public List<string> options = new List<string>();

    //实例化QuestionData的同时将选项存储到数组中
    public QuestionData(XmlElement ele)
    {
        description = ele.GetAttribute("Desc");
        rightKey = int.Parse(ele.GetAttribute("RightKey"));
        foreach (XmlElement item in ele.ChildNodes)
        {
            options.Add(item.InnerText);
        }
    }
}
