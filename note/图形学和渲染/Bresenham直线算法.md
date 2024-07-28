
Bresenham算法是一种用于在二维图形中绘制直线的整数算法。由Jack Bresenham在1962年发明，主要用于在计算机图形学中，解决了低分辨率显示器上高效绘制直线的问题。这个算法的主要优点是只使用整数计算，因此速度快且高效。


## 1.算法思想：

屏幕能显示图像的原因是因为屏幕是由一个一个的像素点组成的，像素是屏幕显示图像的最小单位，它们在屏幕中排列成行和列的网格状结构，并且每个像素可以调整其颜色以实现显示特定形状、颜色的图形。比如我们想在屏幕上绘制一个黑色正方形时，实际就是将这个正方形大小的区域内的所有像素设置为黑色。

对于像正方形这种很规则的图形可以直接无脑的将其大小内部的像素改变颜色，但是对于显示其他比较特殊的图形可能就需要一些策略了，如三角形、圆形、斜线等，这里就以最简单的斜线为例。

在屏幕上画一条斜线我们需要知道线段的起点和终点（先假设斜率0<k<1），然后先将起点和终点设置为我们想要的颜色，如下：

![[StartEnd.jpg]]

起点和终点的像素确定后，我们发现理论直线还会经过1234这四个像素点，但我们该如何确定选择哪个像素点作为我们需要的呢？这里我们就需要用到Bresenham算法的思想了。

![[ChoosePixel.jpg]]

因为斜率在0到1之间，所以我们能确定下一个点的位置的x值一定是加1的，然后此时直线经过了1和3两个像素点，我们可以选取3和1像素点的中心点P<sub>1</sub> 和P<sub>2</sub>，然后分别取得它们到直线的距离D<sub>1</sub> 和D<sub>2</sub>，如果D<sub>1</sub> <D<sub>2</sub>我们就选取P<sub>1</sub> 所在的像素点为下一个像素点，否则选择P<sub>2</sub>所在像素点为下一个像素点。即：
- D<sub>1</sub> <D<sub>2</sub>=>P<sub>1</sub>
- D<sub>1</sub> >D<sub>2</sub>=>P<sub>2</sub>

这就是bresenham的核心原理


## 2.算法推导

首先我们知道直线的计算公式为y=kx+b，可以根据x坐标求出直线上某点的位置。
因为画一条线段，起点和终点是已知的，我们假设起点的坐标为(x<sub>i</sub>,y<sub>i</sub>)，则：

$\because$  起点的坐标为(x<sub>i</sub>,y<sub>i</sub>)

$\therefore$ P<sub>1</sub>的坐标为(x<sub>i</sub>+1,y<sub>i</sub>)，P<sub>2</sub>的坐标为(x<sub>i</sub>+1,y<sub>i</sub>+1)

$\therefore$  D<sub>1</sub> =k(x<sub>i</sub>+1)+b-y<sub>i</sub>;          D<sub>2</sub> =y<sub>i</sub>+1-k(x<sub>i</sub>+1)-b

$\therefore$  D<sub>1</sub> -D<sub>2</sub>= 2k(x<sub>i</sub>+1)-2y<sub>i</sub>+2b-1

$\because$   斜率k=$\Delta$y/$\Delta$x

$\therefore$  D<sub>1</sub> -D<sub>2</sub>= 2$\Delta$y/$\Delta$x(x<sub>i</sub>+1)-2y<sub>i</sub>+2b-1

$\therefore$ $\Delta$x(D<sub>1</sub> -D<sub>2</sub>)=2$\Delta$y(x<sub>i</sub>+1)-2y<sub>i</sub>$\Delta$x+2b$\Delta$x-$\Delta$x（这一步的作用是为了消除浮点数，将除法变为乘法）

为了简化公式量我们设$\Delta$x(D<sub>1</sub> -D<sub>2</sub>)=d<sub>i</sub>
(因为x肯定会递增，即$\Delta$x肯定大于0，所以d<sub>i</sub>的正负还是由D<sub>1</sub> -D<sub>2</sub>决定，我们通过d<sub>i</sub>的正负就能判断D<sub>1</sub> -D<sub>2</sub>的正负就能判断谁离直线更近)

$\therefore$ d<sub>i</sub>=2$\Delta$yx<sub>i</sub>-2y<sub>i</sub>$\Delta$x+C(因为2b$\Delta$x-$\Delta$x都是常数，为了公式更直观直接用C来表示它们的值)

$\therefore$ d<sub>i+1</sub>=2$\Delta$yx<sub>i+1</sub>-2y<sub>i+1</sub>$\Delta$x+C

当我们斜率在0到1之间时，步进方向是在x轴上的（大于0时步进方向在y轴上），步进方向可以帮我们判断像素的前进方向。

![[StepDirection.jpg]]

当步进方向是在x轴上的时候（即斜率在0和1之间），很明显可以知道x<sub>i+1</sub>=x<sub>i</sub>，但y<sub>i+1</sub>会有两种可能，即
- y<sub>i+1</sub>=y<sub>i</sub>(此时D<sub>1</sub> <D<sub>2</sub>,并且d<sub>i</sub><0,参考图二)
- y<sub>i+1</sub>=y<sub>i</sub>+1(此时D<sub>1</sub> >D<sub>2</sub>,并且d<sub>i</sub>>0,参考图二)

这样我们就能根据当前像素点的位置确定下一个像素点的位置，然后我们只需要在程序开始时给定起点和终点的位置，程序一直循环，并根据起点的位置判断下一个像素点的位置，一直循环到终点即可通过像素画出一条直线