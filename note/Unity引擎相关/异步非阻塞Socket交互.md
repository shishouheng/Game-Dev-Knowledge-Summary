# 一、定义

同步与异步（针对于交互）：
- 同步：当用户的进程触发IO操作时等待操作完成
- 异步：当用户进程触发IO操作以后无需等待完成便可以执行后续操作，当IO操作完成后通过回调函数或其他方式来处理结果

**简单来说，同步异步是针对调用者来说，当调用者发起一个请求后，是否需要等待被调用者的反馈，如果需要等待就是同步，否则就是异步**

阻塞与非阻塞（针对访问数据）：
根据IO操作的就绪状态才采用不同的方式，一种读取或写入操作的实现方式
- 阻塞：在读取或写入函数时一直等待
- 非阻塞：读取或者写入函数会立即返回一个状态值

**阻塞和非阻塞是针对被调用者来说，当被调用者收到一个请求之后，做完请求任务才给出反馈就是阻塞，如果收到请求直接给出反馈然后再去做任务就是非阻塞**

# 二、实现异步非阻塞通信

服务端代码
```c#
 public class GameServer
    {
        //存储所有客户端套接字集合
        private static List<Socket> clientSockets = new List<Socket>();
        //缓冲区
        private static byte[] buffer = new byte[1024];
        static void Main(string[] args)
        {
            Initialize();
        }
        //初始化服务器
        public static void Initialize()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork,
           SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"),
           1234));
            socket.Listen(4);
            //开始接受客户端的连接请求
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
            Console.WriteLine("Server Is Ready!");
            Console.Read();
        }
        //客户端连接到服务器的回调
        public static void ClientAccepted(IAsyncResult ar)
        {
            Socket socket = ar.AsyncState as Socket;
            //得到连接到服务器的客户端的socket实例 
            Socket clientSocket = socket.EndAccept(ar);
            clientSockets.Add(clientSocket);
            Console.WriteLine("有客户端连接进来了");
            Send(clientSocket, "你好啊，欢迎光临--" + DateTime.Now.ToString());
            //接受客户端的信息
            clientSocket.BeginReceive(buffer, 0, buffer.Length,
           SocketFlags.None, ReceiveMessage, clientSocket);
            //继续接受下一个客户端的连接请求
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
        }
        public static void ReceiveMessage(IAsyncResult ar)
        {
            try
            {
                Socket client = ar.AsyncState as Socket;
                int length = client.EndReceive(ar);
                string message = Encoding.UTF8.GetString(buffer, 0, length);
                client.BeginReceive(buffer, 0, buffer.Length,
               SocketFlags.None, ReceiveMessage, client);
                RequestHandle(client, message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private static void Send(Socket clientSocket, string message)
        {
            try
            {

                clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(message));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        private static void Broadcast(string message)
        {
            foreach (var socket in clientSockets)
            {
                Send(socket, message);
            }
        }
        private static void RequestHandle(Socket client, string message)
        {
            JsonData data = JsonMapper.ToObject(message);
            int cmd = (int)data["requestType"];
            RequestType type =
           (RequestType)Enum.Parse(typeof(RequestType), cmd.ToString());
            Console.WriteLine(type);
            if (type == RequestType.Login)
            {
                LoginRequest login = JsonMapper.ToObject<LoginRequest>(message);
                Console.WriteLine(login.name);
            }
        }
    }
```

客户端代码

```c#
public enum RequestType { Login,Register}

public class Request
{
    public RequestType requestType;
    public Request() { }
    public Request(RequestType requestType)
    {
        this.requestType = requestType;
    }

}

public class LoginRequest : Request
{
    public string name;
    public LoginRequest() { }
    public LoginRequest(string name):base(RequestType.Login)
    {
        this.name = name;
    }
}

```

```c#
public class NetWorkNIO : MonoBehaviour
{
    private Socket clientSocket;
    //缓冲区
    private static byte[] buffer = new byte[1024];

    private static NetWorkNIO instance;
    public static NetWorkNIO Instance { get { return instance; } }

    private void Awake()
    {
        instance = this;
        Initialize();
    }

    public void Initialize()
    {
        clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        Debug.Log("开始连接服务器");

        clientSocket.BeginConnect("127.0.0.1", 1234, OnConnectCallBack, clientSocket);
    }

    //开始连接的回调
    void OnConnectCallBack(IAsyncResult ar)
    {
        try
        {
            Debug.Log("socket连接:" + ar.IsCompleted);
            //结束连接
            clientSocket.EndConnect(ar);

            //连接完成后异步接受数据
            clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceiveMessage, clientSocket);
        }
        catch(Exception ex)
        {
            print("服务器异常：" + ex);
        }
    }

    //接收数据得回调
    void OnReceiveMessage(IAsyncResult ar)
    {
        try
        {
            Socket socket = ar.AsyncState as Socket;
            int length = socket.EndReceive(ar);

            string message = System.Text.Encoding.UTF8.GetString(buffer, 0, length);
            print(message);

            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, OnReceiveMessage, socket);
        }
        catch(Exception e)
        {
            print(e);
        }
    }

    //发送数据
    private void Send(string message)
    {
        try
        {
            clientSocket.Send(System.Text.Encoding.UTF8.GetBytes(message));
        }
        catch(Exception e)
        {
            print(e);
        }
    }

    public void SendRequest(Request request)
    {
        string message = JsonMapper.ToJson(request);
        Send(message);
    }
}
```