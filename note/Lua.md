# Lua简介

Lua是一种轻量小巧的脚本语言，用标准C语言编写，设计目的是为了嵌入应用程序中，从而为应用程序提供灵活的扩展和定制功能

**Lua特性：**
- 轻量级：编译后仅100多K,可以方便的嵌入程序中
- 可扩展：扩展接口的机制简单方便，由宿主语言提供功能
- 支持函数式编程和面向对象编程
- 自动内存管理
- 通用类型——table，可以用它来实现数组、哈希表、集合、对象
- 函数可以当作一个值来使用
- 闭包和table可以用来实现面向对象的核心机制（抽象、虚函数、继承、重载等）
- 提供多线程（协同程序）

# 一、基本数据类型

- 1.**nil:** 只有nil属于这个类，表示false或者null
- 2.**bollean：** 布尔值，包含true和false
- 3.**number：** 整数或小数都可以通过number表示
- 4.**string：** 字符串，可以通过单引号或者双引号表示
- 5.**function：** 在lua中方法也是一种数据类型，由c或lua编写的方法都是function类型，lua中方法可以具有多个返回值
- 6.**userdata：** 表示任意存储在变量中的c数据结构，允许我们在lua中绑定c或者c++编写的数据结构和功能以便在lua中直接使用
-  **7.thread：** 表示执行的独立协程
- **8.table：** 本质是一个关联数组，是lua中的一种数据结构，可以配合table自定义其他不同的数据类型

## 1. nil(空)
- nil类型表示一种没有任何有效值，该类只有nil一个值，如打印一个没有赋值的变量就会输出一个nil
- 对于全局变量和table，nil还有删除的作用，给全局变量或者table表中的变量赋nil等同于把它们删掉

```lua
print(a)--打印未赋值的变量——输出nil

a=5;
print(a)--输出5
a=nil
print(a)--输出nil
```

## 2.boolean（布尔）
- 除了true和false，lua还把nil看作是false

```lua
if nil then
print("1")
else
print("2")--打印该值
end
```

## 3.number（双精度浮点数double）

- 在lua中不管是整数还是小数都会被看作是number类型，number类型本质是一个双精度浮点数
```lua
print(type(1))--number
print(type(2.3))--number
```

## 4.string(字符串)

- 字符串由一对双引号或单引号表示
- 也可以用两个方括号[[]]来表示长字符串，可以跨越多行并且不需要单引号或双引号
- 在对一个数字字符串进行算术操作时会尝试将这个数字字符串转为一个数字
- 使用#来计算字符串的长度，放在字符串前面
- 通过..两个点的方式来连接字符串

```lua
print("abcc")--abcc
print('bdadfafda')--bdadfafda
print
[[
Hello
World
]]--Hello
  --World 


str="10"
num=str+1
print(num)--11
print(#str)--str的长度2

str="Hellp"
str1="World"
print(str..str1)--Hello World
```

## 5.function（方法）

```lua
--返回较大值
function fun(num1,num2)

local result

if num1>num2 then
result=num1
else
result=num2
end
return result
end

print("较大值是：",fun(5,6))
print("较大值是：",fun(1,7))



--定义方法的方式1
myFun=function(param)
print("MyFunPrint:",param)
end

myFun("game")--调用方法，输出“MyFunPrint：game”


function Add(num1,num2,Fun)--最后一个参数是把方法作为参数
local result=num1+num2
Fun(result)
end

Add(5,6,myFun)--输出“MyFunPrint:11”

Add(2,4,function(p)--类似于lambda表达式的写法
	print ("just like lambda",p)
	end)



--lua中函数可以具有多个返回值，当调用一个返回多个值的函数时可以使用多个变量来接受这些返回值
function Fun()
return 0,1,2,true,4,"hello"
end

a,b,c,d,e,f,g=Fun()
print(a,b,c,d,e,f,g)--依次打印出0,1,2,true,4,hello,nil


--求数组最大值和最大值下标方法
function max(a)--a是一个数组
local mi=1--最大值索引
local m=a[mi]--最大值

for i,val in ipairs(a) do
if val>m then
mi=i
m=val
end
end
return mi,m
end

print(max({8,21,14,33,24,31}))


--lua中传入参数时输入...可传入不定长度的参数列表
function average(...)
local arg={...}

local result=0;
for i, val in inpairs(arg) do
result=result+val
end
print("一共传递了"..#arg.."个参数")
return result/#arg
end

print("平均数：",average(12,22,12,32,35))

```

