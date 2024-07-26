
Unity的静默模式主要用于自动化构建和测试，对于持续集成环境、自动化脚本和构建服务器提供了很大的便利

## 使用方式

需要在命令行中指定Unity路径以及项目路径，这样就能够以批处理的方式启动Unity，然后可以根据需要去指定目标平台、日志路径、执行方法等

- -batchmode:启用静默模式
- -projectPath<\path>:指定项目路径
- -excuteMethod<\ClassName.MethodName>:指定要执行的静态方法（必须是公开的静态方法，并且不能继承Mono）
- -buildTarget<\platform>:指定构建目标平台
- -logFile<\path>:指定日志文件路径


## 示例

假设我要通过批处理模式来对游戏进行打包，则可以通过如下方式来进行

C#代码：

```c#
using UnityEditor;

public class BuildScript
{
	//必须是公开的静态方法
	public static void PerformBuild()
	{
		string[]scenes={"Assets/Scenes/MainScene.unity"};
		string buildPath="Build/WindowsBuild/MyGame.exe";		     
		BuildPipeline.BuildPlayer(scenes,buildPath,BuildTarget.StandaloneWindows,
		BuildOptions.None);
		Debug.Log("Build completed successfully!");
	}
}
```

命令行调用：

```batch
/Applications/Unity/Hub/Editor/2020.3.12f1/Unity.app/Contents/MacOS/Unity \ 
-batchmode \ 
-projectPath /path/to/your/project \ 
-executeMethod BuildScript.PerformBuild \ 
-logFile /path/to/your/logfile.log \ 
-quit
```


如果方法需要参数，则可以在c#端来接收环境变量，然后通过命令行来设置环境变量并调用方法

c#代码
```c#
using UnityEditor; 
using System; 

public class BuildScript 
{ 
	public static void PerformBuild() 
	{ 
		string buildPath = Environment.GetEnvironmentVariable("BUILD_PATH"); 
		if (string.IsNullOrEmpty(buildPath)) 
			{ 
				buildPath = "Builds/DefaultBuild/MyGame.exe"; 
			} 
		string[] scenes = { "Assets/Scenes/MainScene.unity" }; 
		BuildPipeline.BuildPlayer(scenes,buildPath,BuildTarget.StandaloneWindows,
		BuildOptions.None);
		Debug.Log("Build completed successfully to " + buildPath); 
	} 
}
```

命令行调用：
```batch
export BUILD_PATH="Builds/WindowsBuild/MyGame.exe" 
/Applications/Unity/Hub/Editor/2020.3.12f1/Unity.app/Contents/MacOS/Unity \ 
-batchmode \ 
-projectPath /path/to/your/project \ 
-executeMethod BuildScript.PerformBuild \ 
-logFile /path/to/your/logfile.log \ 
-quit
```