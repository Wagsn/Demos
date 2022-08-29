using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Wagsn.Device
{
    /// <summary>
    /// UDP数据接收器
    /// </summary>
    /// <typeparam name="TReceiveData">数据格式</typeparam>
    public abstract class DeviceUdpReceiver<TReceiveData> : IDeviceDataReceiver<TReceiveData>
    {
        public event DataReceivedHandler<TReceiveData> DataReceived;
        public abstract TReceiveData ReceiveConvert(object frame);

        protected UdpClient udpClient;  // UDP客户端
        protected IPEndPoint localEp;  // 本地端

        public DeviceUdpReceiver(string port)
        {
            localEp = new IPEndPoint(IPAddress.Any, int.Parse(port)); // 本机IP和监听端口号，接收数据的地址端口

            udpClient = new UdpClient(localEp);

            var thrRecv = new Thread(ReceiveMessage);
            thrRecv.Start();
        }

        private void ReceiveMessage(object obj)
        {
            while (true)
            {
                try
                {
                    byte[] bytRecv = udpClient.Receive(ref localEp);  // 远程端发给本地主机的端口
                    var udpStruct = ReceiveConvert(bytRecv);

                    if (udpStruct != null && DataReceived != null)
                    {
                        DataReceived(udpStruct);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("接收处理数据失败：" + ex);
                    break;
                }
            }
        }
    }
}