## 6.userdata


## 7.thread


## 8.table(表)

table是lua的一种数据结构，可以配合table自定义其他不同的数据类型
```lua
mytable={}--构造表
mytable[1]='lua'--指定键值对：1==》lua
mytable['a']=123
print(mytable[1])--打印键1对应的值
print(mytable['a']) --打印键a对应的值
mytable=nil --移除引用，垃圾回收释放内存
```

在lua中由于函数也是一种特殊的数据类型，所以可以存储在变量中作为参数传递给其他函数，因此也可以创建一个table来包含多个函数，这样就形成了一个函数的集合

```lua
--方式一

Lib={}

Lib.foo=function(x,y) return x+y end
Lib.goo=function(x,y) return x-y end

print(Lib.foo(1,2))
print(Lib.goo(4,1))

--方式二

Lib2={
	  fun1=function(x,y) return x+y end,
      fun2=function(x,y) return x-y end
	 }

	 print(Lib2.fun1(1,2))
	 print(Lib2.fun2(3,3))


--方式三
Lib3={}
function Lib3.fun3(x,y)
	 return x+y
end

function Lib3.fun4(x,y)
     return x-y
end

print(Lib3.fun3(1,2))
print(Lib3.fun4(7,2))

```
# 二、运算符

## 算数运算符

lua中的算术运算符基本与编程语言中一致，加减乘除取余，但也存在特有的
如：
- ^表示乘幂，如10^2的结果是100
- -除了表示减法还表示负号，如a=10，则-a=-10
- //整除运算符，如5//2=2


## 关系运算符

lua中的关系运算符与c#中唯一不同的是不等于

通过~=表示，如果两个不相等返回true，否则返回false


## 逻辑运算符

假如a=true，b=false
- 逻辑与：用and表示，（a and b）为false
- 逻辑或：用or表示，（a or b）为true
- 逻辑非：用not表示，not a为false，not b为true


## 其他运算符

- `..`:用来连接两个字符串
- #： 一元运算符，返回字符串或者表的长度
# 三、数组

lua中的数组同样分为一维数组和多维数组，但是lua中的数组大小可以是不固定的并且数组的首元素索引从1开始

### 一维数组：
```lua
array={'lua','c#'}

for i=1,#array do --从1开始遍历到array的长度
print(array[i])
end


--给数组赋值并打印
array2={}
for i=1,4 do
array2[i]=i*2
end

for i=1,#array2 do
print(array2[i])
end
```

### 二维数组;

```lua
array={}

for i=1,3 do
array[i]={}
for j=1,3 do
array[i][j]=i*j
end
end

for i=1,#array do
for j=1,#array do
print(array[i][j])
end
end
```


# 四、模块

在lua中，模块是一种组织和封装代码的方式，类似于一个封装库，把一些公用的代码放在一个文件里，以API接口的形式在其他地方调用，有利于代码的重用和降低代码耦合度。

lua中的模块是由变量、函数等已知元素组成的table，创建一个模块就是创建一个table，然后把需要导出的常量、函数放入其中，最后返回这个table即可

例：
重新创建一个lua文件并在此文件内创建方法和变量
```lua
local module={}

module.val="模块变量"

function module.fun1()
     print("这是一个公有函数")
end

local function fun2()--私有函数不能通过点的方式来指定函数所属
     print("这是一个私有函数")
end

return module
```

在另一个文件内通过require加载到该模块，并调用该模块内的方法
```lua
local m=require("module")
print(m.val)
m.fun1()
```


# 五、 迭代器

迭代器的作用是遍历标准模板库容器中的部分或者全部元素

## 1.泛型for迭代器

泛型for在自己内部保存迭代函数（即pairs和ipairs），实际上它保存三个值
- 迭代函数：重复执行指令的算法逻辑，包含了如何遍历集合的代码
- 状态常量：循环过程中不会改变的常量
- 控制遍历：即当前索引的下标（key）

