# 相机、射线及unity常用坐标系

## 一、相机组件

`1、ClearFlags：`清除标识， 决定屏幕的哪部分将被清除。当使用多个相机来描述不同的游戏场景对象时，利用它是非常方便的。有四个选项

- `SkyBox：` 天空盒，默认选项，一般用于3D游戏开发

- `Solid Color：` 显示的是背景色，主要用于2D

- `Depth Only：` 仅深度，可以调节相机深度，决定哪个相机照射的范围最先被渲染

- `Don’t Clear：` 不清除，该模式不清除任何颜色和深度缓存但这样做每帧渲染的结果都会叠加在下一帧之上。通常搭配shader使用

`2、Projection：` 投影模式，有两个选项

- `Perspective：` 透视相机，近大远小，有距离之分。广泛用于3D游戏的开发，层次分明，与Field of view搭配使用可以实现简单的FPS游戏中的狙击枪镜头效果

- `Orthographic：` 正交相机，没有近大远小和距离之分，常用于2D游戏开发，和size搭配能控制正交模式摄像机的视口大小。

`3、Clipping Planes：` 裁剪平面 ，用来控制摄像机的渲染范围，Near为最近的点，Far为最远的点

`4、Viewport Rect：` 视口矩形，用四个数值开控制摄像机的视图在屏幕中的位置和大小，数值在0~1之间；

- `X:` 水平位置起点

- `Y：` 竖直位置起点

- `W:` 宽度

- `H：` 高度

`5、Target Texture：` 目标纹理，包含相机视图输出，可以把某个摄像机的视野做成Render Texture，可以用来实现类似小地图的效果

`6、Occusion Culling：` 遮挡剔除

## 二、Unity常见坐标系

- `World Space：` 世界坐标系，是Unity中最常用的坐标系，它是一个三维坐标系，用于描述游戏场景中的物体位置和方向。世界坐标系的原点通常被定义为场景的中心点，坐标轴通常与场景的方向一致。在世界坐标系中，物体的位置和方向是相对于场景的整体而言的。

- `Local Space:` 局部坐标系,是相对于物体自身的坐标系，它以物体的中心点为原点，以物体的轴向为坐标轴。在局部坐标系中，物体的位置和方向是相对于物体自身而言的。当物体发生旋转、缩放等变换时，局部坐标系会相应地发生变化。

- `Screen Space:` 屏幕坐标系,是一个二维坐标系，用于描述屏幕上的位置。屏幕坐标系的原点通常位于屏幕的左下角，坐标轴的范围是从(0, 0)到(Screen.Width, Screen.Height)。在屏幕坐标系中，物体的位置和方向是相对于屏幕而言的。屏幕坐标系常用于处理鼠标点击、UI布局等与屏幕相关的操作。

- `Viewport Space:`  视口坐标系,是一个二维坐标系，用于描述相机的视口区域。视口坐标系的原点位于相机视口的左下角，坐标轴的范围是从(0, 0)到(1, 1)。在视口坐标系中，(0, 0)表示视口的左下角，(1, 1)表示视口的右上角。视口坐标系常用于处理与相机视口相关的操作，如屏幕后处理效果、裁剪等。

- `GUI Space:` GUI坐标系是一个二维坐标系，用于描述Unity的GUI元素的位置和大小。GUI坐标系的原点位于屏幕的左上角，坐标轴的范围是从(0, 0)到(Screen.width, Screen.height)。在GUI坐标系中，(0, 0)表示屏幕的左上角，(Screen.width, Screen.height)表示屏幕的右下角。GUI坐标系常用于处理UI布局和交互，如按钮、文本框等。

### 坐标系转换常用方法

1. `Transform.TransformPoint(Vector3 position)：`将一个点的位置从局部坐标系转换到世界坐标系。
2. `Transform.TransformVector(Vector3 direction)：`将一个向量从局部坐标系转换到世界坐标系。
3. `Transform.InverseTransformPoint(Vector3 position)：`将一个点的位置从世界坐标系转换到局部坐标系。
4. `Transform.InverseTransformVector(Vector3 direction)：`将一个向量从世界坐标系转换到局部坐标系。
5. `Transform.TransformDirection(Vector3 direction)：`将一个向量从局部坐标系转换到世界坐标系的方向。
6. `Transform.InverseTransformDirection(Vector3 direction)：`将一个向量从世界坐标系转换到局部坐标系的方向。
7. `Camera.WorldToScreenPoint(Vector3 position)：`将一个点的位置从世界坐标系转换到屏幕坐标系。
8. `Camera.ScreenToWorldPoint(Vector3 position)：`将一个点的位置从屏幕坐标系转换到世界坐标系。
9. `Camera.WorldToViewportPoint(Vector3 position)：`将一个点的位置从世界坐标系转换到视口坐标系。
10. `Camera.ViewportToWorldPoint(Vector3 position)：`将一个点的位置从视口坐标系转换到世界坐标系。
11. `Camera.ScreenToViewportPoint(Vector3 position)：`将一个点的位置从屏幕坐标系转换到视口坐标系。
12. `Camera.ViewportToScreenPoint(Vector3 position)：`将一个点的位置从视口坐标系转换到屏幕坐标系。

