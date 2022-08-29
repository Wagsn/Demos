using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wagsn.Device.TankerGps
{
    /// <summary>
    /// 罐车GPS信息 数据接收器
    /// </summary>
    public class TankerGpsReceiver : IDeviceDataReceiver<TankerGpsInfo>
    {
        public event DataReceivedHandler<TankerGpsInfo> DataReceived;

        /// <summary>
        /// 用于UDP接收的网络服务类
        /// </summary>
        private UdpClient udpRecv;
        /// <summary>
        /// 线程：不断监听UDP报文
        /// </summary>
        Thread thrRecv;

        public TankerGpsReceiver()
        {
            var gps_IP_Port = "127.0.0.1;87".Split(':');

            var localIpep = new IPEndPoint(IPAddress.Parse(gps_IP_Port[0]), int.Parse(gps_IP_Port[1]));

            udpRecv = new UdpClient(localIpep);
            thrRecv = new Thread(ReceiveMessage);
            thrRecv.Start();
        }

        private void ReceiveMessage(object obj)
        {
            while (true)
            {
                try
                {
                    IPEndPoint remoteIpep = new IPEndPoint(IPAddress.Any, 0);
                    byte[] bytRecv = udpRecv.Receive(ref remoteIpep);  // 接收任意远端发来的数据
                    var data = ReceiveConvert(bytRecv);
                    if (data != null && DataReceived != null)
                    {
                        DataReceived(data);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("接收处理GPS数据失败！" + ex);
                }
            }
        }

        public TankerGpsInfo ReceiveConvert(object frame)
        {
            try
            {
                byte[] bytRecv = (byte[])frame;

                TankerGpsInfo model = new TankerGpsInfo();
                if (bytRecv.Length >= 28)
                {
                    model.Id = Guid.NewGuid().ToString();

                    model.simNumber = GetCarkNo(bytRecv);
                    model.lat = GetIntValue(bytRecv, 7, 10) / 1000000.0;
                    model.lng = GetIntValue(bytRecv, 11, 14) / 1000000.0;
                    model.height = GetIntValue(bytRecv, 15, 16);
                    model.speed = GetIntValue(bytRecv, 17, 18) / 10;
                    model.direction = GetIntValue(bytRecv, 19, 20);
                    model.datetime = GetDatetimeValue(bytRecv);
                    model.discharge = GetDischargeValue(bytRecv, 27);

                    LatLng P1 = new LatLng(model.lat, model.lng);
                    var p2 = GpsToAmap.transformFromWGSToGCJ(P1);

                    model.lat = double.Parse(p2.Latitude.ToString("0.000000"));
                    model.lng = double.Parse(p2.Longitude.ToString("0.000000"));
                    return model;
                }
                else
                {
                    Console.WriteLine("接收的罐车GPS数据长度有误，标准为28字节长度！");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("罐车GPS数据转换失败！" + ex);
                return null;
            }
        }

        string GetCarkNo(byte[] bytRecv)
        {
            string no = "";
            for (int i = 1; i <= 6; i++)
            {
                no += bytRecv[i].ToString("X2");
            }

            return long.Parse(no).ToString();
        }
        string GetDatetimeValue(byte[] bytRecv)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("20");
            sb.Append(bytRecv[21].ToString("X2"));
            sb.Append("-" + bytRecv[22].ToString("X2"));
            sb.Append("-" + bytRecv[23].ToString("X2"));
            sb.Append(" " + bytRecv[24].ToString("X2"));
            sb.Append(":" + bytRecv[25].ToString("X2"));
            sb.Append(":" + bytRecv[26].ToString("X2"));
            return sb.ToString();
        }

        int GetDischargeValue(byte[] bytRecv, int index)
        {
            var str = bytRecv[index].ToString("X2");
            if (str == "00")//正转
                return 0;
            else if (str == "01")//反转
                return 1;
            else
                return 2;// 停转
        }

        int GetIntValue(byte[] bytRecv, int start, int end)
        {
            string no = "";
            for (int i = start; i <= end; i++)
            {
                no += bytRecv[i].ToString("X2");
            }

            int value = Convert.ToInt32(no, 16);

            return value;
        }
    }
}
