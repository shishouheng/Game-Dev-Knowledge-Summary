# UGUI事件注册

## 一、Graphic Raycaster（发射射线）

Graphic Raycaster是挂载Canvas下用来检测UI输入事件的射线发射器。Unity中的射线发射器除了Graphic Raycaster还有Physics Raycaster和Physics 2D Raycaster。

当Canvas挂载了Graphic Raycaster组件时，该组件会将所有的UI元素（如按钮、文本、图片等）转换为可被射线检测到的图形对象，当鼠标或触摸屏幕有交互操作发生时，Graphic Raycaster会通过射线与这些图形对象进行碰撞检测，找到被点击的UI元素

Graphic Raycaster属性介绍：

- Ignore Reversed Graphics：是否忽略翻转的UI元素

- Blocked Objects：阻挡射线的物体类型，可以选择2D或者3D物体或者选择none，即两者都可以阻挡

- Blocking Mask：能够阻挡射线的物体的层级筛选。同时满足Blocked Objects和Blocking Mask的物体才会阻挡射线

## 二、EventSystem（接收射线）

在通过Graphic Raycaster发射射线后，由EventSystem负责处理接收到的射线并触发相应的事件。EventSystem是UGUI事件系统的核心，负责管理所有的输入模块以及处理所有的事件。EventSystem组件会在每一帧中检测输入事件，并将其传递给对应的UI元素来响应用户操作

## 三、通过Raycast Target实现可视化控件检测用户输入

在可交互控件中，如Button在用户鼠标移动过去、点击时都会触发事件，而对于可视化控件如Image、Text鼠标点击时并没有任何反应，那可视化控件能否像可交互控件一样获取到用户的输入呢？

可视化控件身上会有一个Raycast Target组件，当勾选时代表该控件可以被射线检测到，然后我们就可以通过代码为可视化控件添加事件

    void Update ()
        {
            if(Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current.IsPointerOverGameObject())
                    Debug.Log("点击到可视化控件上了");
                else
                    Debug.Log("没有点击到可视化控件上");
            }
        }

此时如果Image或者Text或者其他可视化控件勾选了Raycast Target，当鼠标点击控件时控制台会输出“点击到可视化控件上了”，也就意味着可视化控件像可交互控件一样获取到了用户的输入

## 四、UGUI事件注册总结

主要有四种方式：

**1、通过编辑器拖拽添加事件：**

**2、代码添加**，通过AddListener

但这两种方式都具有一定的局限性，第一种方式只能给可交互控件添加事件（因为只有可交互控件才有事件的列表），同时这两种方式只有在按下鼠标（button）、值发生改变（toggle）等情况下才能添加事件，具有一定的局限性。因此还有两种更灵活的方式来实现事件的注册

**3、实现接口** 来进行射线检测，这是一种比较完美的一种解决方式

**3、自定义框架**

常用的UGUI事件处理接口：

- **IPointerEnterHandler：** 当鼠标进入UI元素时触发的事件

- **IPointerExitHandler：** 当鼠标从UI元素中移开时触发的事件

- **IPointerDownHandler：** 当鼠标按下时触发的事件

- **IPointerUpHandler:** 当鼠标抬起时触发的事件

- **IPointerClickHandler：** 当鼠标点击时触发的事件

- **IDragHandler：** 当鼠标拖拽UI元素时触发的事件

- **IBeginDragHandler：** 当鼠标开始拖拽UI元素时触发的事件

- **IEndDragHandler：** 当鼠标结束拖拽UI元素时触发的事件

- **IScrollHandler：** 当鼠标滚动或触摸滑动时触发的事件

- **ISelectHandler：** 当UI元素被选中时触发的事件

案例 实现鼠标拖拽UI元素

    using UnityEngine;
    using UnityEngine.EventSystems;
    
    public class InterfaceTest : MonoBehaviour, IBeginDragHandler, IDragHandler
    {
        Vector2 offest;
        public void OnBeginDrag(PointerEventData eventData)
        {
            offest = (Vector2)transform.position - eventData.position;
        }
    
        public void OnDrag(PointerEventData eventData)
        {
            transform.SetAsLastSibling();
            transform.position = eventData.position + offest;
        }
    }
