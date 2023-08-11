using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ConfigsManager : SingleTemplate<ConfigsManager>
{
    private string path;
    public Dictionary<Type, Config> configDic = new Dictionary<Type, Config>();
    #region 读取配置文件
    public void AddConfig<T>()where T:Config,new()
    {
        Config config = new T();
        StartCoroutine(LoadXML(config));
    }
    IEnumerator LoadXML(Config config)
    {
        path =
#if UNITY_ANDROID
            "jar:file//"+Application.dataPath+"!/assets/"+config.ToString()+".xml";
#elif UNITY_IPHONE
            "file//"+Application.dataPath+"/Raw/"+config.ToString().".xml";
#else
            Application.streamingAssetsPath + "/" + config.ToString() + ".xml";
#endif
        WWW www = new WWW(path);
        yield return www;
        config.ReadLoad(www.text);
        configDic.Add(config.GetType(), config);
    }
    #endregion
    #region 根据配置文件获取内容
    public T GetGameConfig<T>()where T:Config
    {
        if (configDic.ContainsKey(typeof(T)))
            return configDic[typeof(T)] as T;
        return null;
    }
    #endregion
}
