
# 1.闭包

声明在一个函数内部的函数叫做闭包函数。其中内部函数总是可以访问其所在的外部函数中声明的参数和变量，即时外部函数已经被返回。

```python
def outer(x):
	def inner(y):
		return x+y
	return inner

print(outer(4)(3))
---------------------
>>>7
```

如代码所示，在outer函数内部定义了一个inner函数，并且inner函数引用了外部函数outer的变量x，这就是一个闭包。在输出时，outer(4)(3)第一个括号传入的值会返回inner函数，即返回了4+y
，此时在传第二个参数进去，就可以得到返回值4+3


## 2.装饰器

python中的装饰器就是闭包的一种应用。装饰器可以拓展原来函数功能，可以在不修改原函数的代码前提下给函数增加新的功能。使用时只需要在函数上加上@函数名即可

```python
def debug(func):
	def wrapper():
		print('[DEBUG]:enter{}()'.format(func.__name__))
		return func()
	return wrapper


@debug
def hello():
	print('hello')

hello()
-------------------
>>>[DEBUG]:enter hello()
>>>hello
```

就这样在不修改原函数的情况下实现了对函数功能的扩展。
其中这种写法等同于：
```python
def debug(func):
	def wrapper():
		print('[DEBUG]:enter{}()'.format(func.__name__))
		return func()
	return wrapper


def hello():
	print('hello')

hello=debug(hello)

hello()
--------------------
>>>[DEBUG]:enter hello()
>>>hello
```

所以说在python中装饰器其实就是一种语法糖，提供了简洁的语法来修改或扩展函数的行为


## 3.带参数的装饰器

```python
def logging(level):
	def outwrapper(func):
		def wrapper(*args,**kwargs):
			print('[{0}]:enter {1}()'.format(level,func.__name__))
			return func(*args,**kwargs)
		return wrapper
	return outwrapper

@logging(level='INFO')
def hello(a,b,c)
	print(a,b,c)

hello('hello','good','morning')
----------------------
>>>[INFO]:enter hello()
>>>hello good morning
```