例：
```lua
table={1,2,3,4}

for k,v in ipairs(table) do
	print(k,v)
end


for k,v in pairs(table) do
	print(k,v)
end

--这两种迭代函数对于数组形式的遍历没有任何区别，输出的内容都是
--1  1
--2  2
--3  3
--4  4
```


```lua
table={[0]=1,2,[-1]=3,4,5}

for k,v in ipairs(table) do
	print(k,v)
end
--1  2
--2  4
--3  5

--由此可发现ipairs是从索引1开始遍历的，小于等于0的值无法得到并且只能找到连续索引的键，如果中间断序遇到nil了则直接退出

print('-------------------')

for k,v in pairs(table) do
	print(k,v)
end
--1  2
--2  4
--3  5
--0  1
---1  3

--而pairs用于元素迭代时可迭代任意泛型集合并且可以返回nil

```

## 2.无状态迭代器和多状态迭代器

- **无状态迭代器：** 不需要保存当前的迭代位置，每次调用都会从头开始遍历，如ipairs就是一个无状态迭代器，每次调用都会自动的从第一个元素开始迭代到最后一个元素，不会记录当前的迭代位置（无状态迭代器需要两个参数实现，即当前索引和最大索引）。
- **多状态迭代器：** 需要记录当前的迭代位置，每次调用时都会从上一次结束的地方继续遍历，lua中并没有提供直接的多状态迭代器，但是可以通过使用闭包或自定义状态变量实现多状态迭代器的效果


### 无状态迭代器
案例:自己定义无状态迭代器实现返回索引的两倍数
```lua
--iteratorCount是最大索引即状态常量，current是当前索引即控制变量
function double(iteratorCount,current)
	if current<iteratorCount then
		current=current+1
		return current,current*2
	end
end

for k,v in double,10,0 do
	print(k,v)
end
```

案例：实现自定义ipairs
```lua
function iter(a,i)
	i=i+1
	local v=a[i]
	if v then
		return i,v
	end
end


function ipairsSelf(a)
	return iter,a,0
end

mytable={12,14,51,31,11,42}

for k,v in ipairsSelf(mytable) do
	print(k,v)
end
--输出
--1 	12
--2 	14
--3 	51
--4 	31
--5 	11
--6 	42
```

### 多状态迭代器

实现多状态迭代器需要使用闭包（closure）
#### 闭包：
闭包（closure）一般用于匿名函数或者嵌套函数，由一个函数和该函数能访问到的非局部变量组成，可以理解为函数A可以返回函数B，此时函数B可以访问函数A中的变量，这个变量就是非局部变量（不是函数B的局部变量，也不是全局变量），也叫upvalue

```lua
function A(a)
	local x=a
	--闭包，改变了传入参数的生命周期
	return function(y)
		print(x+y)  
	end
end

B=Fun1(5) --此时B就是A中返回的函数
B(4)  --输出9
```

案例：闭包实现多状态迭代器
```lua
function iterator(array)
	local index=0
	local count=#array

	--闭包
	return function()
		index=index+1
		if(index<=count) then
			return array[index]
		end
	end
end

mytable={21,22,65,51,45}

for data in iterator(mytable) do
	print(data)
end
```



# 六、Lua协同程序

协同程序是一种特殊的函数，可以在执行过程中暂停并在之后恢复执行，可以帮助我们更好的组织和管理程序的逻辑流程

##  1.基本语法

- `coroutine.creat():` 创建coroutine，会返回一个coroutine，参数是一个函数
- `coroutine.resume():` 重启或开启coroutine
- `coroutine.yield():` 挂起coroutine
- `coroutine.status():` 查看coroutine的状态（dead，suspended，running）
- `coroutine.wrap():` 创建coroutine，会返回一个函数，调用这个函数就进入coroutine
- `coroutine.running():` 返回处于running状态的线程

```lua
--协程的创建
function fun()
	print(100)
end

co=coroutine.create(fun)
print(type(co))--thread

co2=coroutine.wrap(fun)
print(type(co2))--function

coroutine.resume(co)--重启/开启协程，此时执行fun输出100
```

