## 一、EasyTouch
EasyTouch是Unity中的一个插件，它提供了一种简单而强大的方式来处理触摸输入和手势识别。它可以用于移动设备上的游戏开发，使开发者能够轻松地处理触摸屏幕上的各种手势操作。

EasyTouch插件具有以下特点和功能：

1. 多种手势支持：EasyTouch支持多种常用手势，如点击、双击、长按、滑动、拖动、缩放、旋转等。开发者可以通过简单的代码和事件回调来处理这些手势。
    
2. 多点触控支持：EasyTouch可以处理多点触控，允许玩家使用多个手指进行操作，例如在屏幕上同时进行拖动和缩放。
    
3. 自定义手势识别：除了内置的手势支持外，EasyTouch还提供了自定义手势的功能。开发者可以定义自己的手势，并通过代码来识别和处理这些手势。
    
4. UI支持：EasyTouch可以与Unity的UI系统无缝集成，使开发者能够处理UI元素上的手势操作，例如拖动滑块、缩放按钮等。
    
5. 编辑器支持：EasyTouch提供了一个可视化的编辑器界面，使开发者能够轻松配置和调整手势的参数和行为。
    


EasyTouch插件对所有触摸相关的事件进行了封装（如长按、双击、多点触摸、虚拟摇杆等），并通过观察者模式来观察对象在运行过程中所触发的事件，所有事件都通过EasyTouch对象的回调来接受

**注：通常会在OnEnable的时候注册事件并在OnDisable的时候取消事件绑定，这样做是为了避免在不需要使用触摸手势时造成不必要的性能消耗和错误的触摸操作**

案例：点击游戏物体改变他的颜色，抬起鼠标变回白色
```c#
using UnityEngine;
using HedgehogTeam.EasyTouch;

public class SetColor : MonoBehaviour
{
    private void OnEnable()
    {
        EasyTouch.On_TouchStart += EasyTouch_On_TouchStart;
        EasyTouch.On_TouchUp += EasyTouch_On_TouchUp;
    }

    private void EasyTouch_On_TouchUp(Gesture gesture)
    {
        if (gesture.pickedObject == gameObject)
            gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    private void EasyTouch_On_TouchStart(Gesture gesture)
    {
        //如果当前手势选中的对象是当前游戏物体，则变为红色
        if (gesture.pickedObject == gameObject)
            gameObject.GetComponent<Renderer>().material.color = Color.red;
    }
    private void OnDisable()
    {
        EasyTouch.On_TouchStart -= EasyTouch_On_TouchStart;
        EasyTouch.On_TouchUp -= EasyTouch_On_TouchUp;
    }
}
```


案例：实现拖拽游戏物体,松开鼠标回到原位
```c#
using UnityEngine;
using HedgehogTeam.EasyTouch;

public class DragTest : MonoBehaviour
{
    Vector3 startPos;
    Vector3 deltaDirection;

    private void OnEnable()
    {
        EasyTouch.On_DragStart += EasyTouch_On_DragStart;
        EasyTouch.On_Drag += EasyTouch_On_Drag;
        EasyTouch.On_DragEnd += EasyTouch_On_DragEnd;
    }

    private void Awake()
    {
        startPos = transform.position;
    }
    private void EasyTouch_On_DragEnd(Gesture gesture)
    {
        transform.position = startPos;
    }

    private void EasyTouch_On_Drag(Gesture gesture)
    {
        if(gesture.pickedObject==gameObject)
        {
            Vector3 point =gesture.GetTouchToWorldPoint(gesture.pickedObject.transform.position);
            transform.position = point - deltaDirection;
        }
    }

    private void EasyTouch_On_DragStart(Gesture gesture)
    {
        if(gesture.pickedObject==gameObject)
        {
            //将当前触摸位置从当前屏幕坐标转换为世界坐标，这个position3D参数是目标在世界坐标系中的位置，正因为如此，可以用来计算摄像机与目标物体的距离长度
            Vector3 point = gesture.GetTouchToWorldPoint(gesture.pickedObject.transform.position);
            //得到游戏物体中心到鼠标点击位置的偏移向量
            deltaDirection = point - transform.position;
        }
    }
    private void OnDisable()
    {
        EasyTouch.On_DragStart -= EasyTouch_On_DragStart;
        EasyTouch.On_Drag -= EasyTouch_On_Drag;
        EasyTouch.On_DragEnd -= EasyTouch_On_DragEnd;
    }
}
```

案例：通过Joystick摇杆控制角色移动
主要通过Joystick的回调事件实现，当检测到Joystick上的按钮移动时，调用角色的移动脚本。
需要将方法绑定到MoveEvents下的On Move（vector2）中实现
```c#
public class JoyStickMoveControl : MonoBehaviour
{
    CharacterController cc;
    private void Start()
    {
        cc = GetComponent<CharacterController>();
    }
    public void Move(Vector2 dir)
    {
        Vector3 direction = new Vector3(dir.x, 0, dir.y);
        direction = Camera.main.transform.TransformDirection(direction);
        direction.y = 0;
        transform.rotation = Quaternion.LookRotation(direction);
        cc.SimpleMove(direction);
    }

}
```
## 二、KGFMapSystem
小地图制作原理 
1 摄像机以正交模式 从人物正上方向下照射 并实时跟随人物移动 
2 使用Layer在Camera中剔除不需要在小地图中显示的游戏物体
3 在需要显示的物体下方增加子节点（实时获取物体的position） 保存在小地图中显示的信息 
4 如果实现放大和缩放功能 要调整Camera的正交大小