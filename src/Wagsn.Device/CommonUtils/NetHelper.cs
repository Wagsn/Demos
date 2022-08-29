using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class NetHelper
    {
        /// <summary>
        /// Ping IP
        /// </summary>
        /// <param name="hostNameOrAddress">IP</param>
        /// <param name="timeout">超时，默认200毫秒</param>
        /// <returns></returns>
        private static bool PingIp(string hostNameOrAddress, int timeout = 200)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(hostNameOrAddress))
                {
                    return false;
                }
                using (Ping pingSender = new Ping())
                {
                    PingReply reply = pingSender.Send(hostNameOrAddress, timeout);//第一个参数为ip地址，第二个参数为ping的时间 
                    if (reply.Status == IPStatus.Success)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public static bool PingIp(string hostNameOrAddress)
        {
            return PingIp(hostNameOrAddress, 300);
        }

        /// <summary>
        /// 重试四次Ping IP
        /// </summary>
        /// <param name="hostNameOrAddress">IP地址</param>
        /// <returns></returns>
        public static bool TryPingIP(string hostNameOrAddress) 
        {
            if (PingIp(hostNameOrAddress) == false && PingIp(hostNameOrAddress) == false && PingIp(hostNameOrAddress) == false && PingIp(hostNameOrAddress) == false)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 检查一个Socket是否可连接
        /// </summary>
        /// <param name="client">Socket</param>
        /// <returns></returns>
        private bool IsSocketConnected(Socket client)
        {
            bool blockingState = client.Blocking;
            try
            {
                byte[] tmp = new byte[1];
                client.Blocking = false;
                client.Send(tmp, 0, 0);
                return true;
            }
            catch (SocketException e)
            {
                // 产生 10035 == WSAEWOULDBLOCK 错误，说明被阻止了，但是还是连接的
                if (e.NativeErrorCode.Equals(10035))
                    return false;
                else
                    return true;
            }
            finally
            {
                client.Blocking = blockingState;    // 恢复状态
            }
        }
    }
}
