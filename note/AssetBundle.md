
AssetBundle是Unity中的一种资源管理方式，允许我们将多个资源打包到一个文件中，然后在运行时动态的加载这些资源。AssetBundle可以包含各种类型的资源，如模型、纹理、音频、场景等，它们可以用来减少应用程序的初始下载大小，提高资源加载速度，并支持动态更新资源

## 常用的API总结：
### 打包：

`BuildPipeLine.BuildAssetBundles():` 打包的方法，可以将具有相同AssetBundle名字的资源打包到同一个AssetBundle中，该方法的参数可以设置资源的压缩方式和目标平台

- `BuildAssetBundleOptions.UncompressedAssetBundle:` 不对AssetBundle进行压缩，资源较大，加载较快
- `BuildAssetBundleOptions.None:` LZMA格式压缩，是能将资源压缩最小的方式，同样加载时间也是最长的
- `BuildAssetBundleOptions.ChunkBasedCompression:` LZ4压缩方式，资源更小，但是下载后加载时间较长
### 下载;

`UnityWebRequest.GetAssetBundle():` 传入路径获得下载AssetBundle的UnityWebRequest对象

### 加载：

`AssetBundle.UnLoad(bool):` 用于卸载AssetBundle在内存中的占用，该方法接受一个布尔类型的参数，如果传入true，则会同时卸载资源的内存镜像和Load对象，如果传入false，则只会卸载资源的内存镜像。
# 一、打包

资源打包的过程中有单独打包和整体打包两种不同的打包方式
- 单独打包：将每个资源单独打包成一个独立的AssetBundle文件，适用于需要动态加载和卸载特定资源的场景，优点是可以精准的控制资源的加载和卸载，减少内存占用，缺点是增加了管理和维护的复杂性
- 整体打包：将多个资源打包到一个AssetBundle文件中，一个AssetBundle包含多个资源，适用于需要一次性加载多个资源的场景，优点是简化了管理和维护，缺点是无法精准控制每个资源的加载和卸载，可能会增加内存占用


```c#
public class AssetBundleEditor : MonoBehaviour
{
    //单独打包,将AssetbundleName相同的打包到同一个文件中，不同的打包到不同的文件
    [MenuItem("AssetBundle/BuildSingle")]
    static void Build_AssetBundle()
    {
        BuildPipeline.BuildAssetBundles(Application.dataPath + "/StreamingAssets", BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);
        AssetDatabase.Refresh();
    }


    //整体打包
    [MenuItem("AssetBundle/BuildCollection")]
    static void Build_AssetBundles()
    {
        //获得鼠标选中的资源对象
        Object[] selects = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);

        //多个资源打包到一个文件中并设置资源名为AllBundles
        AssetBundleBuild[] buildMap = new AssetBundleBuild[1];
        buildMap[0].assetBundleName = "AllBundles";

        string[] allAssets = new string[selects.Length];
        for(int i=0;i<selects.Length;i++)
        {
            //获得选中的资源路径
            allAssets[i] = AssetDatabase.GetAssetPath(selects[i]);
        }
        //将资源添加到捆绑包中
        buildMap[0].assetNames = allAssets;

        BuildPipeline.BuildAssetBundles(Application.dataPath + "/StreamingAssets", buildMap, BuildAssetBundleOptions.DeterministicAssetBundle | BuildAssetBundleOptions.ChunkBasedCompression, BuildTarget.StandaloneWindows);

        //刷新资源
        AssetDatabase.Refresh();
    }
}
```


# 二、下载

AssetBundle的文件再打包后可以上传到服务i或者本地以供用户在游戏过程中下载游戏资源（通常是上传到服务i），

```c#
public class Loder : MonoBehaviour
{
    public static string m_pathURL;
    private void Awake()
    {
    //不同平台的下载路径
        m_pathURL =
#if UNITY_ANDROID
            "jar::file://"+Application.dataPath+"!/assets/";
#elif UNITY_IPHONE
            Application.dataPath+"/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
            "file://" + Application.dataPath + "/StreamingAssets/";
#else
        string.Empty;
#endif

    }

    private void OnGUI()
    {
        //通过点击不同的按钮加载不同的资源
        if (GUILayout.Button("加载Cube"))
            StartCoroutine(LoadAssetBundleSingle("cube"));
        if (GUILayout.Button("加载capsule"))
            StartCoroutine(LoadAssetBundleSingle("capsule"));
        if (GUILayout.Button("整体加载"))
            StartCoroutine(LoadAssetBundleAll());
    }

    //加载单个资源包
    IEnumerator LoadAssetBundleSingle(string assetName)
    {
        //创建并发送下载的请求
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(m_pathURL + assetName);
        request.SendWebRequest();

        //通过协程进行异步加载
        while (!request.isDone)
            yield return null;

        //如果遇到网络错误或者地址错误则结束协程
        if (request.isHttpError || request.isNetworkError)
            yield break;

        //获得下载请求中的AssetBundle资源
        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        //获取该AssetBundle下的所有资源名
        string[] assetNames = ab.GetAllAssetNames();
        //通过LoadAsset方法遍历AssetBundle并实例化所有资源
        foreach (string name in assetNames)
        {
            Instantiate(ab.LoadAsset(name));
        }
    }

    //加载位于同一资源包下的所有资源
    IEnumerator LoadAssetBundleAll()
    {
        UnityWebRequest request = UnityWebRequest.GetAssetBundle(m_pathURL + "allbundles");
        request.SendWebRequest();

        while (!request.isDone)
            yield return null;

        if (request.isHttpError || request.isNetworkError)
            yield break;

        AssetBundle ab = DownloadHandlerAssetBundle.GetContent(request);
        string[] assetNames = ab.GetAllAssetNames();
        foreach (string name in assetNames)
        {
            Instantiate(ab.LoadAsset(name));
        }
    }
}

```


# 三、卸载

对于程序中确定不再使用的AssetBundle资源包，需要及时卸载来优化内存空间，Unity中卸载AssetBundle的方法是AssetBundle.Unload(bool)，传入true或false都具有不同的效果
- `AssetBundle.Unload(false):` 仅卸载AssetBundle文件的内存镜像，不包含已经被加载到内存中的资源对象
- `AssetBundle.Unload(true):` 释放AssetBundle文件的内存镜像并销毁所有资源对象，无论其是否加载


![](https://github.com/shishouheng/Unity-learning/blob/main/images/assetbundle.png)
# 四、总结

## 1.Unity的资源加载机制

unity的资源加载机制有两种，一种是通过Resources本地加载资源，一种是通过AssetBundle再网络或者本地加载资源。

不管是从磁盘还是服务器下载，AssetBundle资源包都会先在内存中生成一个内存镜像，然后通过AssetBundle.Load对资源进行加载并放入AssetBundle内置缓存中，之后通过实例化或引用复用这些资源形成在Scene中能实际观察到的对象


## 2.Unity的资源卸载机制

所有Scene中的实例化对象和引用可以通过Destroy来释放销毁
AssetBundle加载的内存镜像需要通过AssetBundle.UnLoad(false)来卸载
如果需要同时卸载Load的资源，就需要使用AssetBundle.UnLoad(true)来同时卸载内存镜像和Load中的资源