
# 一、Xlua的映射方式

- 映射到普通类或者结构体
- 映射到接口interface
- 映射到集合List、Dictionary
- 映射到LuaTable、LuaFunction
- 映射到委托事件

# 二、在unity中执行lua语句及加载lua文件信息

在unity中加载lua文件信息必须把lua文件放在Resources文件夹下，同时后缀需要是.lua.txt

先编写一个具有全局变量、方法、表的lua文件，然后命名为test1.lua.txt放在Resources下
```lua
Config={name='张三',age=20,height=170}

Name='李四'
Age=20
IsBoy=true

function printInfo(a)
	print('c#传递的参数是：'..a)
	return 100
end

print('test1.lua执行完毕')
```

然后写c#脚本来尝试加载这个lua文件内的信息或者在c#环境中执行lua语句

```c#
using UnityEngine;
using XLua;
using System;

delegate int Fun(int x);

public class XLuaTestt : MonoBehaviour
{
    LuaEnv env = new LuaEnv();

    private void OnGUI()
    {
        if (GUILayout.Button("c#环境执行lua语句"))
            CSharp_DoLua();
        if (GUILayout.Button("c#加载lua文件加载内部信息"))
            CSharp_CallLuaFile();
    }

    void CSharp_DoLua()
    {
        env.DoString("print 'hello world'");
    }

    void CSharp_CallLuaFile()
    {
        env.DoString("require 'test1'");

        //加载lua中的全局变量
        Debug.Log(env.Global.Get<string>("Name"));
        Debug.Log(env.Global.Get<int>("Age"));
        Debug.Log(env.Global.Get<bool>("IsBoy"));

        //加载lua的表和表中数据
        LuaTable table = env.Global.Get<LuaTable>("Config");
        Debug.Log(table.Get<string>("name"));
        Debug.Log(table.Get<int>("age"));
        Debug.Log(table.Get<int>("height"));

        //映射到c#类
        Config c = env.Global.Get<Config>("Config");
        Debug.Log(c.name);
        Debug.Log(c.age);
        Debug.Log(c.height);

        //把lua函数映射到LuaFunction
        LuaFunction function = env.Global.Get<LuaFunction>("printInfo");
        object[] datas = function.Call(1060);
        Debug.Log(Convert.ToInt32(datas[0]));

        //把lua函数映射到委托
        Fun f = env.Global.Get<Fun>("printInfo");
        Debug.Log(f(30));
    }
}

public class Config
{
    public string name;
    public int age;
    public int height;
}
```

# 三、Lua调用UnityAPI

## 1.通过Lua读取类的信息并打印在控制台

先自定义一个Hero类
```c#
public class Hero
{
    private int hp;
    public int Hp { get { return hp; } set { hp = value; } }

    private string name;
    public string Name { get; set; }

    public Hero(string name,int hp)
    {
        this.name = name;
        this.hp = hp;
    }

    public void PrintHero()
    {
        Debug.Log(name + "的血量是：" + hp);
        
    }

    public int AddHp(int _hp)
    {
        Debug.Log("Lua脚本传递的参数是：" + _hp);
        this.hp += _hp;
        return this.hp;
    }
}
```

然后再一个Lua脚本中写如下代码
```lua
local hero=CS.Hero("薇恩",100)
hero:PrintHero()

print(hero.Hp)

print('c#的返回值是:'..hero:AddHp(5))
```

最后在unity中加载该Lua脚本即可看到控制台成功打印出类的信息


## 2.C#与Lua的交互实现生命周期函数

```c#
using UnityEngine;
using XLua;
using System;

public class LuaCall : MonoBehaviour
{
    LuaEnv env = new LuaEnv();

    //对应的lua文件
    public TextAsset asset;

    //需要映射的生命周期委托
    private Action luaAwake;
    private Action luaStart;
    private Action luaUpdate;
    private Action luaDestroy;
    private Action luaOngui;

    //存储当前对应lua脚本中的信息的table
    private LuaTable table;

    public AnimationClip clip;
    public GameObject other;
    private void Awake()
    {
        table = env.NewTable();
        //元表
        LuaTable meta = env.NewTable();
        meta.Set("__index", env.Global);
        table.SetMetaTable(meta);

        //释放
        meta.Dispose();

        //第三个参数表示内容都加载到这个表中,以后查找数据就从这个table中查找
        env.DoString(asset.text, "TestLua", table);

        //将lua中的self在编译时转换为this
        table.Set("self", this);
        table.Set("clip", clip);
        table.Set("obj1", other);

        //将table中的函数映射到对应的委托中
        table.Get("awake", out luaAwake);
        table.Get("start", out luaStart);
        table.Get("update", out luaUpdate);
        table.Get("onDestroy", out luaDestroy);
        table.Get("ongui", out luaOngui);

        
        if (luaAwake != null)
            luaAwake();
    }

    private void Start()
    {
        if (luaStart != null)
            luaStart();
    }

    private void Update()
    {
        if (luaUpdate != null)
            luaUpdate();
    }

    private void OnGUI()
    {
        if (luaOngui != null)
            luaOngui();
    }

    private void OnDestroy()
    {
        if (luaDestroy != null)
            luaDestroy();
    }
}
```

