# Animator
## ①Transitions
 Has Exit Time：勾选的情况下动画切换会先把前一个动画播完再进入下一个动画
 Transition Duration：过渡时间，从一个动画过渡到另一个动画的时间

![[Transitions.png]]
以该图为例，
勾选了Has Exit Time的情况下，如果发生了动画切换，会从0：00开始播放上一个动画，直到0：20时开始过渡到下一个动画
若未勾选，动画切换时则会直接从0：20开始播放
Transition Duration是动画的过渡时间，即该图中的0：20——1：00范围

# 向量相关
## ① Vector3.Angle:

```c#
float angle = Vector3.Angle(from, to);
```
返回的角度总是两个向量之间的最小夹角，即to不管在from左边60°还是右边60°返回的都是60°


# Collider
## ① Character Controller
在通过射线或者其他方式获取碰撞体信息时，Character Controller实际的碰撞体会比在scene窗口显示出来的碰撞体小一些，这是由于角色控制器的特殊设计有关，因此在一些极限情况通过Debug.DrawLine画出来的射线看起来接触到了碰撞体，但实际上并没有


# Navmesh
##  ①NavmeshAgent
### 1、SetDestination:
当给代理通过SetDestination设置目标点时，如果中途将isStopped设置为true，需要重新设置目标点

### 2、Agent位置赋值
如果agent的默认位置与实例化时需要在的位置之间是分离的（即二者之间有一块无法烘焙的位置），通过代码加载agent时必须在实例化agent的同时给agent的位置进行赋值（即在一行代码中实例化+赋值），否则通过**agent.position=target.position** 会导致agent无法在目标位置生成
这是因为通过给agent.position赋值是修改位置属性，此时agent会尝试移动到目标位置，但是目标位置与现在的位置之间是分离的，导致agent无法移动到目标位置，只会出现在最靠近目标位置的可行走点
如果无法在同一行对位置进行赋值，就需要通过agent.warp方法给agent赋值，这种方式是将agent“传送”到目标位置，可以绕过NavMesh的计算，直接将agent置于目标位置



## ②OffMeshLink

### 1、自定义OffMeshLink结束后需要通知Unity

当agent通过开发者自定义的OffMeshLink时，需要通过Agent.CompleteOffMeshLink方法手动通知Unity的导航系统OffMeshLink连接已经完成

这是因为对于自定义的网格连接无法被导航系统自动识别和处理，如果在完成网格连接后不通知导航系统那么agent可能会继续按照原来的路径移动，导致出现错误的行为