# Unity学习笔记

记录一下我的unity学习过程

## 目录：

1 [Monobehaviour的生命周期函数和事件函数](https://github.com/zhushouheng/Unity-learning/blob/main/note/Monobehaviour%E7%9A%84%E7%94%9F%E5%91%BD%E5%91%A8%E6%9C%9F%E5%87%BD%E6%95%B0%E5%92%8C%E4%BA%8B%E4%BB%B6%E5%87%BD%E6%95%B0.md)

2 [Component类](https://github.com/zhushouheng/Unity-learning/blob/main/note/Component%E7%B1%BB.md)

3[向量](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%90%91%E9%87%8F.md)

4[四元数和欧拉角](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%9B%9B%E5%85%83%E6%95%B0%E5%92%8C%E6%AC%A7%E6%8B%89%E8%A7%92.md)

5[物理系统](https://github.com/shishouheng/Unity-learning/blob/main/note/%E7%89%A9%E7%90%86%E7%B3%BB%E7%BB%9F.md)

6[通过虚拟轴和角色控制器实现第三视角移动](https://github.com/shishouheng/Unity-learning/blob/main/note/%E9%80%9A%E8%BF%87%E8%99%9A%E6%8B%9F%E8%BD%B4%E5%92%8C%E8%A7%92%E8%89%B2%E6%8E%A7%E5%88%B6%E5%99%A8%E5%AE%9E%E7%8E%B0%E7%AC%AC%E4%B8%89%E8%A7%86%E8%A7%92%E7%A7%BB%E5%8A%A8.md)

7[协程](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%8D%8F%E7%A8%8B.md)

8[相机、射线及unity常用坐标系](https://github.com/shishouheng/Unity-learning/blob/main/note/%E7%9B%B8%E6%9C%BA%E3%80%81%E5%B0%84%E7%BA%BF%E5%8F%8Aunity%E5%B8%B8%E7%94%A8%E5%9D%90%E6%A0%87%E7%B3%BB.md)

9[线渲染器Line Render](https://github.com/shishouheng/Unity-learning/blob/main/note/%E7%BA%BF%E6%B8%B2%E6%9F%93%E5%99%A8Line%20Render.md)

10[单例模式与Unity音频系统](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%8D%95%E4%BE%8B%E6%A8%A1%E5%BC%8F%E4%B8%8EUnity%E9%9F%B3%E9%A2%91%E7%B3%BB%E7%BB%9F.md)

11[对象池模式](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%AF%B9%E8%B1%A1%E6%B1%A0%E6%A8%A1%E5%BC%8F.md)

12[常用数据结构和方法总结](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%B8%B8%E7%94%A8%E6%95%B0%E6%8D%AE%E7%BB%93%E6%9E%84%E5%92%8C%E6%96%B9%E6%B3%95%E6%80%BB%E7%BB%93.md)

13[常用移动方式总结](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%B8%B8%E7%94%A8%E7%A7%BB%E5%8A%A8%E6%96%B9%E5%BC%8F%E6%80%BB%E7%BB%93.md)

14[Animation旧版动画系统](https://github.com/shishouheng/Unity-learning/blob/main/note/Animation%E5%8A%A8%E7%94%BB%E7%B3%BB%E7%BB%9F.md)

15[Animator新版动画系统](https://github.com/shishouheng/Unity-learning/blob/main/note/Animator%E6%96%B0%E7%89%88%E5%8A%A8%E7%94%BB%E7%B3%BB%E7%BB%9F.md)

16[NavMesh网格导航](https://github.com/shishouheng/Unity-learning/blob/main/note/NavMesh%E7%BD%91%E6%A0%BC%E5%AF%BC%E8%88%AA.md)

17[UI系统之UGU](https://github.com/shishouheng/Unity-learning/blob/main/note/UI%E7%B3%BB%E7%BB%9F%E4%B9%8BUGUI.md)

18[UGUI基础控件](https://github.com/shishouheng/Unity-learning/blob/main/note/UGUI%E5%9F%BA%E7%A1%80%E6%8E%A7%E4%BB%B6.md)

19[UGUI事件注册](https://github.com/shishouheng/Unity-learning/blob/main/note/UGUI%E4%BA%8B%E4%BB%B6%E6%B3%A8%E5%86%8C.md)

20[UI实现怪物展示(鼠标控制旋转、切换怪物、武器、服装、播放动画)](https://github.com/shishouheng/Unity-learning/tree/main/note/UI%E5%AE%9E%E7%8E%B0%E6%80%AA%E7%89%A9%E5%B1%95%E7%A4%BA)

21[UI框架](https://github.com/shishouheng/Unity-learning/tree/main/note/UI%E6%A1%86%E6%9E%B6)

22[NGUI](https://github.com/shishouheng/Unity-learning/blob/main/note/NGUI.md)

23[批处理和UI优化](https://github.com/shishouheng/Unity-learning/blob/main/note/%E6%89%B9%E5%A4%84%E7%90%86%E5%92%8CUI%E4%BC%98%E5%8C%96.md)

24[数据持久化](https://github.com/shishouheng/Unity-learning/blob/main/note/%E6%95%B0%E6%8D%AE%E6%8C%81%E4%B9%85%E5%8C%96.md)

25[XML框架实现答题系统](https://github.com/shishouheng/Unity-learning/tree/main/note/XML%E6%A1%86%E6%9E%B6%E5%AE%9E%E7%8E%B0%E7%AD%94%E9%A2%98%E7%B3%BB%E7%BB%9F)

26 [塔防游戏](https://github.com/shishouheng/Unity-learning/tree/main/note/%E5%A1%94%E9%98%B2%E6%B8%B8%E6%88%8F/Project)

27[背包商城系统](https://github.com/shishouheng/Unity-learning/tree/main/note/Sqlite%E5%AE%9E%E7%8E%B0%E8%83%8C%E5%8C%85%E3%80%81%E8%A3%85%E5%A4%87%E3%80%81%E5%95%86%E5%9F%8E%E7%B3%BB%E7%BB%9F)

28[迭代器](https://github.com/shishouheng/Unity-learning/blob/main/note/%E8%BF%AD%E4%BB%A3%E5%99%A8.md)

29[玩家动画状态机+敌人状态状态机](https://github.com/shishouheng/Unity-learning/tree/main/note/%E7%8E%A9%E5%AE%B6%E5%8A%A8%E7%94%BB%E7%8A%B6%E6%80%81%E6%9C%BA%2B%E6%95%8C%E4%BA%BA%E7%8A%B6%E6%80%81%E7%8A%B6%E6%80%81%E6%9C%BA)

30[相机跟随策略](https://github.com/shishouheng/Unity-learning/blob/main/note/%E6%91%84%E5%83%8F%E6%9C%BA%E8%B7%9F%E9%9A%8F%E7%AD%96%E7%95%A5.md)

31[导航状态机](https://github.com/shishouheng/Unity-learning/tree/main/note/%E5%AF%BC%E8%88%AA%E7%8A%B6%E6%80%81%E6%9C%BA)

32[AStar寻路算法](https://github.com/shishouheng/Unity-learning/blob/main/note/AStar%E7%AE%97%E6%B3%95.md)

33[动画状态机+顿帧效果 ](https://github.com/shishouheng/Unity-learning/tree/main/note/%E5%8A%A8%E7%94%BB%E7%8A%B6%E6%80%81%E6%9C%BA%2B%E6%89%93%E5%87%BB%E6%84%9F%E6%95%88%E6%9E%9C)

34[EasyTouch触控插件和KGFMapSystem小地图插件](https://github.com/shishouheng/Unity-learning/blob/main/note/EasyTouch%E8%A7%A6%E6%8E%A7%E6%8F%92%E4%BB%B6%E5%92%8CKGFMapSystem%E5%B0%8F%E5%9C%B0%E5%9B%BE%E6%8F%92%E4%BB%B6.md)

35[DoTween插件](https://github.com/shishouheng/Unity-learning/blob/main/note/DoTween%E6%8F%92%E4%BB%B6.md)

36[特性和反射](https://github.com/shishouheng/Unity-learning/blob/main/note/%E7%89%B9%E6%80%A7%E5%92%8C%E5%8F%8D%E5%B0%84.md)

37[AssetBundle](https://github.com/shishouheng/Unity-learning/blob/main/note/AssetBundle.md)

38[Lua](https://github.com/shishouheng/Unity-learning/blob/main/note/Lua.md)

39[Lua与C Sharp对比](https://github.com/shishouheng/Unity-learning/blob/main/note/Lua%E4%B8%8EC%20Sharp%E5%AF%B9%E6%AF%94.md)

40[XLua](https://github.com/shishouheng/Unity-learning/blob/main/note/XLua.md)

41[热更新](https://github.com/shishouheng/Unity-learning/blob/main/note/%E7%83%AD%E6%9B%B4%E6%96%B0.md)

42[检查版本号并进行游戏热更新](https://github.com/shishouheng/Unity-learning/blob/main/note/%E6%A3%80%E6%9F%A5%E7%89%88%E6%9C%AC%E5%8F%B7%E5%B9%B6%E8%BF%9B%E8%A1%8C%E6%B8%B8%E6%88%8F%E6%9B%B4%E6%96%B0.md)

43[Sokect网络交互](https://github.com/shishouheng/Unity-learning/blob/main/note/Socket%E7%BD%91%E7%BB%9C%E4%BA%A4%E4%BA%92.md)

44[网络同步](https://github.com/shishouheng/Unity-learning/blob/main/note/%E7%BD%91%E7%BB%9C%E5%90%8C%E6%AD%A5.md)

45[异步非阻塞Socket交互](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%BC%82%E6%AD%A5%E9%9D%9E%E9%98%BB%E5%A1%9ESocket%E4%BA%A4%E4%BA%92.md)

46[数据结构](https://github.com/shishouheng/Unity-learning/blob/main/note/%E6%95%B0%E6%8D%AE%E7%BB%93%E6%9E%84.md)

47[设计模式](https://github.com/shishouheng/Unity-learning/blob/main/note/%E8%AE%BE%E8%AE%A1%E6%A8%A1%E5%BC%8F.md)

48[通过状态模式和管理者模式搭建游戏框架](https://github.com/shishouheng/Unity-learning/blob/main/note/%E9%80%9A%E8%BF%87%E7%8A%B6%E6%80%81%E6%A8%A1%E5%BC%8F%E5%92%8C%E7%AE%A1%E7%90%86%E8%80%85%E6%A8%A1%E5%BC%8F%E6%90%AD%E5%BB%BA%E6%B8%B8%E6%88%8F%E6%A1%86%E6%9E%B6.md)

49[shader入门](https://github.com/shishouheng/Unity-learning/blob/main/note/Shader%E5%85%A5%E9%97%A8.md#%E4%B8%89%E8%A1%A8%E9%9D%A2%E7%9D%80%E8%89%B2%E5%99%A8%E4%BB%A3%E7%A0%81%E7%BB%93%E6%9E%84)

50[顶点与片元着色器](https://github.com/shishouheng/Unity-learning/blob/main/note/%E9%A1%B6%E7%82%B9%E4%B8%8E%E7%89%87%E5%85%83%E7%9D%80%E8%89%B2%E5%99%A8.md)

51[渲染流水线](https://github.com/shishouheng/Unity-learning/blob/main/note/%E6%B8%B2%E6%9F%93%E6%B5%81%E6%B0%B4%E7%BA%BF.md)

52[光照](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%85%89%E7%85%A7.md)

53[卡通着色](https://github.com/shishouheng/Unity-learning/blob/main/note/%E5%8D%A1%E9%80%9A%E7%9D%80%E8%89%B2.md)

54[游戏优化](https://github.com/shishouheng/Unity-learning/blob/main/note/%E6%B8%B8%E6%88%8F%E4%BC%98%E5%8C%96.md)

55[面试问题总结](https://github.com/shishouheng/Unity-learning/blob/main/note/%E9%9D%A2%E8%AF%95%E9%97%AE%E9%A2%98%E6%80%BB%E7%BB%93.md)

56[百度定位SDK接入]([Unity-learning/note/Unity中SDK接入.md at main · shishouheng/Unity-learning (github.com)](https://github.com/shishouheng/Unity-learning/blob/main/note/Unity%E4%B8%ADSDK%E6%8E%A5%E5%85%A5.md))