```lua
local speed=2
local collider=nil
local anim=nil

function awake()
	
end


function start()
	--调用unity api输出物体名字
	CS.UnityEngine.Debug.Log(self.name)
	--取消物体的碰撞体
	collider=self.gameObject:GetComponent(typeof(CS.UnityEngine.Collider))
	collider.enabled=false;
	
	--获取animation组件并添加动画片段
	anim=self.gameObject:GetComponent(typeof(CS.UnityEngine.Animation))
	anim:AddClip(clip,'GG')
	--修改动画片段播放模式
	clip.wrapMode=CS.UnityEngine.WrapMode.Loop
	anim:Play('GG')
	
	--实例化五个预制体
	for i=1,5 do
		prefab=CS.UnityEngine.Resources.Load('g')
		CS.UnityEngine.Object.Instantiate(prefab)
	end
end


function update()
	--旋转
	local r=CS.UnityEngine.Vector3.up*speed
	self.transform:Rotate(r)
end


function ongui()

end

function onDestroy()

end
```

## 3.Lua控制角色移动、变色、发射射线

```lua
local speed=0.1
local h=0
local v=0
local dir=CS.UnityEngine.Vector3.zero
local rigid
local color
local transform




function start()
		rigid=self.gameObject:AddComponent(typeof(CS.UnityEngine.Rigidbody))
		transform=self.transform
		color=CS.UnityEngine.Color.blue
end


function update()
	h=CS.UnityEngine.Input.GetAxis('Horizontal')
	v=CS.UnityEngine.Input.GetAxis('Vertical')
	dir=CS.UnityEngine.Vector3(h,0,v)
	
	transform:Translate(dir*speed)
	
	Jump()
	DrawLine()
end


function ongui()
	if CS.UnityEngine.GUILayout.Button('变色') then
		self.gameObject:GetComponent(typeof(CS.UnityEngine.Renderer)).material.color=color
	end
end

function onDestroy()

end

function Jump()
	if CS.UnityEngine.Input.GetKeyDown(CS.UnityEngine.KeyCode.Space) then
		rigid:AddForce(CS.UnityEngine.Vector3.up*300)
	end
end

function DrawLine()
	CS.UnityEngine.Physics.Raycast(transform.position,transform.forward,10)
	CS.UnityEngine.Debug.DrawLine(transform.position,transform.position+transform.forward*10)
end
```


## 3.Lua控制相机跟随

同样是先实现生命周期
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using XLua;

public class CameraFollow : MonoBehaviour
{
    LuaEnv env = new LuaEnv();

    //对应的lua文件
    public TextAsset asset;

    //需要映射的生命周期委托
    private Action luaAwake;
    private Action luaStart;
    private Action luaUpdate;
    private Action luaDestroy;
    private Action luaOngui;

    //存储lua脚本中的信息的table
    private LuaTable table;


    private void Awake()
    {
        table = env.NewTable();
        //元表
        LuaTable meta = env.NewTable();
        meta.Set("__index", env.Global);
        table.SetMetaTable(meta);

        //释放
        meta.Dispose();

        //第三个参数表示内容都加载到这个表中,以后查找数据就从这个table中查找
        env.DoString(asset.text, "CameraFollow", table);

        //将lua中的self在编译时转换为this
        table.Set("self", this);


        //将table中的函数映射到对应的委托中
        table.Get("awake", out luaAwake);
        table.Get("start", out luaStart);
        table.Get("update", out luaUpdate);
        table.Get("onDestroy", out luaDestroy);
        table.Get("ongui", out luaOngui);


        if (luaAwake != null)
            luaAwake();
    }