## 三、射线

在Unity中，射线（Ray）是由一个起点和一个方向组成的一条无限延伸的线段。射线可以用于进行射线投射、碰撞检测、物体拾取和路径寻找等操作。

射线在游戏开发中有很多用途，其中一些常见的用途包括：

1. 射线投射（Raycasting）：射线投射是一种检测射线与场景中物体之间的交点的方法。通过发射一条射线，可以检测射线与物体的碰撞，从而判断射线是否与物体相交、交点的位置和交点处的物体信息。射线投射常用于实现射击、点击检测、碰撞检测等功能。

2. 物体拾取（Object Picking）：射线可以用于检测射线与场景中的物体是否相交，从而实现物体的拾取。通过发射一条射线，可以判断射线与物体是否相交，并获取交点处的物体信息，从而实现物体的拾取、拖拽和放置等操作。

3. 路径寻找（Pathfinding）：射线可以用于进行路径寻找。通过发射一条射线，可以判断射线与场景中的障碍物是否相交，从而确定路径上的可行走区域。射线路径寻找常用于实现AI角色的移动和导航功能。

4. 碰撞检测（Collision Detection）：射线可以用于检测物体之间的碰撞。通过发射一条射线，可以判断射线与物体的碰撞，从而触发相应的碰撞事件或进行处理。射线碰撞检测常用于实现物体之间的互动和交互。

unity中使用Ray这个结构体来表示一个射线；（构建射线）origin 代表的是射线的原点； direction 代表的是射线的方向；

在unity中 射线的检测 需要使用Raycast函数（Physics），他可以从一个点出发，沿着方向发射一条射线，检测 射线是否与场景中的物体相交；如果相交，返回相交点的信息，eg：坐标、相交点的物体名称等。

案例1：从相机位置发射射线

     private void Update()
        {
            //检测是否按下鼠标左键
            if(Input.GetMouseButton(0))
            {
              //将鼠标在屏幕上点击的位置转换为一条从摄像机出发指向点击位置的射线并存储在
               变量ray中
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
              //通过Raycast方法判断这个ray射线是否与场景中的物体接触
                bool isObj = Physics.Raycast(ray);
                Debug.Log(isObj);
            }
        }

案例2：从脚本挂载对象的位置发射射线

    private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {    
               //声明一个RaycastHit类型的变量用来保存射线与物体的交点信息
                RaycastHit hitInfo;
               //从物体的位置发射一条向前的射线，并检测是否与其他物体相交，如果相交则把交点信息保存在hitInfo中
                if (Physics.Raycast(transform.position, transform.forward, out hitInfo))
                    //输出与射线相交的物体的名字
                      Debug.Log(hitInfo.transform.name);
                else
                    Debug.Log("什么都没碰到");
            }
        }

案例3：通过射线控制角色移动

    public class RayMove : MonoBehaviour 
    {
        Vector3 target;
        bool isMoveing = false;
        public float speed = 10f;
        void Update()
        {
            MoveByRay();
        }
        void MoveByRay()
        {
            if(Input.GetMouseButtonDown(0))
            {
              //将鼠标在屏幕上点击的位置转换为一条从摄像机发出指向点击位置的射线
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //创建一个RaycastHit类型的变量hitInfo来存储射线与交点的碰撞信息
                RaycastHit hitInfo;
                if(Physics.Raycast(ray,out hitInfo))
                {
                    if(hitInfo.transform.CompareTag("Ground"))
                    {
                        isMoveing = true;
                        target = hitInfo.point;
                    }
                }
            }
            if(isMoveing&&Vector3.Distance(target,transform.position)>0.15f)
            {
               //使物体面朝目标位置
                transform.LookAt(target);
               //使物体向着目标位置移动 
                transform.Translate(Vector3.forward * Time.deltaTime * speed);
            }
        }
    }

上面这段代码控制角色在地面行走时是没有问题的，但是如果在游戏场景中存在一个斜坡，这段代码由于没考虑斜坡倾斜的问题，所以在移动的时候只会直线移动到目标位置而，如下图

![](https://github.com/shishouheng/Unity-learning/blob/main/images/perpendicular%20to%20the%20slope.png)

这并不符合现实生活中的规律，当人站在斜坡上时身体仍然会保持竖直，并且在x轴方向的并不会有所旋转，正确的样子是这样的

![](https://github.com/shishouheng/Unity-learning/blob/main/images/after%20modification.png)

所以为了展示正确的在斜坡上的姿势，还需要对代码进行修改

    
