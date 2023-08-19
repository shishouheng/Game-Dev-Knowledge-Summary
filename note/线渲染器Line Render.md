# 线渲染器Line Render

定义：线渲染器是unity中的一个组件，可以使用3D空间中两个或多个点的数组，在每个点之间绘制一条直线。因此，单个线渲染器组件可用于绘制从简单到复杂螺旋线的任何线条。这条线始终是连续的；如果需要绘制两条或更多完全独立的线，则应使用多个游戏对象，每个游戏对象都要有自己的线渲染器。

用途：可以用来制作一些特效，如激光、电弧、路径等

常用属性简介：

- **Materials**：用于渲染线条的材质数组。对于数组中的每种材质，该线将被绘制一次。
- **Positions**：要连接的 Vector3 点的数组。
- **Width**：定义宽度值和曲线值以控制线沿其长度的宽度。曲线是在每个顶点处采样的，因此其精度受制于线中的顶点数量。线的总宽度由宽度值控制。
- **Color**：定义一个渐变来控制线沿其长度的颜色。Unity 在每个顶点处从颜色渐变 (Color Gradient) 中采样颜色。在每个顶点之间，Unity 对颜色应用线性插值。
- **Loop**：启用此属性可连接线的第一个和最后一个位置并形成一个闭环。
- **Alignment**：设置线面向的方向。View 线面向摄像机。TransformZ 线朝向其变换组件的 Z 轴。

## 案例1：按下鼠标左键实现单条线段的绘制

分析：线渲染器绘制线段的原理是将在空间中的点连接成线段，所以我们要将鼠标按下时鼠标每帧在屏幕上的位置转换为世界坐标后作为参数传入LineRender的Positions的数组中，然后LineRender就会自动将这些点连接起来。

为实现这个效果还需要先将相机设置为正交相机，即将相机的Projection设置为Orthographic，然后设置Clear Flags为Solid Color（纯色），以便更好的观察到绘制的线段

```c#
  public class DrawOneLine : MonoBehaviour
    {
        private LineRenderer lineRenderer;
        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        void Start()
        {
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth =0.2f;
        }
        void Update()
        {
            DrawLine();
        }
        void DrawLine()
        {
            if(Input.GetMouseButton(0))
            {
                Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                lineRenderer.positionCount++;
                lineRenderer.SetPosition(lineRenderer.positionCount-1, mouseWorldPosition);
            }
        }
    }
```

**注：在Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));这段代码中不直接将Input.position作为参数传入的原因是Input.Position是鼠标在屏幕上的位置信息，它的z轴默认是相机的位置，而由于Vector3中的x、y、z都是浮点数，会上下浮动，如果某一时刻Input.Position.z的值浮动到小于相机的z时，相机就无法观测到，所以最好自己设置这个点的z值，只要保证大于相机的z值即可**

此时即可实现如下效果

![](https://github.com/shishouheng/Unity-learning/blob/main/images/DrawOneLine.png)

## 案例2：每按下一次鼠标绘制一条线段

分析：在案例1中实现了按下鼠标绘制一条线段，当按下鼠标绘制完一条线段松手后再次点击，会将这次点击的位置与松手前的的位置连接起来，这是因为他们本质上还都是同一条LineRender。

所以如果要实现每按下一次鼠标绘制一条线段可以通过Input.GetMouseButtonDown方法在每按下鼠标时实例化一个LineRender，然后再通过InPut.GetMouseButton,在鼠标持续按下时为每一条实例化的LineRender添加点。

因此可以将一个添加了线渲染器的空物体设置为预制体，在每次鼠标按下时实例化一个该物体，并在鼠标持续按下时将鼠标的position作为参数传入当前LineRender的Positions中。

同时为了增加代码的扩展性，减少代码之间的耦合性，可以将加点的代码放在预制体身上，将实例化预制体和获取鼠标世界坐标的代码挂载到场景中某个物体身上，这样在后续有其他需求时可以直接在预制体的代码中进行修改，然后再实例化的代码中直接调用即可，减少了耦合性

```c#
//预制体身上代码
    public class Line : MonoBehaviour 
    {
        private LineRenderer lineRenderer;
        List<Vector3> points = new List<Vector3>();
        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        public void DrawLines(Vector3 pos)
        {
            points.Add(pos);
            lineRenderer.positionCount = points.Count;
            for(int i=0;i<points.Count;i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
    }
    
    //将获取的鼠标位置信息直接传入Line中的DrawLines方法即可
    public class DrawLines : MonoBehaviour
    {
        public GameObject mLine;
        private GameObject mIns_Line;
        void Update()
        {
            DrawMultLines();
        }
        void DrawMultLines()
        {
            if (Input.GetMouseButtonDown(0))
                mIns_Line = Instantiate(mLine);
            if(Input.GetMouseButton(0))
            {
                Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 1));
                mIns_Line.GetComponent<Line>().DrawLines(pos);
            }
        }
    }
```

假如后续又需要对绘制的线条的起始和结束宽度进行设置，可以在Line类中添加一个SetWidth方法，然后在DrawLines调用，如下：

```c#
 public class Line : MonoBehaviour 
    {
        private LineRenderer lineRenderer;
        List<Vector3> points = new List<Vector3>();
        void Awake()
        {
            lineRenderer = GetComponent<LineRenderer>();
        }
        public void DrawLines(Vector3 pos)
        {
            points.Add(pos);
            lineRenderer.positionCount = points.Count;
            for(int i=0;i<points.Count;i++)
            {
                lineRenderer.SetPosition(i, points[i]);
            }
        }
        public void SetWidth(float start,float end)
        {
            lineRenderer.startWidth = start;
            lineRenderer.endWidth = end;
        }
    }
```

即可实现如下多条线段的绘制

![](https://github.com/shishouheng/Unity-learning/blob/main/images/DrawMulitLines.png)
