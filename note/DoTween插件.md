# 一、介绍
DOTween是一个快速、高效、完全类型安全的面向对象的动画引擎，专为Unity优化，面向C#用户，免费且开源，具有许多高级功能。

1 让游戏物体的变换和数据更新更加简单 
2 可以通过Tween来实现丰富的动画效果
3 控制整个Tween的过程 暂停，重播，倒播等 
4 实时获取到Tween的数据变化和回调事件 
5 支持的数据类型：float double int long vector2/3/4 Quaternion Rect RectOffSet Color string


# 二、功能模块
## 1、DoTween Animation

### 1.01 介绍：
用于实现各种类型的动画效果。提供了丰富的方法和功能，可以对物体的位置、大小、旋转、颜色、透明度等属性进行动画操作。通过设置起始值、目标值、持续时间、缓动函数等属性，可以实现平滑的过渡效果

常用方法： 

以DO开头的方法：补间动画的方法。例如：Transform.DOMoveX(10,1) 

以Set开头的方法：设置补间动画的属性。例如：Tweener.SetLoops(4, LoopType.Yoyo) 

以On开头的方法：补间动画的回调函数。例如：Tweener.OnStart(callBackFunction) 

### 1.02 DO（设置动画）：
```c#
void TestFunction()
    {
        //5秒内 局部坐标从自身位置 增加10 10 10个单位
        transform.DOBlendableLocalMoveBy(new Vector3(10, 10, 10), 5);
        //5秒内 局部旋转从自身旋转 增加30 30 30 个单位
        transform.DOBlendableLocalRotateBy(new Vector3(30, 30, 30),5);
        //5秒内 世界坐标从自身位置 增加10 10 10 个单位
         transform.DOBlendableMoveBy(new Vector3(10, 10, 10), 5);
        //5秒内 世界旋转从自身旋转 增加10 10 10 个单位
        transform.DOBlendableRotateBy(new Vector3(10, 10, 10), 5);
        //5秒内 缩放值从自身缩放 增加3 3 3 个单位
        transform.DOBlendableScaleBy(new Vector3(3, 3, 3), 5);
        //变化立即结束并完成运动
    }
```

### 1.03 Set（设置属性）:
```c#
SetAutoKill //设置是否自动销毁 
SetDelay //设置延迟 
SetEase // 
SetId //object类型的值标签 
SetRecyclable //设置为可回收 可循环利用 
SetRelative //设置相对变换 
SetTarget //设置目标 
SetUpdate Update更新模式 //设置是否收到unity的时间影响 TimeScale
SetLookAt //在运动过程中始终面向移动方向
```

### 1.04 On（回调函数）：
```c#
 OnComplete(TweenCallback callback)//当补间动画完成时调用的回调函数
 OnKill(TweenCallback callback)//当补间动画被杀死时调用的回调函数
 OnPlay(TweenCallback callback)//当补间动画开始播放时调用的回调函数，暂停后重新播放也会调用
 OnPause(TweenCallback callback)//当补间动画暂停时调用的回调函数。
 OnStart(TweenCallback callback)//当补间动画开始时调用的回调函数。
 OnUpdate(TweenCallback callback)//每一帧都会调用的回调函数。
 OnRewind(TweenCallback callback)//当补间动画倒带时调用的回调函数。
```



### 1.05 transform类

