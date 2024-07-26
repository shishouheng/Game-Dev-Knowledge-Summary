
## 1.介绍

在c++和c#中均有const关键字，用来表示某个值是不可变的，使用了const关键字就相当于指定了一个语义约束，编译器会强制实施这个约束，在开发者尝试修改这个值的时候编译器就会报错来提示开发者这个值是不可改变的。

## 2.C++与C# const的区别

### 2.1 C#中的const

- 在c#中，const关键字用于定义编译时常量，这些常量在编译时其值必须确定，并且在程序运行时不能改变，并且只能用于基本数据类型或字符串
```c#
public class Example
{
    public const int MyConstant = 42; // 编译时常量
    public const string Greeting = "Hello, World!";
}

```
- c#中const关键字也可以用于方法内部的局部变量
```c#
public void MyMethod()
{
    const int LocalConstant = 10;
    Console.WriteLine(LocalConstant);
}

```
- 在c#中const只能修饰字段和局部变量，不能修饰方法参数、返回值或成员函数

### 2.2 C++中的const

- 在c++中，const可以像c#中的const一样用来定义常量，它们在初始化后不能改变
```cpp
const int MyConstant=42;
const std::string Greeting="Hello World";
```
- 修饰指针，用于指针和引用，指定指针指向的值或者指针本身是不可变的
```cpp
const int* ptr=&MyConstant;//指针指向的值是常量
int* const ptr2=&MyConstant;//指针本身是常量
const int* const ptr3=&MyConstant;//指针和指向的值都是常量
```
- 修饰函数参数，表示该参数在函数内部不能被修改
```cpp
void PrintValue(const int value)
{
	//value=100;尝试修改时编译器会直接报错
	std::cout<<value<<std::endl;
}
```
- 修饰函数返回值，防止对返回值进行修改
```cpp
const std::string& GetGreeting()
{
	return Greeting;
}
```
- 修饰成员函数，表示该函数不会修改类的成员变量
```cpp
class MyClass
{
	public:
		int GetValue() const
		{
			return value;
		}
	private:
		int value;
};
```



## 3.C++ const详解

### 3.1 const 修饰普通类型变量

```cpp
const int  a = 7; 
int  b = a; // 正确
a = 8;       // 错误，不能改变
```

a 被定义为一个常量，并且可以将 a 赋值给 b，但是不能给 a 再次赋值。对一个常量赋值是违法的事情，因为 a 被编译器认为是一个常量，其值不允许修改。

接着看如下的操作：

```cpp
#include<iostream>
 
using namespace std;
 
int main(void)
{
    const int a = 7;   // 定义常量变量 a
    int *p = (int*)&a; // 将常量变量 a 的地址强制转换为 int* 类型的指针
    *p = 8;            // 通过指针 p 修改 a 的值
    cout << a;         // 输出 a 的值
    system("pause");   // 暂停系统以查看输出
    return 0;
}

```

这段代码试图通过指针修改const变量的值，虽然在标准c++中是未定义行为，但在许多编译器上实际可以改变内存中的值，但如果编译器的优化级别较高，编译器可能会优化掉这种未定义行为，即
**即使内存中的值被修改为了8，但编译器会假设const的值不会被修改，因此直接输出7或者引发错误**

### 3.2 const修饰指针变量

const修饰指针变量会有以下三种情况：
- const修饰指针指向的内容，则内容为不可变量
- const修饰指针，则指针为不可变量
- const修饰指针和指针指向的内容，则指针和指针指向的内容都为不可变量

对于第一种情况
```cpp
const int *p=8;
```
此时指针指向的内容8为不可变量，因为const修饰的是\*p

对于第二种情况
```cpp
int a = 8;
int* const p = &a;
*p = 9; // 正确
int  b = 7;
p = &b; // 错误
```
对于const指针p指向的内存地址不能被改变，但是其内容可以改变， 因为const修饰的是p

对于第三种情况
```cpp
int a=8;
const int * const p=&a;
```
此时const p指向的内容和指向的内存地址都已固定，不可改变


### 3.3 const参数传递和函数返回值

对于const修饰函数参数可以分为三种情况

1.值传递的const修饰传递，一般这种情况不需要const修饰，因为函数会自动产生临时变量复制实参值
```cpp
#include<iostream>
 
using namespace std;
 
void Cpf(const int a)
{
    cout<<a;
    // ++a;  是错误的，a 不能被改变
}
 
int main(void)
 
{
    Cpf(8);
    system("pause");
    return 0;
}
```

2.当const参数为指针时，可以防止指针被意外篡改
```cpp
#include<iostream>
 
using namespace std;
 
void Cpf(int *const a)
{
    cout<<*a<<" ";
    *a = 9;//可以修改指针指向的值，但不能修改指针本身
}
 
int main(void)
{
    int a = 8;
    Cpf(&a);
    cout<<a; // a 为 9
    system("pause");
    return 0;
}
```

3.可以通过const+传递引用来实现引用传递，虽然不加const关键字只传递引用也可以实现引用传递，但是通过const关键字可以保护传入的对象不被修改，并且只能调用const方法，同时也能提高代码的可读性，表明函数不会修改传入的参数

```cpp
#include <iostream>
#include <string>
// 定义一个简单的类

class MyClass {

public:

	//普通构造函数
    MyClass(std::string name) : name(name) {

        std::cout << "Constructor called for " << name << std::endl;

    }

  
	//拷贝构造函数
    MyClass(const MyClass& other) : name(other.name) {

        std::cout << "Copy constructor called for " << name << std::endl;

    }

  

    void print() const {

        std::cout << "MyClass object: " << name << std::endl;

    }

  

private:

    std::string name;

};

  

  

// 按值传递

void byValue(MyClass obj) {

    obj.print();

}

  

// 按引用传递

void byConstReference(const MyClass& obj) {

    obj.print();

}

  

int main() {

    MyClass obj("Test");//创建一个MyClass对象，并调用构造函数

  

    std::cout << "Calling byValue:" << std::endl;

    byValue(obj); // 会调用拷贝构造函数

  

    std::cout << "\nCalling byConstReference:" << std::endl;

    byConstReference(obj); // 不会调用拷贝构造函数

  

    return 0;

}
```

4.可以用来修饰返回值，修饰返回值时又分为三种情况：
- 修饰内置类型的返回值时：修饰与不修饰返回值作用一样
- const修饰自定义类型的返回值，此时返回值不能作为左值使用，既不能被赋值也不能被修改
- const修饰返回的指针或者引用，是否返回一个指向const的指针，取决于我们想让用户干什么

5.const修饰类成员函数，目的是防止成员函数修改被调用对象的值，如果我们不想修改一个调用对象的值，所有的成员函数都应当声明为const成员函数
```cpp
#include <iostream>
#include <string>

class MyClass {
public:
    MyClass(std::string name) : name(name) {}

    // const成员函数声明
    void print() const {
        std::cout << "MyClass object: " << name << std::endl;
        // name = "New Name"; // 错误：不能修改成员变量
    }

    // 非const成员函数声明
    void setName(std::string newName) {
        name = newName;//可以修改成员变量
    }

private:
    std::string name;
};

```