    private void Start()
    {
        if (luaStart != null)
            luaStart();
    }

    private void Update()
    {
        if (luaUpdate != null)
            luaUpdate();
    }

    private void OnGUI()
    {
        if (luaOngui != null)
            luaOngui();
    }

    private void OnDestroy()
    {
        if (luaDestroy != null)
            luaDestroy();
    }
}
```

然后在lua中实现相机跟随的逻辑
```lua
local dir
local target
local pos

function start()
	target=CS.UnityEngine.GameObject.Find('Cube').transform
	dir=self.transform.position-target.transform.position
end


function update()
	pos=target.position+dir
	self.transform.position=CS.UnityEngine.Vector3.Lerp(self.transform.position,pos,CS.UnityEngine.Time.deltaTime)
end
```

## 4.Lua中使用Unity的协程

在lua中使用unity的协程需要导入xlua提供的"xlua.util"模块(通过内部提供的coroutine_call方法唤醒协程)和"cs_coroutine"模块(使用内部提供的yield_return方法，这样即可在lua中使用unity提供的WaitForSeconds)

```lua
--导入xlua提供的'xlua.util'实现协程
local util=require 'xlua.util'

--将'cs_couroutine'模块中的'yield_return'函数赋值给自定义的yield_return
local yield_return=(require 'cs_coroutine').yield_return

local fun=function()
	print('StartCoroutine')
	
	for i=1,3 do
		yield_return(CS.UnityEngine.WaitForSeconds(3))
		print('3s到了')
	end
end

local co1=util.coroutine_call(fun)
co1()
```

## 5.在lua中使用数组、泛型集合等

创建数组遍历数组的方式,不能使用lua的pairs和ipairs来遍历int类型的数组，因为这两个是遍历lua的table数据结构的，而c#数组的数据结构不同于lua的数据结构
```lua
--创建一个int类型的数组，并且长度为5
local array=CS.System.Array.CreateInstance(typeof(CS.System.Int32),5)
print(array.Length)

--遍历数组
for i=0,4 do
	print(array[i])
end
```


创建动态数组
```lua
function start()
	--创建一个string类型的动态数组并用StarList接收
	local StrList=CS.System.Collections.Generic.List(CS.System.String)
	--实例化该类型的动态数组
	local list=StrList()
	--传入数据
	list:Add('add')
	list:Add('fff')

	
	--通过迭代器遍历(这里可以用的原因是xlua帮我们封装好了)
	for k,v in pairs(list) do
		print(k,v)
	end
end

