# NavMesh网格导航

## 一、基本介绍

### 1.1定义：

NavMesh是基于AStar寻路算法实现的在3D游戏世界中动态物体自动寻路的一种技术，将游戏中复杂的结构组织关系简化为带有一定信息的网格，并在携带信息的网格上通过一系列计算来实现自动寻路。

### 2.2主要功能：

- **寻路：** 可通过NavMesh计算出角色从一个点到另一个点的最短路径，并提供导航指令给角色。

- **避障：** 可以检测场景中的障碍物并帮助角色绕过这些障碍物

- **动态更新：** NavMesh可以在运行使动态更新，以适应场景中的动态障碍物的变化

### 3.3自动寻路的步骤

- 选中场景中的地面以及障碍物并在inspector面板将他们设置为Navigation Static（导航静态，意味着他们在游戏过程中无法通过任何方式进行移动）

- 选择Windows下拉菜单中的Navigation，此时就会在inspector面板右侧出现一个Navigation选项，点击Bake将场景烘焙好

- 给需要导航的物体添加NavMeshAgent（导航代理）组件，如玩家或者敌人

- 通过代码给导航代理组件设置目标点

## 二、NavMeshAgent属性介绍

![](https://github.com/shishouheng/Unity-learning/blob/main/images/NavMesh/meshAgent.jpg)

![](https://github.com/shishouheng/Unity-learning/blob/main/images/NavMesh/meshAgent%E5%B1%9E%E6%80%A7.png)

## 三、Navigation属性介绍

- **Agent Radius：** 代理半径，指烘焙路面时网格边缘距离路面边缘的值

- **Agent Height:** 代理高度，在场景中需要导航的物体高度

- **Max Slope：** 能通过的最大坡度

- **Step Height：** 台阶高度

**off Mesh Links：** 网格外链接 (Off-Mesh Link) 是一种用于合并无法使用可行走表面来表示的导航捷径的组件。例如，跳过沟渠或围栏，或在通过门之前打开门，全都可以描述为网格外链接。OffMeshLink 组件允许您手动指定路径线路，可以实现跨越鸿沟或者往高跳跃的效果。

在勾选Object面板中的Generate Offmeshlinks的前提下可以设置Drop Height（掉落高度）和Jump Distance（跳跃距离），使导航对象可以从高处向地处跳下或者从一个平面跳向另一个平面

![](https://github.com/shishouheng/Unity-learning/blob/main/images/NavMesh/drop%20height.jpg)

![](https://github.com/shishouheng/Unity-learning/blob/main/images/NavMesh/jump%20distance.jpg)

## 四、常用组件

### 4.1  OffMeshLink

通过在inspector面板中添加OffMeshLink组件并添加起点和终点，就可以实现在导航网格中创建不连续得路径，如悬崖、跳跃或者其他不规则得移动方式

![](https://github.com/shishouheng/Unity-learning/blob/main/images/NavMesh/offmeshlink.jpg)

### 4.3  NavMeshObstacle

动态障碍物，由于烘焙过得障碍物都添加了Navigation Static，所以他们在运行得时候无法通过任何方式进行移动，这个时候就引入了NavMeshObstacle，只要为场景中得某个物体添加了NavMeshObstacle无需添加Navigation Static，重新烘焙场景后该物体就可以影响导航系统中角色得路径规划，使其绕开障碍物，并且该障碍物可以通过代码任意移动，而不像勾选了Navigation Static得障碍物一样无法通过任何方式进行移动

![](https://github.com/shishouheng/Unity-learning/blob/main/images/NavMesh/navmeshObstacle.jpg)
