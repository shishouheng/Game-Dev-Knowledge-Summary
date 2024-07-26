sizeof是一个关键字，它是一个编译时运算符，用于判断变量或数据类型的字节大小。可用于获取类、结构、共用体和其他用户自定义数据类型的大小。使用sizeof的语法如下
`sizeof(data type)`，其中，data type是要计算大小的数据类型，包括类、结构体、共用体和其他自定义数据类型

例：
```cpp
#include <iostream>
using namespace std;
 
int main()
{
   cout << "Size of char : " << sizeof(char) << endl;
   cout << "Size of int : " << sizeof(int) << endl;
   cout << "Size of short int : " << sizeof(short int) << endl;
   cout << "Size of long int : " << sizeof(long int) << endl;
   cout << "Size of float : " << sizeof(float) << endl;
   cout << "Size of double : " << sizeof(double) << endl;
   cout << "Size of wchar_t : " << sizeof(wchar_t) << endl;
   return 0;
}

```

执行时结果会根据使用的机器而不同
```cpp
Size of char : 1
Size of int : 4
Size of short int : 2
Size of long int : 4
Size of float : 4
Size of double : 8
Size of wchar_t : 4

```


## 注意事项：

### 1.指针的大小是固定的

指针的大小在一个给定的处理器中是固定的，因为指针需要足够的位数来表示内存地址。在32位处理中中，指针的大小是4字节，它可以寻址的内存空间是2^32字节。在64位处理器上，指针大小是8个字节，可以寻址的内存空间是2^64字节

### 2.字符串数组要算上末尾的 \0

当声明一个字符数组并初始化为一个字符串时，编译器会自动在字符串末尾添加‘\0‘，例如字符串“hello”在内存中实际存储的字符是`'H', 'e', 'l', 'l', 'o', '\0'`。

### 3.数组作为函数参数时会退化为指针，大小要按指针的计算

这是因为数组作为函数参数传递时，传递的实际是数组第一个元素的地址，而不是整个数组。具体来说即数组在函数参数中会自动转换成指向数组第一个元素的指针，这种行为被称为数组退化。
例：
```cpp
int func(char array[]) {
    printf("sizeof=%d\n", sizeof(array));//输出8，数组退化为指针
    printf("strlen=%d\n", strlen(array));//输出11，字符串长度为11
}

int main() {
    char array[] = "Hello World";
    printf("sizeof=%d\n", sizeof(array));//输出12，11个字符+一个终止字符\0,共12字节
    printf("strlen=%d\n", strlen(array));//输出11，字符串长度为11
    func(array);
}

```

### 4.结构体要考虑字节对齐

结构体的成员通常按照它们自己的大小排列，同时根据编译器的字节对齐规则进行排列和填充。例如一个int变量需要按照4字节对齐，如果结构体中有多个int类型成员，则编译器可能会在它们之间插入填充字节，以确保每个int变量都从一个合适的地址开始，以提高访问效率

例如
```cpp
struct Example {
    char c;
    int i;
    double d;
};
```

在64位系统中
- char类型占用1字节
- int类型占用4字节，且需要对齐到4字节边界
- double类型占用8字节，且需要对齐到8字节边界

所以结构体Example的布局如下：
```
--------------------------------
| c | 填充 | i (int) | d (double) |
--------------------------------

```

因此在64位系统上结构体Example的大小为：

```
1 (char c) + 3 (填充) + 4 (int i) + 8 (double d) = 16 字节
```