```c#
using UnityEngine;
using DG.Tweening;

public class DoTest : MonoBehaviour
{
    private void Start()
    {
        TestFunction();
    }

    void TestFunction()
    {
        //5秒内 局部坐标从自身位置 增加10 10 10个单位
        transform.DOBlendableLocalMoveBy(new Vector3(10, 10, 10), 5);
        //5秒内 局部旋转从自身旋转 增加30 30 30 个单位
        transform.DOBlendableLocalRotateBy(new Vector3(30, 30, 30),5);
        //5秒内 世界坐标从自身位置 增加10 10 10 个单位
         transform.DOBlendableMoveBy(new Vector3(10, 10, 10), 5);
        //5秒内 世界旋转从自身旋转 增加10 10 10 个单位
        transform.DOBlendableRotateBy(new Vector3(10, 10, 10), 5);
        //5秒内 缩放值从自身缩放 增加3 3 3 个单位
        transform.DOBlendableScaleBy(new Vector3(3, 3, 3), 5);
        //变化立即结束并完成运动
        transform.DOComplete();
        Invoke("Flip", 3.0f);
        //直接得到动作第3秒的状态
        transform.DOGoto(3);
        //直接停止tween 并删除tween对象
        transform.DOKill(true);
        //10秒 弹跳3次 跳跃到0 10 0的坐标
        transform.DOJump(new Vector3(0, 0, 0), 5, 1,2);
        //3秒 本地坐标移动到5 5 5
        transform.DOLocalMove(new Vector3(5, 5, 5), 3);
        transform.DOLocalMoveX(6, 3);
        transform.DOLocalMoveY(6, 3);
        transform.DOLocalMoveZ(6, 3);
        //3秒 世界坐标移动到10 10 10
        transform.DOMove(new Vector3(10, 10, 10), 3);
        transform.DORotate(new Vector3(50, 50, 50), 3);
         transform.DOScale(new Vector3(5, 5, 5), 3);
        transform.DOPause();//暂停
        transform.DOPlay();//播放或者继续
         transform.DORestart();//在变化结束之前 重新变化
        transform.DORewind();//在变化结束之前 回归原始
        transform.DOTogglePause();//暂停开关
        transform.DOPlayBackwards();//倒播
        transform.DOPlayForward();//正播
        //弹簧效果
        transform.DOPunchPosition(new Vector3(10, 10, 10), 5);
        transform.DOPunchRotation(new Vector3(50, 50, 50), 5);
        transform.DOPunchScale(new Vector3(5, 5, 5), 5);
        //震动
        //10秒 在-5 5之间震动
        transform.DOShakePosition(10, new Vector3(10, 10, 10));
        transform.DOShakeRotation(10, new Vector3(10, 10, 10));
        transform.DOShakeScale(10, new Vector3(10, 10, 10));
        transform.DOMove(new Vector3(0, 5, 0), 2).From();
        transform.DOMove(new Vector3(0, 5, 0), 2).SetRelative();
        transform.DOBlendableMoveBy(new Vector3(1, 1, 1), 2);
        transform.DOBlendableMoveBy(new Vector3(-1, 0, 0), 2);
    }
}
```

### 1.06 Camera类
```c#
void CameraDoTween()
    {
        //调整相机宽高比
        Camera.main.DOAspect(0.6f, 2);
        //调整相机背景色
        Camera.main.DOColor(Color.red, 2f);
        //调整相机近平面和远平面
        Camera.main.DONearClipPlane(200, 2);
        Camera.main.DOFarClipPlane(200, 2);
        //调整相机的FOV大小
        Camera.main.DOFieldOfView(30, 2);
        //调整相机正交大小
        Camera.main.DOOrthoSize(2000, 2);
        //相机震动
        Camera.main.DOShakePosition(1, 10, 10, 50);
        //根据屏幕像素进行显示范围调整
        Camera.main.DOPixelRect(new Rect(0f, 0f, 600f, 500f), 2);
        //根据屏幕百分比进行显示范围调整
        Camera.main.DORect(new Rect(0.5f, 0.5f, 0.5f, 0.5f), 2);
    }
```

### 1.07 Material类
```c#
 void MaterialDoTween()
    {
        Material material = GetComponent<Renderer>().material;
        material.DOColor(Color.red, 2f);
        material.DOColor(Color.red, "MainColor",2f);
        //调整Shader Alpha
        material.DOFade(0, 2);
        //调整贴图的UV坐标
        material.DOOffset(new Vector2(1, 1), 2);
        material.DOVector(new Vector4(0, 0, 0, 1), "sss", 2);
        material.DOBlendableColor(Color.red, 3);
        material.DOBlendableColor(Color.gray, 3);
    }
```

### 1.08 Text类
```c#
void MaterialDoTween() 
{ 
 uiText.DOColor(Color.black, 2); 
 uiText.DOFade(0, 2); 
 uiText.DOText("你好啊，我是你的好朋友", 5); }
```


### 1.09 Tween对象

在DOTween中，Tween对象是用于实现动画效果的核心对象。Tween对象表示一个动画过程，可以控制目标对象的属性从一个值过渡到另一个值，实现平滑的动画效果。

Tween对象有多种类型，常用的包括：

1. Tweener：用于控制一个属性的过渡动画，例如移动位置、改变颜色等。可以通过调用目标对象的属性来设置起始值和目标值，然后通过调用`Play`方法来开始动画。
    
2. Sequence：用于控制多个Tween对象按顺序播放。可以通过调用`Append`方法将Tween对象添加到Sequence中，然后通过调用`Play`方法来开始播放。
    
3. DOTweenAnimation：是一个组件，可以直接添加到游戏对象上，用于快速创建常见的动画效果。通过在Inspector面板中设置参数，可以实现移动、旋转、缩放等动画效果。

