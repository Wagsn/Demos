using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wagsn.Net.WebSockets
{
    /// <summary>
    /// UDP消息服务器
    /// </summary>
    public class UdpServerBase<DataType>
    {
        /// <summary>
        /// 用于UDP发送的网络服务类
        /// </summary>
        static UdpClient udpcRecv = null;

        /// <summary>
        /// 
        /// </summary>
        static IPEndPoint localEp = null;

        /// <summary>
        /// 开关：在监听UDP报文阶段为true，否则为false
        /// </summary>
        static bool IsUdpcRecvStart = false;

        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        static Thread thrRecv;

        /// <summary>
        /// 数据处理委托
        /// </summary>
        /// <param name="data"></param>
        public delegate void DataHandle(DataType data);

        /// <summary>
        /// 数据处理事件
        /// </summary>
        public event DataHandle OnDataHandle;

        /// <summary>
        /// 数据转换委托
        /// </summary>
        /// <param name="bytRecv">数据帧</param>
        public delegate T DataConvert<T>(byte[] bytRecv);

        /// <summary>
        /// 数据转换处理
        /// </summary>
        public DataConvert<DataType> Converter;

        /// <summary>
        /// 开始接收
        /// </summary>
        /// <param name="ip">监听IP地址</param>
        /// <param name="port">监听端口号</param>
        public void StartReceive(string ip, int port)
        {
            if (!IsUdpcRecvStart) // 未监听的情况，开始监听
            {
                localEp = new IPEndPoint(IPAddress.Parse(ip), port); // 本机IP和监听端口号
                udpcRecv = new UdpClient(localEp);
                thrRecv = new Thread(ReceiveMessage);
                thrRecv.Start();
                IsUdpcRecvStart = true;
                Console.WriteLine("UDP监听器已成功启动 =>");
            }
        }

        /// <summary>
        /// 停止接收
        /// </summary>
        public static void StopReceive()
        {
            if (IsUdpcRecvStart)
            {
                //thrRecv.Abort(); // 必须先关闭这个线程，否则会异常
                udpcRecv.Close();
                IsUdpcRecvStart = false;
                Console.WriteLine("UDP监听器已成功关闭 =>");
            }
        }

        /// <summary>
        /// 接收数据
        /// </summary>
        /// <param name="obj"></param>
        private void ReceiveMessage(object obj)
        {
            while (IsUdpcRecvStart)
            {
                try
                {
                    byte[] bytRecv = udpcRecv.Receive(ref localEp);
                    if (Converter == null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("数据转换器（Converter）不能为空！");
                        Console.ResetColor();
                        StopReceive();
                        return;
                    }
                    DataType udpStruct = Converter(bytRecv);

                    Console.WriteLine("接收报文：" + Newtonsoft.Json.JsonConvert.SerializeObject(udpStruct));

                    if (OnDataHandle != null)
                    {
                        OnDataHandle(udpStruct);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    break;
                }
            }
        }
    }
}