start()
```


创建字典

在lua中访问字典时不可以直接通过["key"]的方式访问值，也不可以这样直接修改值，只能通过get_Item和set_Item方式来访问或修改字典中的值

**也可以使用unity中提供的TryGetValue方法来访问值，在unity中该方法需要传入一个out修饰的参数，但由于lua支持多返回，因此在lua中调用这种类型的方法的时候无需传入out修饰的参数**
```lua
function start()
	--创建一个string为键，vector3为值的字典类型
	local Dic_Str_Vec=CS.System.Collections.Generic.Dictionary(CS.System.String,
	CS.UnityEngine.Vector3)
	--创建该字典类型的实例
	local dic=Dic_Str_Vec()
	--添加元素
	dic:Add('a',CS.UnityEngine.Vector3(1,2,3))
	--获取元素
	print(dic['a'])--输出nil
	print(dic:get_Item('a'))--(1,2,3)
	
	--修改元素
	dic:set_Item('a',CS.UnityEngine.Vector3(2,4,6))
	print(dic:TryGetValue('a')--在unity中需要传入out的函数在lua中都不用传，因为lua支持多返回值
end
start()
```


## 6.在Lua中执行Invoke和InvokeRepeating

```lua
function start()
	--Invoke是Monobehaviour这个父类里继承来的方法，因此通过self调用
	self:Invoke('Fun1',3.0)
	self:InvokeRepeating('Fun2',1.0,1.0)
end

function Fun1()
	print('fun1')
end

function Fun2()
	print('fun2')
end
```

```c#
using UnityEngine;
using XLua;
using System;

public class InvokeTest : MonoBehaviour
{
    public TextAsset asset;
    LuaEnv env = new LuaEnv();

    private LuaTable table;

    private Action luaStart;
    private Action luaFun1;
    private Action luaFun2;

    private void Awake()
    {
        table = env.NewTable();
        //设置元表
        LuaTable meta = env.NewTable();
        meta.Set("__index", env.Global);
        table.SetMetaTable(meta);
        //释放
        meta.Dispose();

        //加载对应表中的脚本
        env.DoString(asset.text,"InvokeTest",table);

        table.Set("self", this);

        table.Get("Fun1", out luaFun1);
        table.Get("Fun2", out luaFun2);
        table.Get("start", out luaStart);
     
    }
    private void Start()
    {
        if (luaStart != null)
            luaStart();
    }

    void Fun1()
    {
        if (luaFun1 != null)
            luaFun1();
    }

    void Fun2()
    {
        if (luaFun2 != null)
            luaFun2();
    }
}
```

## 7.在Lua中使用OnCollision事件函数

在碰撞的时候输出碰撞物体的名字

```lua
function LuaOnCollisionEnter(other)
	print(other.gameObject.name)
end

function LuaOnCollisionStay(other)

end

function LuaOnCollisionExit(other)

end
```

```c#
//带参数的委托，用来接收lua中传递的方法
//需要使用一个特性对该委托进行注册并重新生成代码，否则会报错
[CSharpCallLua]
public delegate void OnCollision(Collision other);

public class CollisionTest : MonoBehaviour
{
    public TextAsset asset;
    LuaEnv env = new LuaEnv();
    private LuaTable table;

    private OnCollision luaOnCollisionEnter;
    private OnCollision luaOnCollisionStay;
    private OnCollision luaOnCollisionExit;
    private void Awake()
    {
        table = env.NewTable();
        LuaTable meta = env.NewTable();
        meta.Set("__index", env.Global);
        table.SetMetaTable(meta);
        meta.Dispose();
        env.DoString(asset.text, "CollisionTest", table);

        table.Set("self", this);

        table.Get("LuaOnCollisionEnter", out luaOnCollisionEnter);
        table.Get("LuaOnCollisionStay", out luaOnCollisionStay);
        table.Get("LuaOnCollisionExit", out luaOnCollisionExit);
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (luaOnCollisionEnter != null)
            luaOnCollisionEnter(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (luaOnCollisionStay != null)
            luaOnCollisionStay(collision);
    }

    
}
```

在这里如果未给自定义的OnCollision委托添加标签，就会报错

**这是因为[CSharpCallLua]特性是用于标记委托类型的特性，指示该委托是可以从Lua中调用的，如果未添加该特性Lua则无法识别这个委托** 


# 四、XLua的配置方式

XLua是一个用于在unity中实现C#和Lua之间互相调用的桥接工具。在使用XLua时，可以通过不同的配置方式来指定Lua文件的加载方式、绑定规则以及其他的一些配置

## 1.Attribute打标签


### XLua.LuaCallSharp
该特性用于标记Lua中可以调用的C#函数或方法，添加此配置后XLua会生成这个类型的适配代码（即wrap结尾的代码），适配代码中包含该类型的构造、成员属性、方法、静态属性、静态方法等等内部元素的访问和设置方法，这些都会被自动绑定到Lua中并且可以在Lua中进行调用

如果不添加此标记也可以正常运行但是会使用性能较低的反射来进行访问和设置


### XLua.CSharpCallLua
如果需要把一个Lua的函数映射到一个c#的自定义委托（不包括c#提供的委托，已经帮我们实现了）或者把一个Lua的表映射到一个c#的接口，该委托或者接口 **必须添加CSharpCallLUA** 


### XLua.BlackList
黑名单，如果不希望一个类中的某些成员生成适配代码，确定这些成员不会再Lua中调用，可以用这个特性来剔除


## 静态列表
指在配置文件或者注解中直接指定需要绑定的c#类型和lua函数。这些列表再编译时就确定了不会再运行时改变。静态列表的优点是配置简单，缺点是不够灵活，无法在运行时动态添加或删除绑定


## 动态列表
指在运行时动态添加或删除需要绑定的c#类型和lua函数，在xlua中可以通过lua脚本来指定动态列表。通过调用xlua提供的接口可以在lua脚本中动态的添加或删除需要绑定的c#类型和lua函数。动态列表的优点是灵活性高，可以根据运行时的需求动态调整，缺点是配置复杂



# 五、c#与lua的交互父类

首先实现一个创建lua虚拟机及常用方法的LuaEnvironmentManager
```c#
using XLua;

public class LuaEnvironmentManager
{
    //提供获取实例的唯一方式
    private static LuaEnvironmentManager instance;
    public static LuaEnvironmentManager Instance
    {
        get
        {
            if (instance == null)
                instance = new LuaEnvironmentManager();
            return instance;
        }
    }

    private LuaEnv env;
    public LuaEnv Env
    {
        get { return env; }
    }

    //私有构造
    private LuaEnvironmentManager()
    {
        env = new LuaEnv();
    }

    //获取全局表
    public LuaTable GetGlobalTable()
    {
        return env.Global;
    }

    //创建新的table
    public LuaTable CreateLuaTable()
    {
        return env.NewTable();
    }

    //释放
    public void Dispose()
    {
        env.Dispose();
    }
}
```

然后实现一个lua的父类用来封装常用方法，后续代码直接继承该脚本即可
```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
using System;

[CSharpCallLua]
public delegate void OnCollisionDelegate(Collision other);

public class LuaParent : MonoBehaviour
{
    public TextAsset asset;
    protected LuaTable table;

    protected Action luaAwake;
    protected Action luaStart;
    protected Action luaUpdate;
    protected Action luaDestroy;
    protected Action luaOngui;

    protected OnCollisionDelegate luaCollisionEnter;
    protected OnCollisionDelegate luaCollisionStay;
    protected OnCollisionDelegate luaCollisionExit;

    private void Awake()
    {
        table = LuaEnvironmentManager.Instance.CreateLuaTable();
        LuaTable meta = LuaEnvironmentManager.Instance.CreateLuaTable();
        meta.Set("__index", LuaEnvironmentManager.Instance.GetGlobalTable());
        table.SetMetaTable(meta);
        meta.Dispose();

        LuaEnvironmentManager.Instance.Env.DoString(asset.text, GetType().Name, table);
        SetLuaData();
        GetLuaData();

        if (luaAwake != null)
            luaAwake();

    }
    protected virtual void SetLuaData()
    {
        table.Set("self", this);
    }

    protected virtual void GetLuaData()
    {
        table.Get("awake", out luaAwake);
        table.Get("start", out luaStart);
        table.Get("update", out luaUpdate);
        table.Get("onDestroy", out luaDestroy);
        table.Get("ongui", out luaOngui);
        table.Get("LuaCollisionEnter", out luaCollisionEnter);
        table.Get("LuaCollisionStay", out luaCollisionStay);
        table.Get("LuaCollisionExit", out luaCollisionExit);
    }

    private void Start()
    {
        if (luaStart != null)
            luaStart();
    }

    private void Update()
    {
        if (luaUpdate != null)
            luaUpdate();
    }

    private void OnGUI()
    {
        if (luaOngui != null)
            luaOngui();
    }

    private void OnDestroy()
    {
        if (luaDestroy != null)
            luaDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (luaCollisionEnter != null)
            luaCollisionEnter(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (luaCollisionStay != null)
            luaCollisionStay(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (luaCollisionExit != null)
            luaCollisionExit(collision);
    }
}
```

# 六、完全通过Lua实现画线游戏逻辑

## lua逻辑层

首先封装一个名为common的公共lua模块来减少在lua中的代码量

```lua
Time=Unity.Time
Resources=Unity.Resources
Input=Unity.Input
Camera=Unity.Camera
LineRender=Unity.LineRenderer
EdgeCollider2D=Unity.EdgeCollider2D
Vector3=Unity.Vector3
Vector2=Unity.Vector2

Vector3List=CS.System.Collections.Generic.List(Vector3)
Vector2List=CS.System.Collections.Generic.List(Vector2)

--获取对象身上指定脚本的某个字段或者方法
function getLuaData(obj,script,dataName)
	local instance=obj:GetComponent(script)
	return instance:GetLuaData(dataName)
end
```

然后是每个线段预制体身上的逻辑

```lua
require 'common'

local lineRender=nil
local edge=nil
points=nil

function awake()
	lineRender=self.gameObject:GetComponent(typeof(LineRender))
	edge=self.gameObject:GetComponent(typeof(EdgeCollider2D))
	points=Vector3List()
end

--给linerenderer增加点和edge碰撞体的方法，后续在DrawLine中调用，将鼠标点位赋值给linerenderer的点位
function Draw()
	if lineRender==nil then
		return
	end
	
	lineRender.positionCount=points.Count
	local verts=Vector2List()
	
	for i=0,points.Count-1 do
		lineRender:SetPosition(i,points[i])
		verts:Add(Vector2(points[i].x,points[i].y))
	end
	
	if verts.Count>1 then
		edge.points=verts:ToArray()
	end
end


--设置线条宽度的方法
function SetWidth(startWidth,endWidth)
	lineRender.startWidth=start
	lineRender.endWidth=endWidth
end
```

画线的逻辑
```lua
require 'common'

local prefab=nil
local obj=nil
local beginDraw=false

function start()
	prefab=Resources.Load("Line")
end

function update()
	if Input.GetMouseButtonDown(0) then
		obj=GameObject.Instantiate(prefab)
		beginDraw=true
	end
	
	if Input.GetMouseButtonUp(0) then
		beginDraw=false
	end
	
	if beginDraw then
		local pos=Vector3(Input.mousePosition.x,Input.mousePosition.y,1.0)
		pos=Camera.main:ScreenToWorldPoint(pos)
		
		--通过common模块提供的方法获取obj对象身上的Line脚本，然后获取所需的成员
		local list=getLuaData(obj,typeof(CS.Line),'points')
		list:Add(pos)
		
		local drawFun=getLuaData(obj,typeof(CS.Line),'Draw')
		drawFun()
		
		local setWidth=getLuaData(obj,typeof(CS.Line),'SetWidth')
		setWidth(0.1,0.1)
	end
end
```

实例化小球的逻辑
```lua
require 'common'

local timer=2
local time=0
local ball=nil

function start()
	ball=Resources.Load('Ball')
end

function update()
	time=time+Time.deltaTime
	if time>=timer then
		local newball=GameObject.Instantiate(ball)
		GameObject.Destroy(newball,5)
		time=0
	end
end
```

## c#接收层

首先是LuaParent父类，封装接收Lua所需的成员字段和方法
```c#
using UnityEngine;
using XLua;
using System;

[CSharpCallLua]
public delegate void OnCollisionDelegate(Collision other);

[LuaCallCSharp]
public class LuaParent : MonoBehaviour
{
    public TextAsset asset;
    protected LuaTable table;

    protected Action luaAwake;
    protected Action luaStart;
    protected Action luaUpdate;
    protected Action luaDestroy;
    protected Action luaOngui;

    protected OnCollisionDelegate luaCollisionEnter;
    protected OnCollisionDelegate luaCollisionStay;
    protected OnCollisionDelegate luaCollisionExit;

    private void Awake()
    {
        table = LuaEnvironmentManager.Instance.CreateLuaTable();
        LuaTable meta = LuaEnvironmentManager.Instance.CreateLuaTable();
        meta.Set("__index", LuaEnvironmentManager.Instance.GetGlobalTable());
        table.SetMetaTable(meta);
        meta.Dispose();

        LuaEnvironmentManager.Instance.Env.DoString(asset.text, GetType().Name, table);
        SetLuaData();
        GetLuaData();

        if (luaAwake != null)
            luaAwake();

    }
    protected virtual void SetLuaData()
    {
        table.Set("self", this);
    }

    protected virtual void GetLuaData()
    {
        table.Get("awake", out luaAwake);
        table.Get("start", out luaStart);
        table.Get("update", out luaUpdate);
        table.Get("onDestroy", out luaDestroy);
        table.Get("ongui", out luaOngui);
        table.Get("LuaCollisionEnter", out luaCollisionEnter);
        table.Get("LuaCollisionStay", out luaCollisionStay);
        table.Get("LuaCollisionExit", out luaCollisionExit);
    }


    //获取对应的lua脚本里的信息
    public object GetLuaData(string dataName)
    {
        object obj;
        table.Get(dataName, out obj);
        return obj;
    }

    private void Start()
    {
        if (luaStart != null)
            luaStart();
    }

    private void Update()
    {
        if (luaUpdate != null)
            luaUpdate();
    }

    private void OnGUI()
    {
        if (luaOngui != null)
            luaOngui();
    }

    private void OnDestroy()
    {
        if (luaDestroy != null)
            luaDestroy();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (luaCollisionEnter != null)
            luaCollisionEnter(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if (luaCollisionStay != null)
            luaCollisionStay(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (luaCollisionExit != null)
            luaCollisionExit(collision);
    }
}
```

然后是几个用来接收对应lua脚本的c#类，只需要在unity中将对应的lua脚本拖拽赋值即可，无需任何其他逻辑
```c#
public class Line : LuaParent{}
public class DrawLine:LuaParent{}
public class InstantiateBall:LuaParent{}
```