```lua
co=coroutine.create(
	function()
		for x=1,5 do
			print('第'..x..'次循环')
			if x==3 then
				print(coroutine.status(co))
				print(coroutine.running())
			end
			coroutine.yield()
		end
	end
)
coroutine.resume(co)--第一次循环
coroutine.resume(co)--第二次循环
coroutine.resume(co)--第三次循环  running thread：00A6D750
coroutine.resume(co)--第四次循环
coroutine.resume(co)--第五次循环
coroutine.resume(co)--无，在第五次循环时函数执行完毕，协程死亡
print(coroutine.status(co))--dead
```

## 2.意义

```lua
function fun(a)
	print('fun得到的参数是：',a)
	return coroutine.yield(5*a)
end

co=coroutine.create(
	function(a,b)
		print('第一次输出:',a,b)
		local r=fun(a+1)

		print('第二次输出：',r)

		local r,s=coroutine.yield(a+b,a-b)
		print('第二次输出',r,s)
		return b
	end
)

print(coroutine.resume(co,1,10))
--第一次输出:	1	10
--fun得到的参数是：	2
--true	10
--由上面的输出可以看出除了执行协程里的print还会返回一个true和10，协程成功唤醒会返回一个true
--10则是由协程挂起后返回的5*a的值
print(coroutine.resume(co,'666'))
--第二次输出：	666
--true	11	-9
--r变为666的原因是上一次协程在yield时暂停，这次开启并传入了666，然后yield会将666返回出来复制给r
--true表示协程开启成功
--11和-9表示遇到yield时返回出来的a+b和a-b
```



案例：生产者与消费者问题
```lua
--生产者的协同程序，初始为挂起状态
local productor=coroutine.create(
	function(money,goods)
		for x=1,100 do
			print('商家收款'..money..'元,订单是一杯'..goods)
			print('商家开始制作...')
			set(goods)
		end
	end
)

--生成奶茶----->传递给消费者----->自身继续挂起
function set(goods)
	coroutine.yield(goods)
end

--消费者
function customer()
	for x=1,100 do
		local i,j=Get(x)
		if i then
			print('顾客取走一杯'..j)
		end
	end
end

function Get(x)
	print('第'..x..'位客人来了')
	return coroutine.resume(productor,20,'珍珠奶茶')
end

customer()
```

**`通过在主线程中调用resume和在协程中调用yield，我们可以实现在主线程和协程之间进行状态的传递，通过resume将外部的状态传递到协程内，通过yield将协程内部的状态传递到主线程`**



# 七、元表

## 1.概念

在lua的table中我们可以访问对应的key来得到value值，但是无法对两个table进行操作（如加减乘除），因此lua提供了元表，允许我们改变table的行为，每个行为关联了对应的元方法，例如我们可以使用元表定义两个table的相加操作a+b

当lua试图对两个表进行相加时会先检查两者之一是否有元表，之后检查是否有一个叫`__add` 的字段,若找到，则调用对应的值。`__add` 等即时字段对应的值就是元方法

- **`setmetatable(table,metatable):`** 对指定table设置元表（metatable）
- **`getmetatable(table):`** 返回对象的元表(metatable)


## 2.__index元方法

这是metatable最常用的键，当通过键来访问table时，如果这个键没有值，那么lua就会寻找该table的metatable中的__index键。如果__index包含一个表格，lua会在表格中查找相应的键


使用元表中的__index指向元函数
```lua
--普通表
table1={'c#','lua','c++'}

--元表
metatable1={
	__index=function(tab,key)
		print(key)
		return tab[1]
	end
}

table1=setmetatable(table1,metatable1)

--正常访问，key有效，直接得到value
print(table1[2])--lua

--key无效，触发__index元事件
print(table1['gf'])--gf  c#
print(table1['fs'])--fs  c#
print(table1['qwe'])--qwe  c#
```


使用元表中的__index指向另一张表
```lua
--普通表
table1={'c#','lua','c++')
table2={'www','fff','aaa','vvv'')

--元表
metatable1={
	__index=table2
}

table1=setmetatable(table1,metatable1)

print(table1[1])--c#
print(table1[4])--vvv,table1中没有就去table2中查找
```

