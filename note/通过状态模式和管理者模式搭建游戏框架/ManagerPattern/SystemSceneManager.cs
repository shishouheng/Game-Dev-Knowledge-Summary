using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//场景加载完成的回调
public delegate void OnSceneLoadSucceed(string sceneName);

public class SystemSceneManager : SystemModule
{
    public static SystemSceneManager Instance
    {
        get { return SystemModuleManager.Instance.GetSystemModule<SystemSceneManager>(); }
    }

    private OnSceneLoadSucceed onSceneLoaded;

    //获取当前Scene
    public Scene GetCurrentScene
    {
        get { return SceneManager.GetActiveScene(); }
    }

    //场景同步加载的方法
    public void ChangeScene(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
            return;

        SceneManager.LoadScene(sceneName);
    }

    public void ChangeScnee(int sceneIndex)
    {
        if (sceneIndex < 0)
            return;

        SceneManager.LoadScene(sceneIndex);
    }

    //场景异步加载的方法
    public void ChangeSceneByAsync(int index,OnSceneLoadSucceed onSceneLoaded)
    {
        if (index < 0)
            return;

        string sceneName = SceneManager.GetSceneAt(index).name;
        ChangeSceneByAsync(sceneName, onSceneLoaded);
    }

    public void ChangeSceneByAsync(string  sceneName, OnSceneLoadSucceed onSceneLoaded)
    {
        if (string.IsNullOrEmpty(sceneName))
            return;
        StartCoroutine(LoadSceneAsync(sceneName, onSceneLoaded));
    }

    IEnumerator LoadSceneAsync(string sceneName,OnSceneLoadSucceed onSceneLoaded)
    {
        //切换场景的时候打开Loading界面
        SystemUIManager.Instance.ShowLoadingPanel(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        while(!operation.isDone)
        {
            //这里可以更新Loading界面的进度条
            yield return null;
        }
        yield return new WaitForSeconds(2f);

        //切换场景后清空已显示界面信息
        SystemUIManager.Instance.Dispose();
        //关闭loading界面
        SystemUIManager.Instance.ShowLoadingPanel(false);
        onSceneLoaded(sceneName);

    }
}
