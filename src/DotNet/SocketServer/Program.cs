using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

class SocketServer
{
    static void Main(string[] args)
    {
        WebServer.Start();
        Console.ReadKey();
    }
}

public class WebServer
{
    static Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

    public static void Start()
    {
        int listenPort = 8002;
        socket.Bind(new IPEndPoint(IPAddress.Any, listenPort));

        socket.Listen(100);

        //接收客户端的 Socket请求
        socket.BeginAccept(OnAccept, socket);

        Console.WriteLine($"当前web服务器启动成功,端口号为：{listenPort}");
    }

    //接收请求
    public static void OnAccept(IAsyncResult async)
    {
        var serverSocket = async.AsyncState as Socket;

        //获取到客户端的socket
        var clientSocket = serverSocket.EndAccept(async);

        //进行下一步监听
        serverSocket.BeginAccept(OnAccept, serverSocket);

        var bytes = new byte[10000];

        //获取socket的内容
        var len = clientSocket.Receive(bytes);

        //将 bytes[] 转换 string
        var request = Encoding.UTF8.GetString(bytes, 0, len);

        var response = string.Empty;

        if (!string.IsNullOrEmpty(request) && !request.Contains("favicon.ico"))
        {
            // /1.html
            var filePath = request.Split("\r\n")[0].Split(" ")[1].TrimStart('/');

            if (System.IO.File.Exists(filePath)) {
                response = System.IO.File.ReadAllText(System.IO.Path.Combine("wwwroot", filePath), Encoding.UTF8);
            }
            else
            {
                response = System.IO.File.ReadAllText("wwwroot/index.html", Encoding.UTF8);
            }
            //获取文件内容
        }

        //按照http的响应报文返回
        var responseHeader = string.Format(@"HTTP/1.1 200 OK
Date: Sun, 26 Aug 2018 03:33:36 GMT
Server: nginx
Content-Type: text/html; charset=utf-8
Cache-Control: no-cache
Pragma: no-cache
Via: hngd_ax63.139
X-Via: 1.1 tjhtapp63.147:3800, 1.1 cbsshdf-A4-2-D-14.32:8101
Connection: keep-alive
Content-Length: {0}

", Encoding.UTF8.GetByteCount(response));

        //返回给客户端了
        clientSocket.Send(Encoding.UTF8.GetBytes(responseHeader));
        clientSocket.Send(Encoding.UTF8.GetBytes(response));

        clientSocket.Close();
    }
}