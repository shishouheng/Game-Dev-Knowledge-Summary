# UI系统之UGUI

UGUI是unity官方的UI实现方式，相比于OnGUI系统更加人性化，而且是一个开源系统，利用游戏开发人员进行界面开发，具有灵活、快速、可视化等优点。

## 一、UGUI的创建及属性介绍

UGUI的创建可以通过在Hierarchy面板右键====》UI====》选择你需要的UI控件来进行创建。当创建好一个UI控件时，Unity会自动为我们创建一个Canvas（画布）和一个EventSystem（事件系统）

- **Canvas：** 画布是摆放所有UI元素的区域，在场景中创建的所有UI控件都会自动变为Canvas的子对象。若场景中没有画布，在创建控件时都会自动创建画布

- **EventSystem：** 在创建控件生成Canvas的同时也会生成一个EventSystem，用来承接事件，挂载了若干个与事件监听相关的组件可供设置

### 

### 1、1  RectTransform组件介绍

所有创建的UI控件都携带一个默认的组件——RectTransform，用来表示UI控件的位置、旋转和缩放信息，类似于在3D场景中创建一个对象必然会携带Transform组件，它们都是用来存储对象的位置、旋转和缩放信息的。但RectTransform是专门用于UI元素的，RectTransform类继承于Transform类，并在Transform类的基础上添加了额外的功能，如锚点、中心点、位置和大小等，专门用于2D布局。

### 2、2  RectTransform属性介绍

![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/recttransform%20attribute.png)

**Anchors(锚点）：** 定义了当前控件在其父物体中的位置

由两个二维向量Min和Max组成，这两个二维向量以当前控件的父物体的左下角为

（0，0），右上角为（1，1），当Min与Max一致时，四个锚点集中在一起，Min与Max不一致时，四个锚点分开

![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/Anchor.jpg)

**Pivot(轴心）：** 定义了当前控件的旋转和缩放中心

其取值也在[0,1]之间，[0,0]表示当前控件的左下角，[1,1]表示当前控件的右上角

**Anchor PreSet：** 锚点预设

按住Alt键点击指定的锚点预设时锚点会修改到对应位置并且控件也移动到相应位置

![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/archor%20preset.jpg)

## 二、Canvas属性介绍

![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/Canvas.png)

**1、Render Mode:** 渲染模式

![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/render%20mode.jpg)

- Screen Space Overlay:屏幕空间覆盖模式，画布会填满整个游戏运行窗口并将画布下的所有UI都置于屏幕最上层，如果屏幕尺寸发射改变，画布会自动改变尺寸来匹配屏幕

- Screen Space Camera：相机模式，画布填满整个游戏运行窗口，如果屏幕尺寸发生改变，画布会自动改变尺寸来匹配屏幕，该模式下，画布会被放在相机前方
  
  ![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/screen%20space%20camera.jpg)

- Word Space：完全3D的UI，常在VR开发中使用，存在近大远小的效果，也可以用来实现跟随人物移动的血条、名称的效果
  
  ![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/world%20space.jpg)

**2、CanvasScaler：** 画布缩放（UI自适应）

![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/canvas%20scaler.png)

UIScaleMode:UI缩放模式

![](https://github.com/shishouheng/Unity-learning/blob/main/images/UGUI/UI%20scale%20mode.jpg)

- ContentPixelSize:无论屏幕大小如何，UI元素都保持相同的像素大小

- Scale With ScreenSize：屏幕越大，UI元素越大，常用来做自适应

- Contant PhysicalSize：无论屏幕大小和分辨率如何，UI元素都保持相同的物理大小

案例：通过代码利用ContentPixelSize实现不同分辨率设备的适配（主要考虑宽高，并按照自身指定的权重值进行缩放）

```c#
        using System.Collections;
		using System.Collections.Generic;
		using UnityEngine;
		using UnityEngine.UI;		
		public class UIAdjust : MonoBehaviour
		{
		    private CanvasScaler scaler;
		    public float x;
		    public float ScreenWidth;
		    public float ScreenHeight;
		    private void Awake()
		    {
		        scaler = GetComponent<CanvasScaler>();
		    }
		    void Update()
		    {
		        float w = Screen.width;
		        float h = Screen.height;
		        w = w / ScreenWidth;
		        h = h / ScreenHeight;
		        scaler.scaleFactor = w * x + h * (1 - x);
		    }
		}
```


Scale with ScreenSize：自适应
