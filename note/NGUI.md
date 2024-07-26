# NGUI

## 一、三大基础组件

### 1、1 UIRoot

主要是制作屏幕适配时使用，用于控制UI的缩放模式，功能类似于UGUI中的Canvas

ScalingStyle：缩放模式，有以下三种选择

- Flexible：自由缩放，一般适用于PC端

- Constrained：限制缩放，一般用于移动端

- Constrained On Mobiles：以上两种的综合，自动判断运行平台

实现屏幕适配的步骤，选择Constrained On Mobiles模式，并输入当前参考分辨率，然后将以下代码挂载到UICamera上即可实现屏幕适配
```c#
 public class CameraAdjust : MonoBehaviour
    {
        //设备分辨率的宽高
        private float device_Width;
        private float device_Height;
        //参考分辨率的宽高
        public float standard_Width = 320;
        public float standard_Height = 480;
        private Camera cam;
        private void Awake()
        {
            device_Width = Screen.width;
            device_Height = Screen.height;
            cam = GetComponent<Camera>();
        }
        private void Start()
        {
            SetSize();
        }
        void SetSize()
        {
            float device_wh = device_Width / device_Height;
            float standard_wh = standard_Width / standard_Height;
            if (device_wh < standard_wh)
            {
                cam.orthographicSize = standard_wh / device_wh;
            }
        }
    }
```

### 1、2 UIPanel

UI界面的管理组件，控制UI面板的显示效果如透明度、层级等，功能类似于UGUI中的Panel

Alpha：控制所有子物体的透明度

Depth：控制所有子物体的深度，深度越高越先被渲染

### 1、3 UICamera

负责监听UI模块的事件，如点击事件、拖拽事件；功能类似于UGUI中的EventSystem

Event Type 一般选择2DUI；事件的监听顺序和Depth相关；
如果UICamera组件挂在主摄像机上，设置成3DUI ；事件监听和到相机的距离相关；
Event Mask用来决定哪些层会接收事件；
Debug 选项用来显示当前在鼠标下面的是什么；
Allow Multi Touch 选项用来控制是否支持多点触碰；如果勾选掉，多点触碰也会当做单点
触碰；
Sticky Tooltip 选项用来微调tooltip的行为 如果关掉，当鼠标再次移动的时候就会立即关
掉tooltip；
如果打开，只要鼠标一直在这个game object上 tooltip就会移至显示 ；
Tooltip Delay用来控制当鼠标停在某个object上时， 经过多长时间OnTooltip消息会被发
送到这 个object上， 以秒为单位；
Raycast Range 控制raycast的长度; 大多数情况下这个值可以被忽略. 这个值是世界坐标
系的值;
Event Sources 用来确定哪些事件类型会被处理, 被勾选掉的事件就不会被处理。 有些平
台会强制关闭一些事件。 比如使用手柄时会自动关掉鼠标和touch事件;
Thresholds 可以调整click、drag和tap事件的阈值 来微调鼠标和touch事件的行为以像
素为 单位 ；
Axes和Keys 部分用来控制哪个轴控制哪个方向的移动,这些名字需要和Input Manager里
面的一致

### 1、4 NGUI的事件函数

UICamera发送以下事件给collider：
OnHover (isOver)：发送时机为鼠标悬停（只触发一次）或者离开collider ；
OnPress (isDown)：发送时机为鼠标在collider上按下；
OnSelect (selected) 发送时机为鼠标点击和松开的时候都在同一个object上 ；
OnClick () 发送时机和OnSelect一样，但是要求鼠标没有移动特别多；
UICamera.currentTouchID表示按下的鼠标哪个键；
OnDoubleClick () 发送时机为当在四分之一秒内click两次的时候；
UICamera.currentTouchID表示按下的鼠标哪个键。
OnDragStart () 发送时机为OnDrag()事件之前 ；
OnDrag (delta) 发送时机为一个object被拖拽；
OnDragOver (draggedObject) 发送时机为其他的object拖拽到他的上面； 
OnDragOut (draggedObject) 发送时机为其他的object拖拽出他的上面 ；
OnDragEnd () 发送时机为drag事件结束。 发送给被拖拽的object；
OnInput (text) 发送时机为输入的时候（在点击选择了一个collider之后） ；
OnTooltip (show) 发送时机为鼠标悬停在一个collider上一段时间没有移动；
OnScroll (float delta) 发送时机为鼠标滚轮滚动。
OnKey (KeyCode key) 发送时机为键盘或者输入控制器被使用的时候。

## 二、NGUI基础控件

### 2、1  UITexture

作用类似于UGUI中的RawImage组件，可以用来显示一个纹理，如一张图、一个视频等

### 2、2  UISprite