可以得出Lua查找一个表元素的规则就是如下三个步骤
- 1.在表中查找，如果找到，则返回该元素，找不到则继续
- 2.判断该表是否有元表，如果没有元表，则返回nil，有元表则继续
- 3.判断元表有没有__index方法，如果__index方法为nil，则返回nil，如果__index方法是一个表，则重复1、2、3；如果__index方法是一个函数，则返回该函数的返回值


## 3.__newindex元方法

__newindex元方法用来对表进行更新，__index则用来对表进行访问
当给表中一个不存在的索引赋值时，解释器就会查找__newindex元方法，如果存在则调用这个函数但不对该表进行赋值，而是对__newindex中指定的表的索引进行赋值

```lua
--普通表
table1={'c#','lua','c++'}
table2={}

--元表
metatable1={
	__newindex=table2
}

table1=setmetatable(table1,metatable1)

table1[1]=1
table1[3]=3
table1[5]=5

print(table1[1])--1
print(table1[3])--3
print(table1[5])--nil

print(table2[1])--nil
print(table2[3])--nil
print(table2[5])--5
```


## 4.__tostring元方法

__tostring元方法用于修改表的输出行为

```lua
mytable=setmetatable({1,2,3,4},{
	__tostring=function(mytable)
	sum=0
	for k,v in pairs(mytable) do
		sum=sum+v
	end
	return '所有元素的和为'..sum
end
})

print(mytable)--所有元素的和为10
```

## 5.__call元方法

__call元方法允许我们将一个表当作函数使用，当lua试图调用一个值时，如果该值有元表，并且元表有__call元方法，那么就会调用__call元方法

```lua
local A={}
local metaA={
	__call=function(...)
		print('call A with args:',...)
	end
}

setmetatable(A,metaA)

A('string',1,true)--call A with args:	table: 00C69428	string	1	true

```

当我们像调用函数一样调用表A时，lua会查找A的元表，并尝试调用其中的__call元方法，最后打印出       call A with args:	table: 00C69428	string	1	true


## 6.__add元方法

__add元方法允许我们定义两个表之间的加法操作，当对两个表进行加法操作时，会检查二者之一是否有元表，并且元表中是否有__add的字段，如果找到了，那么lua就会调用这个字段对应的函数，并将这两个相加的表作为参数传递给这个函数

```lua
local A={1,2,3}
local B={4,5,6}
local metaA={
	__add=function(op1,op2)
		local result={}
		for _,v in pairs(op1) do
			result[v]=true
		end
		for _,v in pairs(op2) do
			result[v]=true
		end
		return result
	end
}

setmetatable(A,metaA)

local C=A+B
for k,v in pairs(C) do
	print(k,v)
end
```

## 7.其他元方法

- **`__sub:`** 对应运算符-
- **`__mul:`** 对应运算符*
- **`__div:`** 对应运算符/
- **`__mod:`** 对应运算符%
- **`__unm:`** 对应运算符-
- **`__concat:`** 对应运算符...
- **`__eq:`** 对应运算符==
- **`__lt:`** 对应运算符<
- **`__le:`** 对应运算符<=


# 八、面向对象

lua本身没有面向对象的概念，但是通过table的各种用法，可以模拟出类似面向对象的特性。再lua中我们可以使用table和元表来实现面向对象编程的一些基本概念，如封装、继承和多态

```lua
class1={x=0,y=0,z=0}
class1.__index=class1--查不到得元素从class1中查找

--构造函数，通过冒号语法可以将调用对象作为第一个参数传入即相当于class1.new(class1,x,y)
function class1:new(x,y)
	local t={}
	t.x=x or 0
	t.y=y or 0
	t.z=x+y
	setmetatable(t,class1)--将t的元表设置为class1
	return t
end

function class1:printZ()
	print('x和y的和是：',self.z)
end


local r=class1:new(4,3)
print(r.x)--4
r:printZ()--7

local r2=class1:new(8,9)
print(r2.y)--9
r2:printZ()--17
```