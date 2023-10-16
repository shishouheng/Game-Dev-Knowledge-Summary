# 一、基础知识

## 1.1 网络通信的要素

- Internet Protocol：因特网协议，用于在互联网进行数据传输，Ip协议定义了数据包的格式和传输规则。它为每个设备分配了一个唯一的IP地址用于在网络中标识设备的位置，如本机的ip地址是127.0.0.1
- 端口号：端口号用于标识设备上的应用程序或服务，一个设备可以运行多个应用程序或服务，每个应用程序或服务通过不同的端口号进行识别，端口号的范围是0-65535

**在网络编程中，通过指定目标设备的IP地址和端口号，可以将数据包传递给目标设备上运行的特定应用程序或服务**


## 1.2 网络协议

网络协议是一组规则和约定，用于在计算机网络中进行通信和数据交换。它定义了数据的格式、传输方式、错误检测和纠正机制、连接建立和断开的过程等，以确保不同设备和系统之间的互操作性和数据的可靠传输。最常见的网络协议体系结构是TCP/IP协议，包含了以下几个层次
- 物理层：负责传输比特流，定义了电气、光学和机械接口等物理连接的规范
- 数据链路层：负责将比特流组织成数据帧，提供可靠的点对点数据传输
- 网络层：负责将数据包从源设备传输到目标设备，包括路由选择、寻址和分组转发等功能。IP协议就是网络层的核心协议
- 传输层：负责提供端到端的可靠数据传输，常见的协议有TCP和UDP。TCP协议提供面向连接的可靠传输，而UDP提供无连接的不可靠传输
- 应用层：提供特定应用程序的协议和服务，如HTTP、FTP、SMTP等。应用层协议定义了数据的格式和交换方式，使不同应用程序能够相互通信和交换数据


## 1.3 HTTP网络通信

HTTP协议是一种应用层协议，定义了客户端和服务器之间的通信规则和格式。HTTP协议使用TCP协议作为传输层协议，通过TCP协议提供可靠的数据传输服务


HTTP最显著的特点是客户端发送的每次请求都需要服务器回送响应，然后在客户端的请求结束后会主动释放连接。从建立连接到关闭连接的过程叫做“一次连接”

HTTP协议是一种短链接，即如果需要保持客户端的在线状态，则需要不断像服务器发送连接请求，即使不需要任何数据，客户端也保持每隔一个固定的事件就像服务器发送一次保持连接的请求——这种现象被称为“心跳”


## 1.4 Socket网络通信

Socket不属于协议范畴，而仅仅是一个编程中的调用接口，它是对一些基础协议进行的二次封装，通过调用Socket才能使用TCP/IP协议

Socket使用套接字来进行通信，套接字是一种抽象的网络通信端点，它可以用来创建网络连接、发送和接收数据。在Socket通信中，有两种常见的角色
- 服务器端：服务器端创建一个套接字，并监听指定的端口。当客户端请求连接时，服务器端接收连接请求，并与客户端建立一个连接。服务器端可以接收来自客户端的请求并做出相应的处理
- 客户端：客户端创建一个套接字，并指定服务器的IP地址和端口号。客户端可通过套接字连接到服务器，并发送请求，客户端可以接收来自服务器的响应数据


Socket通信的基本流程如下：

