在c语言中，通常用(type_name)expression这种方式来做强制类型转换，但是在c++中，更推荐使用四个转换操作符来实现显式类型转换：

- static_cast
- dynamic_cast
- const_cast
- reinterpret_cast


## 1.static_cast

用法：`static_cast<new_type>(expression)`
这里的`static_cast`和c语言()做强制类型转换基本是等价的
主要用于以下场景：

### 1.1基本类型之间的转换

将一个基本类型转换为另一个基本类型，例如将整数转换为浮点数或将字符转换为整数

```cpp
int a=42;
double b=static_cast<double>(a);//将整数a转换为双精度浮点数
```

### 1.2 指针类型之间的转换

将一个指针类型转换为另一个指针类型，尤其是在类层次结构中从基类指针转换为派生类指针，这种转换不执行运行时类型检查，可能不安全，要自己保证指针确实可以互相转换。

```cpp
class Base {};
class Derived : public Base {};

Base* base_ptr = new Derived();
Derived* derived_ptr = static_cast<Derived*>(base_ptr); // 将基类指针base_ptr转换为派生类指针derived_ptr
```

### 1.3 引用类型之间的转换

类似于指针类型之间的转换，可以将一个引用类型转换为另一个引用类型，在这种情况下，也应注意安全性

```cpp
Derived derived_obj;
Base& base_ref = derived_obj;
Derived& derived_ref = static_cast<Derived&>(base_ref); // 将基类引用base_ref转换为派生类引用derived_ref
```

**static_cast在编译时执行类型转换，在进行指针或引用类型转换时，需要自己保证合法性。如果想要运行时类型检查，可以使用`dynamic_cast`进行安全的向下类型转换

## 2.dynamic_cast

用法：`dynamic_cast<new type>(expression)`

`dynamic_cast`在c++中主要应用于父子类层次结构中的安全类型转换。
它在运行时执行类型检查，因此相比于`static_cast>`它更加的安全

`dynamic_cast`的主要应用场景：

### 2.1向下类型转换

当需要将基类指针或引用转换为派生类指针或引用时，`dynamic_cast`可以确保类型兼容性。如果转换失败，`dynamic_cast`会返回空指针（指针类型）或抛出异常（引用类型）

```cpp
class Base { virtual void dummy() {} };
class Derived : public Base { int a; };

Base* base_ptr = new Derived();
Derived* derived_ptr = dynamic_cast<Derived*>(base_ptr); // 将基类指针base_ptr转换为派生类指针derived_ptr，如果类型兼容，则成功
```

### 2.2用于多态类型检查

处理多态对象时，`dynamic_cast`可以用来确定对象的实际类型，例如：

```cpp
class Animal { public: virtual ~Animal() {} };
class Dog : public Animal { public: void bark() { /* ... */ } };
class Cat : public Animal { public: void meow() { /* ... */ } };

Animal* animal_ptr = /* ... */;

// 尝试将Animal指针转换为Dog指针
Dog* dog_ptr = dynamic_cast<Dog*>(animal_ptr);
if (dog_ptr) {
    dog_ptr->bark();
}

// 尝试将Animal指针转换为Cat指针
Cat* cat_ptr = dynamic_cast<Cat*>(animal_ptr);
if (cat_ptr) {
    cat_ptr->meow();
}
```

另外，要使`dynamic_cast`有效，基类至少需要一个虚函数。
因为`dynamic_cast`只有在基类存在虚函数的情况下才有可能将基类转化为子类

## 3.const_cast

用法：`const_cast<new type>(expression)`,new_type必须是一个指针、引用或者指向对象类型成员的指针。

### 3.1修改const对象

当需要修改const对象时，可以使用`const_cast`来删除const属性
```cpp
const int a = 42;
int* mutable_ptr = const_cast<int*>(&a); // 删除const属性，使得可以修改a的值
*mutable_ptr = 43; // 修改a的值
```

### 3.2 const对象调用非const成员函数

当需要使用const对象调用非const成员函数时，可以使用`const_cast`删除对象的const属性

```cpp
class MyClass {
public:
    void non_const_function() { /* ... */ }
};

const MyClass my_const_obj;
MyClass* mutable_obj_ptr = const_cast<MyClass*>(&my_const_obj); // 删除const属性，使得可以调用非const成员函数
mutable_obj_ptr->non_const_function(); // 调用非const成员函数
```

