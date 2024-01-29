在unity中接入SDK的方式有两种：
- 该SDK有对应的Unity版本，则直接将该SDK导入unity中即可
- 无Unity版本SDK，则需要通过Android Studio创建一个与Unity交互的安卓项目，并将该安卓项目通过aar包的方式导入Unity中然后接入


# 一、接入百度定位SDK


 由于百度定位的SDK没有专门针对Unity的版本 ，因此首先需要通过Android Studio来创建一个与Unity交互的安卓项目，然后将百度定位的SDK接入到该安卓工程，最后导出aar包并导入Unity以供Unity内部调用即可

## 1.创建交互项目

- Unity项目设置好PackageName和KeyStore后创建一个空的安卓工程
- 该安卓工程的PackageName和Minimum SDK版本需要与Unity中的一致
- 交互项目创建完成后删除工程中的两个Test项目以及res下的无用资源
 ![[Pasted image 20240128213929.png]]
 ![]([Unity-learning/images/baiduSDK/delete.png at main · shishouheng/Unity-learning (github.com)](https://github.com/shishouheng/Unity-learning/blob/main/images/baiduSDK/delete.png))

- 接着需要配置build.gradle文件，切换到project视图，打开app下的build.gradle文件，由于我们是要导出aar文件，因此需要将
```java
plugins {  
id 'com.android.application'  
}
```

更改为
```java
plugins {  
id 'com.android.library'  
}
```

- 然后删除defaultConfig中的applicationId
- 将Unity安装路径下的classes.jar包导入安卓工程的app/libs路径下并设置Add as library
``` 
D:\unity\2022.3.16f1c1\Editor\Data\PlaybackEngines\AndroidPlayer\Variations\mono(il2cpp)\Release\Classes
```

- 将Unity安装路径下的UnityPlayerActivity脚本拷贝到安卓工程的app/src/main/java路径下
```
D:\unity\2022.3.16f1c1\Editor\Data\PlaybackEngines\AndroidPlayer\Source...
\UnityPlayerActivity
```

![[import depend.png]]
![]([Unity-learning/images/baiduSDK/import depend.png at main · shishouheng/Unity-learning (github.com)](https://github.com/shishouheng/Unity-learning/blob/main/images/baiduSDK/import%20depend.png))
- 接着需要修改MainActivity脚本（改为继承UnityPlayerActivity,然后注释OnCreate函数中的setContentView代码
```java
package com.personal.sdkPractice;  

import android.os.Bundle;  
  
import com.unity3d.player.UnityPlayerActivity;  
  
public class MainActivity extends UnityPlayerActivity {  
  
@Override  
protected void onCreate(Bundle savedInstanceState) {  
super.onCreate(savedInstanceState);  
}  
}
```

- 修改AndroidManifest.xml配置文件的内容

```xml
<?xml version="1.0" encoding="utf-8"?>  
<manifest xmlns:android="http://schemas.android.com/apk/res/android"  
xmlns:tools="http://schemas.android.com/tools">  
  
<application  
android:allowBackup="true"  
android:dataExtractionRules="@xml/data_extraction_rules"  
android:fullBackupContent="@xml/backup_rules"  
android:icon="@mipmap/ic_launcher"  
android:label="@string/app_name"  
android:roundIcon="@mipmap/ic_launcher_round"  
android:supportsRtl="true"  
android:theme="@style/Theme.BaiduSDK"  
tools:targetApi="31">  
<activity  
android:name=".MainActivity"  
android:exported="true">  
<intent-filter>  
<action android:name="android.intent.action.MAIN" />  
  
<category android:name="android.intent.category.LAUNCHER" />  
</intent-filter>  
</activity>  
</application>  
  
</manifest>


//改为以下内容


<?xml version="1.0" encoding="utf-8"?>  
<manifest xmlns:android="http://schemas.android.com/apk/res/android"  
xmlns:tools="http://schemas.android.com/tools">  
  
<application>  
<activity  
android:name=".MainActivity"  
android:exported="true">  
<intent-filter>  
<action android:name="android.intent.action.MAIN" />  
<category android:name="android.intent.category.LAUNCHER" />  
</intent-filter>  
<meta-data android:name="unityplayer.UnityActivity" android:value="true"/>  
</activity>  
</application>  
  
</manifest>

```


## 2.下载对应SDK并获取AK码

在百度开放平台下载好所需的SDK后需要获取对应的密钥，可通过在Android Studio的Terminal中输入
```
keytool -list -v -keystore D:\UnityProject\SDKPractice\SDKPractice\androidSDK.keystore -alias mygame

```

如果该种方式无效，则在Android Studio安装目录下找到keytool.exe工具，复制地址，然后通过cmd打开并输入keytool -list -v -keystore D:\UnityProject\SDKPractice\SDKPractice\androidSDK.keystore -alias mygame这段命令然后输入密钥库的密码，即可获得SHA1码

在百度的控制台输入发布版的SHA1码后即可获得应用的AK码


## 3.将百度SDK导入安卓项目并修改配置和权限

将下载好的百度定位SDK文件中的所有文件拷贝到安卓项目的app/libs路径下（其中的jar包需要add as library)

然后根据官方文档的内容进行配置修改

在AndroidMainifest.xml中填写AK码
```xml
<meta-data android:name="com.baidu.lbsapi.API_KEY" android:value="AK" > </meta-data>
```

## 4.Unity调用对应API

Unity如果想要调用SDK的api，需要先按照官方文档说明并在安卓工程中添加对应的代码，然后在安卓工程中通过UnityPlayer.UnitySendMessage将相关信息传递给Unity，最后生成aar包导入Unity

```c#
public class BaiduSDK : MonoBehaviour  
{  
    [SerializeField] private Button btn;  
  
    [SerializeField] private Text location;  
  
    private void Start()  
    {        //点击按钮时调用安卓端的代码，固定写法  
        btn.onClick.AddListener((() =>  
        {  
            using (AndroidJavaClass ajc = new AndroidJavaClass("com.unity3d.UnityPlayer"))  
            {                using (AndroidJavaObject ajo=ajc.GetStatic<AndroidJavaObject>("currentActivity"))  
                {                    ajo.Call("GetAddr");  
                }            }        }));  
    }  
    //提供给安卓端调用的代码  
    public void GetAddr(string addr)  
    {        location.text = addr;  
    }}
```

整体的逻辑就是当用户点击按钮时会调用安卓项目里的GetAddr方法来获取定位信息，然后在安卓里通过UnityPlayer.UnitySendMessage再将获取到的定位信息传递给unity

```java
public class MyLocationListener extends BDAbstractLocationListener  
{  
@Override  
public void onReceiveLocation(BDLocation location)  
{  
String addr = location.getAddrStr(); //获取详细地址信息  
String country = location.getCountry(); //获取国家  
String province = location.getProvince(); //获取省份  
String city = location.getCity(); //获取城市  
String district = location.getDistrict(); //获取区县  
String street = location.getStreet(); //获取街道信息  
String adcode = location.getAdCode(); //获取adcode  
String town = location.getTown(); //获取乡镇信息  
//第一个参数是场景中的物体名，第二个参数是该物体身上的脚本的方法名，第三个参数是传递的数据
UnityPlayer.UnitySendMessage("BaiduSDK","GetAddr",addr);  
}  
}
```

代码完成后即可导出aar包，在安卓工程中点击Build-Make Module "xxxxx"，等待打包完成后在Project视图中按照app-build-outputs找到对应aar包复制一份导入到Unity中的Plugins-Android路径下，然后再将安卓工程app-src-main-AndroidMainfest.xml也导入到unity中同样的位置

在Unity中导入aar包后并不能直接使用，还需要删除aar包中的一些内容
- libs路径下的classes.jar删除
- 打开外部的classes.jar删除内部的unity3d


## 5.打包测试

在接入SDK后，通过Unity打包和AndroidStudio打包都是可以的，但为了避免依赖的第三方库或系统库找不到，可以选择使用AndroidStudio打包（AndroidStudio可以自动下载缺少的内容）

如果在手机上安装的apk会有两个图标，则可通过打开对应的安卓工程，找到unityLibrary-manifests下的AndroidMainifest.xml,删除里面的即可
```xml
<category android:name="android.intent.category.LAUNCHER" />
```

