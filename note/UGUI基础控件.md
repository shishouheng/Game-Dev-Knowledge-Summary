# UGUI基础控件

## 一、可视化控件

### 1、1 Text

- **属性介绍：**

![]()



- **相关组件：**

**Out Line：** 可以为文本或者图像添加简单的轮廓效果。轮廓组件有以下几个属性：包括轮廓的颜色、轮廓效果在水平和垂直方向的距离以及是否将图形颜色叠加到效果颜色上

**Shadow：** 可以为文本或者图像添加阴影效果。阴影组件具有以下几个属性：阴影的颜色、阴影的偏移（表示为矢量）以及是否将图形颜色叠加到效果颜色上



### 1、2 Image

Image中比较重要的属性是Image Type，并具有以下四种选项，需要根据需求来确定Image Type

- Simple：简单模式，图像按原样显示，一般用于不需要根据控件大小进行拉伸或平铺的普通图片

- Sliced：切片模式，图像被分为九个部分（四个角，四条边和中心），并根据控件的大小进行拉伸。通常用于需要自由调整大小的UI元素，如按钮、面板和窗口，允许图像的边缘保持不变而中心部分可以根据控件大小进行拉伸

- Tiled：平铺模式，图像被重复平铺以填充控件

- Filled：填充模式，图像被部分显示来表示进度。可以选择填充方法：水平、垂直、

左<---->右，上<---->下。一般用于技能冷却、加载进度、血条等等

案例：鼠标右键实现血条的减少

    public class Hp : MonoBehaviour
    {
        public Image hpImage;
        private float allHp = 100;
        private float currentHp = 100;
        public float CurrentHp
        {
            get { return currentHp; }
            set
            {
                if (value <= 0)
                    currentHp = 0;
                else
                    currentHp = value;
            }
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                currentHp -= Random.Range(5, 10);
                hpImage.fillAmount = currentHp / allHp;
            }
        }
    }  



### 1、3 Raw Image

![]()

Raw Image在游戏开发中通常有以下三种用法

- 展示一张普通的图片（相比Image占用的内存会更大，尽量避免）

- 实现小地图的效果：
  
  1、创建一个摄像机调整角度获取小地图需要播放的画面
  
  2、创建一个Render Texture并分配给该摄像机的Target Texture属性，这样摄像机捕捉的画面会被渲染到Render Texture上
  
  3、在UI界面创建一个Raw Image组件，并将Texture属性设置为刚才创建的Render Texture。

- 播放视频：
  
  1、创建一个Video Player组件，并将Video Clip属性设置为需要播放的视频文件
  
  2、将Video Player中的Render Mode属性设置为Render Texture，并创建一个Render Texture来接受视频画面
  
  3、创建Raw Image，并将Textrue属性设置为刚才创建的Render Texture
  
  如果需要播放声音的话再添加一个Audio Sources组件





## 二、可交互控件

### 2、1 Button

可以通过编辑器拖拽或者写代码这两种方式实现点击Button触发相应的事件

- 编辑器拖拽：
  
  首先随意写一段代码，并将这段代码的脚本挂载到Canvas上
  
      public void OnClickBtn1()
          {
              Debug.Log("开始执行重新开始游戏的逻辑");
          }
      

   然后将脚本挂载的物体拖拽到事件中并选择需要触发的事件即可![]()



- 通过代码添加事件：
  
      public class ButtonTest : MonoBehaviour
      {
          public Image buttonImage;
          public Sprite targetImage;
          public Button button;
      	// Use this for initialization
      	void Start ()
          {
              if(button!=null)
              {
                  button.onClick.AddListener(() => button.transform.GetComponent<Image>().sprite= targetImage);  
              }
      	}
      }

通过代码给按钮添加事件需要通过onClick.AddListener方法来添加



### 2、2 Toggle

该组件允许用户在两个状态之间进行切换。通过用于表示开/关或启用/禁用设置。

该组件由一个可选的背景图像、一个可选的标签文本和一个切换开关（通常是一个复选框）组成。当用户点击切换开关时，Toggle组件的状态将改变并触发一个事件

     tog.onValueChanged.AddListener(OnClickTog); 
     private void OnClickTog(bool isOn)
        {
            Debug.Log("按下了开关"+isOn);
        }
    
    

 通过onValueChanged.AddListener方法来添加事件



### 2、3 Slider

滑动条，允许用户通过拖动滑块来选择一个范围内的值。通常用于调整音量、亮度或者其他需要精确控制的设置。

该组件由一个背景图像、一个可填充区域和一个滑块组成。用户可通过拖动滑块或点击可填充区域来更改Slider组件的值



### 2、4 Scrollbar

滑块，允许用户通过拖动滑块或点击箭头按钮来滚动内容。通常如Scroll View组件结合使用以提供滚动视图

该组件由一个背景图像、两个箭头按钮和一个滑块组成。用户可以通过拖动滑块、点击箭头按钮或点击背景图像来更改Scrollbar组件的值



### 2、5 Dropdown

下拉菜单，允许用户从一个下拉菜单中选择一个选项。Dropdown组件通常用于选择设置或者配置选项。

Dropdown组件由一个标签文本、一个箭头图像和一个下拉菜单组成。当用户点击箭头图像时，下拉菜单将展开，显示所有可用选项。用户可点击其中一个选项来选择他，然后下拉菜单将关闭，并且标签文本将更新为显示所选选项



### 2、6 InputField

输入框，允许用户输入文本，通常用于获取用户输入，如用户名、密码或搜索查询。

该组件由一个文本框和一个可选的占位符组成。当用户点击文本框时，他们可以开始输入文本。占位符文本在文本框为空时显示，用于提示用户应该输入什么内容

Placeholder：提示的文本信息

Text：用户输入的文本信息



### 2、7 Panel

通常充当父物体来将多个UI元素分组在一起，以便更好的组织UI界面



### 2、8  Scroll View

允许用户通过拖动或滚动来查看超出当前视图范围的内容。通常用于显示长列表或大型文本块（如背包道具以及游戏公共等）。

Scroll View组件由一个视口（Viewport）、一个可选的垂直滚动条（Vertical Scrollbar）和一个可选的水平滚动条（Horizontal Scrollbar）组成。视口定义了用户可以看到的内容区域，而滚动条允许用户控制当前查看的内容

通常与以下布局组件结合使用以实现需要的效果

- ContentSizeFitter：用于调整游戏对象的大小以适应子对象的大小。具有两个属性：Horizontal Fit和Vertical Fit，分别控制水平和垂直方向上的调整方式

- Grid Layout Group：用于将子对象排列成网格形式。可以控制网格的布局方式、单元格大小、间距和填充

- Vertical Layout Group：将子对象序列垂直排列，可控制子对象的对齐方式、间距和填充

- Horizontal Layout Group：将子对象水平排列，可控制子对象的对齐方式、间距和填充



搭配使用可实现道具背包和游戏公共的效果：

![]()

![]()