类似于UGUI中的Image，用于显示精灵图，但与UGUI不同的是在NGUI中需要先将打包到Atlas（图集）中才可以使用。使用Atlas是为了将多张小的贴图合成一张大的贴图从而减少DrawCall（CPU向GPU发送一次渲染请求就叫一次DrawCall）次数，从而提高渲染效率。

因此在开发的过程中应尽量避免使用UITexture而尽可能的用UISprite代替

可以通过这种方式查看DrawCall次数

![](https://github.com/shishouheng/Unity-learning/blob/main/images/NGUI/open%20draw%20call%20tool.png)

### 2、3  UILabel

类似于UGUI中的Text

Font Size: 字体大小
Text：显示内容
Overflow：填充内容选项 当label的text超过允许长度的时候应该怎么处理
1，ShrinkContent（以内容为基准进行填充） 会自动被缩小以便适应区域
2，ClampContent(以字体为基准进行剪切） 文本没有适应，就直接截断
3，ResizeFreely label的大小通过文本来控制 不能自己修改大小
4，Resizeheight 必要的时候增加label的高度，宽度不变
Alignment：字体的对齐方式
Gradient：字体渐变
Effect：字体效果
shadow 阴影
outline 外边框 阴影会增加双倍几何，而外边框则是5倍
Spaceing：字体间隔
Max Lines：多少行来显示字体 0的话是不限制

### 2、4  UIButton

类似于UGUI中的Button，同样可以通过编辑器拖拽或者代码两种方式添加回调函数

```c#
using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class NGUITest : MonoBehaviour
    {
     public UIButton uiBtn;
     void Start()
     {
     if (uiBtn != null)
     {
     UIEventListener.Get (uiBtn.gameObject).onClick +=
    OnClickBtn3;
     UIEventListener.Get (uiBtn.gameObject).onClick += (go) => {
     Debug.Log ("点击了Play按钮4"+UIEventListener.Get (uiBtn.gameObject).onDoubleClick +=
    (go) => {
     Debug.Log("双机");
     };
     }
     }
     public void OnClickBtn1()
     {
     Debug.Log ("点击了Play按钮1");
     }
     public void OnClickBtn2()
     {
     Debug.Log ("点击了Play按钮2");
     }
     public void OnClickBtn3(GameObject go)
     {
     Debug.Log ("点击了Play按钮3"+go.name);
     }
    }go.name);
     };
```

### 2、5 UIToggle

一个Toggle由BackGround、CheckMark和Label组成，并且需要添加一个触发器

State of ‘None’： 主要用于Group，是否允许你当前为空；
StartingState： 当前Toggle开始的状态；

## 三、实现RPG背包

主要通过继承NGUI提供的UIDragDropItem类来实现，在NGUI提供的UIDragDropItem类中为我们提供了OnDragDropStart（开始拖拽）、OnDragDropMove（拖拽过程中）OnDragDropRelease（拖拽释放）和OnDragDropEnd（拖拽结束），这几个时机的虚方法，通过重写这些虚方法可以实现RPG背包道具的拖拽效果

```c#
 public class DragDropItem : UIDragDropItem
    {
        UISprite sprite;
        int startDepth;
        protected override void Awake()
        {
            base.Awake();
            sprite = GetComponent<UISprite>();
        }
        protected override void OnDragDropStart()
        {
            base.OnDragDropStart();
            startDepth = sprite.depth;
            sprite.depth = 100;
        }
        protected override void OnDragDropRelease(GameObject surface)
        {
            base.OnDragDropRelease(surface);
            sprite.depth = startDepth;
            if (surface.CompareTag("Slot"))
            {
                transform.SetParent(surface.transform);
                transform.localPosition = Vector3.zero;
            }
            else if(surface.CompareTag("Item"))
            {
                Transform temp = transform.parent;
                transform.SetParent(surface.transform.parent);
                transform.localPosition = Vector3.zero;
                surface.transform.SetParent(temp);
                surface.transform.localPosition = Vector3.zero;
            }
            else
            {
                transform.localPosition = Vector3.zero;
            }
        }
    }
```

## NGUI动画系统

NGUI为我们提供了Tween Position、Tween Rotation、Tween Color等等很多UI的动画脚本，借助这些脚本可以实现许多UI的动画效果

```c#
  public class NGUITest : MonoBehaviour
    {
     //public TweenPosition startPanelTP;
     //public TweenPosition settingPanelTP;
     public TweenScale startPanelTP;
     public TweenScale settingPanelTP;
     public void OnClickSetBtn()
     {
     startPanelTP.PlayForward ();
     settingPanelTP.PlayForward ();
     }
     public void OnClickCloseBtn()
     {
     startPanelTP.PlayReverse ();
     settingPanelTP.PlayReverse（）；
     }
    public void OnFinishedStart()
     {
     Debug.Log ("Start播放完毕");
     }
    }
```
