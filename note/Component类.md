# Component类

Component类是组件类的基类，组件代表所有可以附加到GameObject的对象。如常用的Collider(碰撞)、Rigibody(刚体)、Transform（变换）和Behaviour(行为)都是Component的子类，而我们最常用的Monobehaviour又继承于Behaviour类，也就间接的继承了Component类。由此可以看出Component是unity中一个非常重要的类。

因此我们可以把Component类看作是一个游戏对象的属性和组件的集合，一个游戏对象通常包含了多个组件，不同的组件确定了对象的不同功能，如碰撞、移动、显示等，在unity中可以通过添加或移除组件来定制和修改一个游戏对象的属性和行为。所以可以经常可以看到这么一句话：unity是面向组件编程的游戏引擎。就是因为它的设计思路是将游戏对象看作是由多个组件组成的，每个组件提供独立的功能，具有一定的内聚和松耦合性。每个组件都可以独立地添加到、删除或切换到游戏对象上，由此来实现游戏对象的不同外观、行为和功能。

## unity类的继承关系图

![](https://github.com/zhushouheng/Unity-learning/blob/main/images/%E7%B1%BB%E7%BB%A7%E6%89%BF%E5%85%B3%E7%B3%BB%E5%9B%BE.png)
