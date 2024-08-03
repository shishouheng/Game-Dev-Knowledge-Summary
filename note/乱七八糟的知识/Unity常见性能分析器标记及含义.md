当我们使用Unity的Profile录制性能数据时，可以看到每一帧的耗时分布，并以此来确定性能瓶颈，对于每个耗时点，如PlayerLoop、EditorLoop等都是Unity官方自己命名分配的tag，我们也可以自己在函数的入口和出口出添加BeginSample和EndSample来添加自定义的函数tag，来获取我们所关注的函数在每一帧的耗时情况。这里就对Unity官方命名的一些tag进行解释。

# 1.主线程基础标记

- PlayerLoop：游戏总循环的耗时，把PlayerLoop每一帧的耗时作为游戏每一帧的开销
- EditorLoop：仅在编辑器模式下能看到该标记，包含PlayerLoop以及编辑器运行所产生的开销

# 2. 脚本更新标记

- BehaviourUpdate：包含所有MonoBehaviour.Update方法在每一帧的开销
- CoroutineDelayCalls：首次生成后的所有协程的开销
- FixedBehaviourUpdate：包含所有FixedBehaviourUpdate方法在每一帧的开销
- PreLateUpdate.ScriptRunBehaviourLateUpdate:包含所有PreLateUpdate.ScriptRunBehaviourLateUpdate方法在每一帧的开销
- Update.ScriptRunBehaviour:包含所有Update.ScriptRunBehaviour和协程在每一帧的开销

# 3.渲染和VSync标记

### 3.1 WaitForTargetFPS：

该参数一般出现在CPU开销较低，且通过了设定的目标帧率的情况下。例如我们的游戏锁定为30帧，那么平均每帧的开销应该为33ms，但是如果在某帧只花费了20ms就完成了所有任务，那么就会在下一帧产生一个13ms的WaitForTargetFPS开销来维持目标帧率，如果下一帧其他所有耗时为25ms，那么加上因为上一帧耗时太低而产生的WaitForTargetFPS，这一帧在Profile中的总耗时就变为了38ms。
**因此，由WaitForTargetFPS造成的Profile开销较高的现象，只是耗时的假象，可以对它视而不见*

### 3.2 Gfx.WaitForPresent && Graphics.PresentAndSync

这两个参数经常出现在CPU占用较高的情况。产生的原因是CPU和GPU之间的垂直同步导致的，之所以会有两种参数，主要与项目是否开启多线程渲染有关。当开启多线程渲染时，看到就是Gfx.WaitForPresent；未开启多线程渲染时，看到的就是Graphics.PresentAndSync。

#### Gfx.WaitForPresent

当项目开启多线程渲染时，引擎会将Present等相关的工作尽可能放到渲染线程去执行，即主线程只需要通过指令调用渲染线程并让其进行Present，从而降低主线程的压力。但是当CPU希望进行Present操作时，需要等待GPU完成上一次的渲染，如果GPU的渲染压力很大，则CPU的Present操作将一直处于等待状态，其等待时间就是当前帧的Gfx.WaitForPresent时间


#### Graphics.PresentAndSync

同理，如果项目未开启多线程渲染时，引擎会在主线程进行Present，当然Present操作同样需要等待GPU完成上一次的渲染。如果GPU渲染开销很大，则CPU的Present操作将会一直处于等待操作，其等待时间就是当前帧的Graphics.PresentAndSync时间


如果游戏选择开启垂直同步，那么Gfx.WaitForPresent和Graphics.PresentAndSync也可能出现占用较高的情况，并且cpu在开销很低或者很高的时候这两个都会显示有较高的耗时

- CPU开销很低时：Present很早被执行，但此时屏幕的刷新周期还没到，就会出现较高的等待时间
- CPU开销很高时：Present很晚被执行，可能跟不上这次屏幕的刷新周期，要等到下一次屏幕刷新，从而导致较高的等待时间

总结一下导致Gfx.WaitForPresent和Graphics.PresentAndSync这两个参数CPU占用较高的原因有以下三点：

- CPU开销很低，所以CPU在等待GPU完成渲染或者等待VSync的到来
- CPU开销很高，使Present错过了当前帧的VSync，不得不等待下一次VSync的到来
- GPU开销很高，使CPU需要花费很多时间等待GPU的渲染完成



参考：[Profiler中WaitForTargetFPS详解_waitforfps-CSDN博客](https://blog.csdn.net/suifcd/article/details/50942686)
[常见性能分析器标记 - Unity 手册](https://docs.unity.cn/cn/2020.3/Manual/profiler-markers.html)