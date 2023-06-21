# Monobehaviour的生命周期函数和事件函数

Monobehaviour类是unity中脚本的基类，所有用户创建的unity脚本都默认派生自该类。并且只有继承了Monobehaviour的类才可以挂载到创建的游戏对象身上。用来代表游戏对象的某一行为。

例：可以创建一个继承于Monobehaviour类的PlayerController脚本，并将其附加到游戏对象身上，然后在PlayerController脚本中可以编写代码来控制角色的移动、跳跃、攻击等行为

在Monobehaviour类中有两组特殊的函数：生命周期函数和事件函数

## 生命周期函数

生命周期函数会在脚本实例的不同阶段被调用（脚本实例的含义是将一个脚本附加到游戏对象身上时，这个脚本就会被实例化，即创建了一个脚本实例）。在脚本的生命周期内，会按预定顺序自动执行这些函数。

完整的生命周期函数执行顺序为：

1 Awake：唤醒，当一个脚本实例被载入时被调用，一般用于用于游戏开始之前初始化变量或者游戏状态

2 OnEnable：当对象可用或激活时调用

3 Start：仅在Update函数第一次被调用前调用，并且只会在脚本实例启用时被调用一次

4 FixedUpdate：在每个固定的物理时间内执行一次（一般是0.02s执行一次，可在Time Manager中设置），不受设备配置的影响，所以常用于物理相关的更新，例如刚体和碰撞检测

5 Update：每帧执行一次

6 LateUpdate：每帧更新后执行的函数，在Update函数之后执行，因此可以确定在LateUpdate函数执行时，所有Update函数中的计算都已经完成，一般在此执行摄像机的移动和旋转，可以确保在摄像机跟随之前角色已经完成了移动

7 OnGUI：在渲染GUI或者处理GUI消息时调用

8 OnApplicationPause：在帧的结尾处调用，当检测到游戏暂停时会将所有GameObject的OnApplicationPause设置为true

9 OnApplicationFocus：当检测到游戏开始运行时会返回一个true

10 OnDisable：当游戏对象被禁用时调用

11 OnDestroy：当游戏对象被销毁时调用

12 OnApplicationQuit：当游戏退出时调用

unity官方也提供了完整的生命周期函数图，如下：

![image](D:\Unity学习笔记\Unity-learning\note\a27daec1899241e69f5e13916c1fffe2.png)

## 事件函数

事件函数是Unity中脚本与传统程序概念不同的地方。在传统程序中，代码在循环中连续运行直到完成任务。而unity通过调用在脚本中声明的某些函数来间歇的将控制权交给脚本。函数执行完毕后，控制权交回给unity。这些函数由unity激活以响应游戏中发生的事件，因此称为事件函数。常见的事件函数有：

OnCollisionEnter：发生碰撞时执行

OnCollisionStay：持续碰撞时执行

OnCollisionExit：碰撞结束时执行

OnTriggerEnter：触发时执行

OnTriggerStay：持续触发时执行

OnTriggerExit：触发结束时执行

OnMouseDown：鼠标按下时执行

OnMouseDrag:鼠标按下(未抬起)一直执行

OnMouseUp:鼠标抬起时执行

等等等等，完整的可以参考unity官方文档[Unity - 脚本 API：单体行为 (unity3d.com)](https://docs.unity3d.com/ScriptReference/MonoBehaviour.html)