- 服务器端创建一个套接字，并绑定到指定的IP地址和端口号
- 服务器端监听指定端口，等待客户端的连接请求
- 客户端创建一个套接字，并指定服务器的IP地址和端口号
- 客户端通过套接字连接到服务器
- 服务器端接收客户端的连接请求，并与客户端建立一个连接
- 客户端发送请求数据到服务器
- 服务器端接收客户端的请求数据，并做出相应的处理
- 服务器端发送响应数据到客户端
- 客户端接收服务器端的响应数据
- 客户端和服务器端关闭连接
![](https://github.com/shishouheng/Unity-learning/blob/main/images/Socket.png)
![[Socket.png]]

socket的两种通信模式

- 面向连接的通信：在这种通信模式中，通信双方在建立连接之后才能进行数据的传输。这种通信模式使用的是TCP协议，提供了可靠、有序、面向字节流的数据传输。在建立连接之前，需要先建立一个可靠的连接，通常称为“三次握手”。然后，在连接建立之后，通信双方可以通过Socket进行数据的传输。这种通信模式适用于需要可靠传输、顺序传输以及双向通信的场景，如文件传输、视频传输等
- 无连接的通信：在这种通信模式中通信双方不需要事先建立连接，可以直接进行数据的传输。这种通信模式使用的是UDP协议，是一种无连接、不可靠的数据传输协议。在无连接的通信中，数据包可以独立发送，不需要保持顺序，也不需要确认接收。这种通信模式适用于实时性要求高、数据量小、丢失一些数据不会对应用造成重大影响的场景，如在线游戏、实时视频聊天等

## 1.5 三次握手

第一次握手（SYN）：客户端向服务器发送一个带有SYN（同步）标志的数据包，表示请求建立连接。此时客户端进入SYN_SENT状态，等待服务器的确认。

第二次握手（SYN+ACK）：服务器收到客户端的请求后，会发送一个带有SYN和ACK（确认）标志的数据包作为响应。服务器还会为该连接分配资源，并进入SYN_RECV状态。

第三次握手（ACK）：客户端收到服务器的响应后，会发送一个带有ACK标志的数据包作为确认。此时，客户端和服务器都进入已建立连接的状态，可以开始进行数据传输。



## 1.6 案例：实现客户端与服务端的连接


服务端
```c#
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace Server
{
    class ServerSocket
    {
        static void Main(string[] args)
        {
            new ServerSocket().Start();
        }

        void Start()
        {
            //IP地址
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //绑定IP地址和端口号
            IPEndPoint point = new IPEndPoint(ip, 1234);
            //创建socket，参数分别是地址族、字节流、和协议类型
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //socket绑定ip和端口
            socket.Bind(point);
            socket.Listen(10);
            Console.WriteLine("open server succeed");

            while(true)
            {
                Socket client = socket.Accept();
                Console.WriteLine("client  connect");
            }
        }
    }
}
```

客户端

```c#
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class Client : MonoBehaviour
{
    private void OnGUI()
    {
        if(GUILayout.Button("connect server"))
        {
            Socket clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //连接到本机的1234端口
            clientSocket.Connect("127.0.0.1", 1234);
        }
    }

}
```


## 二、游戏聊天室搭建

## 2.1 转换方法

在网络通信的过程中，需要将各种数据类型转换为字节流进行传输或存储，然后再将字节流转换回对应的数据类型，这就是一个常用的转换工具类

```c#
public class TypeConvert
{
    public TypeConvert()
    {
    }
    public static byte[] getBytes(float s, bool asc)
    {
        int buf = (int)(s * 100);
        return getBytes(buf, asc);
    }
    public static float getFloat(byte[] buf, bool asc)
    {
        int i = getInt(buf, asc);
        float s = (float)i;
        return s / 100;
    }
    public static byte[] getBytes(short s, bool asc)
    {
        byte[] buf = new byte[2];
        if (asc)
        {
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                buf[i] = (byte)(s & 0x00ff);
                s >>= 8;
            }
        }
        else
        {
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(s & 0x00ff);
                s >>= 8;
            }
        }
        return buf;
    }
    public static byte[] getBytes(int s, bool asc)
    {
        byte[] buf = new byte[4];
        if (asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                buf[i] = (byte)(s & 0x000000ff);
                s >>= 8;
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(s & 0x000000ff);
                s >>= 8;
            }
        return buf;
    }
    public static byte[] getBytes(long s, bool asc)
    {
        byte[] buf = new byte[8];
        if (asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                buf[i] = (byte)(s & 0x00000000000000ff);
                s >>= 8;
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                buf[i] = (byte)(s & 0x00000000000000ff);
                s >>= 8;
            }
        return buf;
    }
    public static short getShort(byte[] buf, bool asc)
    {
        if (buf == null)
        {
            //throw new IllegalArgumentException("byte array isnull!");
        }
        if (buf.Length > 2)
        {
            //throw new IllegalArgumentException("byte array size >2 !");
 }
        short r = 0;
        if (!asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                r <<= 8;
                r |= (short)(buf[i] & 0x00ff);
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                r <<= 8;
                r |= (short)(buf[i] & 0x00ff);
            }
        return r;
    }
    public static int getInt(byte[] buf, bool asc)
    {
        if (buf == null)
        {
            // throw new IllegalArgumentException("byte array is null!");
        }
        if (buf.Length > 4)
        {
            //throw new IllegalArgumentException("byte array size >4 !");
        }
        int r = 0;
        if (!asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                r <<= 8;
                r |= (buf[i] & 0x000000ff);
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                r <<= 8;
                r |= (buf[i] & 0x000000ff);
            }
        return r;
    }
    public static long getLong(byte[] buf, bool asc)
    {
        if (buf == null)
        {
            //throw new IllegalArgumentException("byte array is null!");
        }
        if (buf.Length > 8)
        {
            //throw new IllegalArgumentException("byte array size >8 !");
        }
        long r = 0;
        if (!asc)
            for (int i = buf.Length - 1; i >= 0; i--)
            {
                r <<= 8;
                r |= (buf[i] & 0x00000000000000ff);
            }
        else
            for (int i = 0; i < buf.Length; i++)
            {
                r <<= 8;
                r |= (buf[i] & 0x00000000000000ff);
            }
        return r;
    }
}
```

## 2.2 简单聊天室的实现

主要思路是：
服务器在主线程监听来自客户端Socket的连接并保存，同时开辟新线程来和客户端实现数据交互，
客户端与服务器之间通过字节流实现信息传输。
传输过程中双方需约定传递规范（存在顺序）：①消息类型②消息长度③消息数据


公共模块
```c#
namespace ClientServerCommon
{
    public class Common
    {
        public int id;
        public Common(int id)
        {
            this.id = id;
        }
    }

    public class PlayerMessage:Common
    {
        public int playerId;
        public int exp;
        public int level;
        public int attack;

        public PlayerMessage() : base(0) { }

        public PlayerMessage (int id,int playerId,int level,int exp,int attack):base(id)
        {
            this.playerId = playerId;
            this.level = level;
            this.exp = exp;
            this.attack = attack;
        }
    }
}
```


客户端
```c#
public class Client : MonoBehaviour
{
    SocketThread st;

    string username = "请输入用户名";
    public string message = "";
    string talk = "";
    private void OnGUI()
    {
        if(st==null)
        {
            username = GUILayout.TextField(username);
            if(GUILayout.Button("连接服务器"))
            {
                Connect();
            }
        }
        else
        {
            Talk();
        }

        
    }

    private void Connect()
    {
        Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        mySocket.Connect("127.0.0.1", 1234);
        st = new SocketThread(mySocket,this);
        st.thread = new Thread(new ThreadStart(st.Run));
        st.thread.Start();
        st.DoLogin(username);

        
    }

    void Talk()
    {
        GUI.TextArea(new Rect(0, 0, 200, 300), message);
        talk = GUI.TextField(new Rect(0, 300, 200, 50), talk);

        if(GUI.Button(new Rect(200,300,80,50),"发送"))
        {
            st.DoTalk(talk);
        }
    }
}
```

```c#
public class SocketThread
{
    //根据不同的编号服务器进行不同的处理
    public const int LOGIN = 1001;
    public const int TALK = 1002;


    Socket socketClient;
    Client client;
    public Thread thread;

    public SocketThread (Socket socketClient,Client client)
    {
        this.socketClient = socketClient;
        this.client = client;
    }
    //客户端执行的分线程方法
    public void Run()
    {
        while(true)
        {
            try
            {
                string s = ReceiveString();
                client.message += s+"\r\n";
            }
            catch(Exception e)
            {
                thread.Abort();
            }
        }
    }

    //执行登录发送用户名
    public void DoLogin(string username)
    {
        try
        {
            Send(LOGIN);
            Send(username);
        }
        catch(Exception e)
        {
            MonoBehaviour.print(e);
            thread.Abort();
        }
        
    }

    //执行对话，发送聊天内容
    public void DoTalk(string message)
    {
        try
        {
            Send(TALK);
            Send(message);
        }
        catch(Exception e)
        {
            MonoBehaviour.print(e);
            thread.Abort();
        }
    }
	
    public void Send(int data)
    {
        byte[] datas = TypeConvert.getBytes(data, true);
        socketClient.Send(datas);
    }

    public void Send(string data)
    {
        byte[] datas = Encoding.UTF8.GetBytes(data);
        Send(datas.Length);
        socketClient.Send(datas);
    }

    int ReceiveInt()
    {
        byte[] buf = new byte[4];
        socketClient.Receive(buf);
        return TypeConvert.getInt(buf, true);
    }

    string ReceiveString()
    {
        int length = ReceiveInt();
        byte[] bytes = new byte[length];
        socketClient.Receive(bytes);
        return Encoding.UTF8.GetString(bytes);
    }

    public void SendJsonMessage(int commonId,string message)
    {
        Send(commonId);
        Send(message);  
    }
}
```

服务端
```c#
namespace Server
{
    //分线程
    class SocketThread
    {
        //根据不同的编号服务器进行不同的处理
        public const int LOGIN = 1001;
        public const int TALK = 1002;

        //在线玩家的集合
        private Dictionary<string, Player> dics;

        //客户端套接字
        private Socket socket;

        Player player;

        public SocketThread(Socket clientSocket,Dictionary<string,Player>dics)
        {
            socket = clientSocket;
            this.dics = dics;
        }

        //执行分线程的方法
        public void Run()
        {
            while(true)
            {
                switch(ReceiveInt())
                {
                    case LOGIN:
                        string username = ReceiveString();
                        if (dics.ContainsKey(username))
                            Console.WriteLine("用户名重复");
                        else
                        {
                            Console.WriteLine(username + "进入聊天室");
                            Player player = new Player();
                            player.MySocket = socket;
                            player.Name = username;
                            dics.Add(username, player);
                            SendMessageToAllPlayer(username + "进入聊天室");
                        }


                        break;

                    case TALK:
                        string talkContext = ReceiveString();
                        SendMessageToAllPlayer(player.Name+":"+talkContext);
                        break;
                }
            }
        }

        //广播
        void SendMessageToAllPlayer(string content)
        {
            byte[] datas = Encoding.UTF8.GetBytes(content);
            int length = datas.Length;

            foreach (var item in dics)
            {
                 player = item.Value;
                player.MySocket.Send(TypeConvert.getBytes(length, true));
                player.MySocket.Send(datas);
            }
        }

        int ReceiveInt()
        {
            byte[] buf = new byte[4];
            socket.Receive(buf);
            return TypeConvert.getInt(buf, true);
        }

        string ReceiveString()
        {
            int length = ReceiveInt();
            byte[] bytes = new byte[length];
            socket.Receive(bytes);
            return Encoding.UTF8.GetString(bytes);
        }

        public void Send(int data)
        {
            byte[] datas = TypeConvert.getBytes(data, true);
            socket.Send(datas);
        }

        public void Send(string data)
        {
            byte[] datas = Encoding.UTF8.GetBytes(data);
            Send(datas.Length);
            socket.Send(datas);
        }
    }
}
```

```c#
namespace Server
{
    class Player
    {
        private Socket mySocket;
        public Socket MySocket
        {
            get { return mySocket; }
            set { mySocket = value; }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
    }
}
```

```c#
namespace Server
{
    class ServerSocket
    {
        //字典，通过名字获取对应玩家信息
        private Dictionary<string, Player> dics = new Dictionary<string, Player>();

        static void Main(string[] args)
        {
            new ServerSocket().Start();
        }

        void Start()
        {
            //ip地址
            IPAddress ip = IPAddress.Parse("127.0.0.1");
            //绑定ip地址和端口号
            IPEndPoint point = new IPEndPoint(ip, 1234);
            //创建socket  地址族、字节流、tcp协议
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //绑定ip和端口
            socket.Bind(point);
            socket.Listen(10);
            Console.WriteLine("open server succeed");

            while(true)
            {
                Socket client = socket.Accept();
                Console.WriteLine("client  connect");

                //把当前连接进来的客户端存储到一个线程中
                SocketThread st = new SocketThread(client, dics);
                //在新线程开启SocketThread的Run方法
                new Thread(new ThreadStart(st.Run)).Start();
            }
        }
    }
}
```

## 2.3 把自定义数据类型转换为字符串进行传输

可以先将类型转换为JSON字符串，然后再通过上述方法进行传输

角色数据信息类封装到一个公共模块中以供客户端和服务端共同使用
```c#
 public class PlayerMessage:Common
    {
        public int playerId;
        public int exp;
        public int level;
        public int attack;

        public PlayerMessage() : base(0) { }

        public PlayerMessage (int id,int playerId,int level,int exp,int attack):base(id)
        {
            this.playerId = playerId;
            this.level = level;
            this.exp = exp;
            this.attack = attack;
        }
    }
```

然后在客户端实例化一个角色信息类并转换为json字符串，然后转换为字节流传送给客户端
```c#
 private void Connect()
    {
        Socket mySocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        mySocket.Connect("127.0.0.1", 1234);
        st = new SocketThread(mySocket,this);
        st.thread = new Thread(new ThreadStart(st.Run));
        st.thread.Start();
        

        PlayerMessage message = new PlayerMessage(SocketThread.LOGIN, 2003, 7000, 50, 200);
        st.SendJsonMessage(message.id, JsonMapper.ToJson(message));
    }


        public void SendJsonMessage(int commonId,string message)
    {
        Send(commonId);
        Send(message);  
    }
```


在服务端接收客户端传来的用户数据，将字节流转换为json字符串，然后将字符串转换回对象

```c#
        string data = ReceiveString();
        PlayerMessage message = JsonMapper.ToObject<PlayerMessage>(data);
        Console.WriteLine(message.id);
        Console.WriteLine(message.level);
        Console.WriteLine(message.attack);
        Console.WriteLine(message.playerId);
```