### 1.10 Tween协程
```c#
 private IEnumerator Wait()
    {
        Tweener _tweener = trans1.DOMove(Vector3.one, 2);
        yield return _tweener.WaitForStart();
        Debug.Log("OnStart");
        yield return _tweener.WaitForPosition(1);
        Debug.Log("1秒时间到");
        yield return _tweener.WaitForCompletion();
        Debug.Log("Move完成");
        _tweener = trans1.DORotate(Vector3.up * 90f, 2).SetLoops(5,LoopType.Restart).SetEase(Ease.Linear);
        yield return _tweener.WaitForElapsedLoops(2);
        Debug.Log("循环两次旋转之后");
        _tweener.Pause();
        yield return new WaitForSeconds(1f);
        _tweener.Play();
        _tweener.Flip();
        yield return _tweener.WaitForRewind();
        Debug.Log("翻转完成");
        _tweener.Kill();
        yield return _tweener.WaitForKill();
        Debug.Log("Kill");
    }
```
### 1.11 Sequence序列

在DOTween中，Sequence是一种特殊的补间动画，它不是控制一个值，而是控制其他补间动画并将它们作为一个组进行动画处理。可以使用`DOTween.Sequence()`来创建一个序列，并使用`Append`方法将多个补间动画添加到序列中。这些补间动画将按照添加的顺序依次播放。此外，您还可以使用`Insert`方法来在指定时间插入补间动画，以便在序列播放过程中并行播放多个补间动画。

Sequence有几个常用的方法：

- `Append(Tween tween)`：在Sequence的最后添加一个tween。
- `AppendCallback(TweenCallback callback)`：在Sequence的最后添加一个回调函数。
- `AppendInterval(float interval)`：在Sequence的最后添加一段时间间隔。
- `Insert(float atPosition, Tween tween)`：在给定的时间位置上放置一个tween，可以实现同时播放多个tween的效果，而不是一个接一个播放。
- `InsertCallback(float atPosition, TweenCallback callback)`：在给定的时间位置上放置一个回调函数。
- `Join(Tween tween)`：在Sequence的最后一个tween的开始处放置一个tween。可以实现同时播放多个tween的效果，而不是一个接一个播放。
- `Prepend(Tween tween)`：在Sequence开始处插入一个tween，原先的内容根据时间往后移。
- `PrependCallback(TweenCallback callback)`：在Sequence开始处插入一个回调函数。
- `PrependInterval(float interval)`：在Sequence开始处插入一段时间间隔，原先的内容根据时间往后移。




### 1.12缓动函数
![[Pasted image 20230829223232.png]]
![](https://github.com/shishouheng/Unity-learning/blob/main/images/%E7%BC%93%E5%8A%A8%E5%87%BD%E6%95%B0.png)

## 2、DoTween Path
DOTweenPath是DOTween的另一个组件，允许在Unity中创建和编辑路径动画。可以使用它来定义一个物体沿着一条路径移动的动画。

要使用DOTweenPath，首先需要在Hierarchy面板中创建一个空的游戏对象，并将DOTweenPath组件添加到该游戏对象上。然后可以在Scene视图中编辑路径。

在Inspector面板中，可以设置DOTweenPath的各种参数。如设置动画的持续时间、延迟时间、缓动类型等。此外，还可以设置路径类型（线性或曲线）、是否封闭路径、物体朝向等。

除了基本的路径动画设置外，DOTweenPath还提供了一些高级功能。如可以使用`OnWaypointChange`事件来在物体到达特定路径点时执行特定操作。还可以使用`DOPause`、`DOPlay`、`DORestart`等方法来控制动画的播放。

案例：多个物体共用一条DoTweenPath路径移动
```c#
public class FindPath : MonoBehaviour
{
    private DOTweenPath path;
    public GameObject[] prefabs;
    void Start()
    {
        path = GetComponent<DOTweenPath>();
        path.GetTween().SetAutoKill(false);//取消自动销毁
        InvokeRepeating("CreateEnemy", 1.0f, 4.0f);
    }
    private void CreateEnemy()
    {
        int index = Random.Range(0, prefabs.Length);
        GameObject obj = Instantiate(prefabs[index],
       transform.position, Quaternion.identity);
        obj.GetComponent<Enemy>().SetPath(path);
    }
```
```c#
public class Enemy : MonoBehaviour
{
    private DOTweenPath path;
    public float speed;
    void Start()
    {
    }
    public void SetPath(DOTweenPath path)
    {
        this.path = path;
        transform.DOPath(path.wps.ToArray(),
       path.GetTween().PathLength() / speed, PathType.CatmullRom)
        .OnWaypointChange(SetLookAt).OnComplete(OnComplete);
    }
    private void SetLookAt(int index)
    {
        transform.DOLookAt(path.wps[index], 0.1f);
    }
    private void OnComplete()
    {
        Destroy(gameObject);
    }
}
```