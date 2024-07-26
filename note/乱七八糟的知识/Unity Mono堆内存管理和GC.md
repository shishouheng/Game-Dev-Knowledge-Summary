在Unity中，内存分为了库代码、Native堆和Mono堆
- 库代码：Unity库和第三方库
- Native堆：由C++代码管理的内存堆，主要存储引擎的核心数据结构和对象。Native堆在unity的内部用来管理引擎的运行时数据，包括场景、游戏对象、组件、材质、纹理等资源（提供了API以供在C#代码中来进行资源的加载与释放）
- Mono堆：由Mono运行时环境管理的内存堆，用于存储C#脚本中的数据和对象

![[GameRunningMemory.png]]

![UnityDevTips/Image/GameRunningMemory.png at main · shishouheng/UnityDevTips (github.com)](https://github.com/shishouheng/UnityDevTips/blob/main/Image/GameRunningMemory.png)


在C#代码中内存分为堆栈和托管堆两部分，其中托管堆就是Mono内存。堆栈的内存由编译器进行分配和释放，托管堆则通过Mono的GC来进行管理，由于GC的过程较为耗时容易造成游戏的卡顿，因此开发者也需要对GC进行管理避免频繁触发GC影响玩家游戏体验。

Mono内存又分为两部分，已用内存（used）和堆内存（heap）
- 已用内存：程序当前已经分配并使用的内存空间
- 堆内存：动态分配的内存区域，由mono向操作系统进行申请

堆内存和已用内存的差值就是mono的空闲内存，这个差值可以用来衡量程序运行时动态分配的内存是否得到有效释放以及内存管理的效率（如果堆内存中的未使用内存过多可能会导致内存泄漏的问题）。
当mono需要分配内存时会先查看空闲内存是否足够，如果足够会直接在空闲内存中分配，否则会进行一次GC来释放更多的空闲内存，如果GC后仍然没有足够的空闲内存，那么mono会向操作系统申请内存并扩充堆内存

![[GCFlow.png]]
![UnityDevTips/Image/GCFlow.png at main · shishouheng/UnityDevTips (github.com)](https://github.com/shishouheng/UnityDevTips/blob/main/Image/GCFlow.png)

由此可知道GC的主要作用是从已用内存中找出不再需要被使用的内存并进行释放。Mono中的GC主要有以下几个步骤：
1.停止所有需要mono内存分配的线程（因此无论是否在主线程调用都会造成一定的卡顿）
2.遍历所有已用内存，找到不再需要被使用的内存并进行标记
3.释放被标记的内存到空闲内存
4.重新开始被停止的线程

**注：GC释放的内存指挥留给Mono使用而不会返还给操作系统，因此Mono堆内存是只增不减的。**



## Mono内存泄漏分析

Mono通过引用查找的方式来判断已用内存中不再被需要的部分，首先将正在活动的对象标记为Root节点，之后查找被其直接或间接引用的对象，这些都会被标记为可达对象，而没有被标记的则是不可达对象，不可达对象将会通过GC回收。

由此可分析出内存泄漏的主要来源：
- 静态对象的引用：由于静态对象自始至终都是root节点，被静态对象直接或间接引用的对象将不会被释放，会导致内存泄漏
- 运行时保存的内部对象：如在运行时的某个脚本的内部变量引用了不再被使用的对象，会导致内存泄漏


## GC优化策略

1.减少创建临时对象和字符串拼接
2.使用对象池
3.大量字符串拼接时使用StringBuilder
4.关卡切换或其他loading节点时提前申请内存分配
5.loading时主动调用GC.Collect，