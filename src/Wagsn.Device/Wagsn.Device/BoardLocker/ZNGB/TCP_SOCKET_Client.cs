using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace TCZNGB.Service.TCP_SOCKET
{
    public class TCP_SOCKET_Client : IDisposable
    {

        private NetworkStream ns;
        private bool _alreadyDispose = false;
        private string _ip;
        private int _port;
        //private ILogger logger = ServiceContainer.Resolve<ILogger>();

        TcpClient tcp;

        #region 构造与释构

        public TCP_SOCKET_Client(string ip, int port)
        {
            _ip = ip;
            _port = port;
            Connection(_ip, _port);

        }

        ~TCP_SOCKET_Client()
        {
            Dispose();
        }
        protected virtual void Dispose(bool isDisposing)
        {
            if (_alreadyDispose) return;
            if (isDisposing)
            {
                if (ns != null)
                {
                    try
                    {
                        ns.Close();
                        tcp.Close();
                        //logger.Debug(this.GetType(), "ns关闭 ");

                    }
                    catch (Exception E)
                    {
                        //logger.Debug(this.GetType(), "Dispose出错: " + E.Message);
                    }
                    ns.Dispose();
                }
            }
            _alreadyDispose = true;
        }
        #endregion

        #region IDisposable 成员

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion


        #region 打开端口
        /// <summary>
        /// 打开端口
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="port"></param>
        /// <returns></returns>
        public virtual void Connection(string ip, int port)
        {
            if (ip == null || ip == "") return;
            if (port < 0) return;
            if (port.ToString() == string.Empty) port = 80;
            tcp = null;
            try
            {
                tcp = new TcpClient(ip, port);
                //logger.Debug(this.GetType(), string.Format("设备：{0}:{1},Connection成功", _ip, _port));
            }
            catch (Exception E)
            {
                //logger.Debug(this.GetType(), string.Format("设备：{0}:{1},Connection出错:{2}", _ip, _port, E.Message));
                throw new Exception("Can't connection:" + _ip);
            }
            this.ns = tcp.GetStream();
        }
        #endregion

        #region 发送Socket
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="ns"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public virtual bool Send(string message)
        {
            if (ns == null) return false;
            if (message == null || message == "") return false;
            byte[] buf = Encoding.ASCII.GetBytes(message);
            try
            {
                ns.Write(buf, 0, buf.Length);
            }
            catch (Exception E)
            {
                //logger.Debug(this.GetType(), "Send出错:" + E.Message);
                throw new Exception("Send Date Fail!");
            }
            Dispose();
            return true;
        }

        public virtual bool Send(byte[] message)
        {
            if (ns == null) return false;
            if (message == null || message.Length < 1) return false;
            try
            {
                ns.Write(message, 0, message.Length);
                //  MyLog4NetInfo.Error(MethodBase.GetCurrentMethod().DeclaringType, new Exception("Send成功"));
            }
            catch (SocketException se)
            {
                return false;
                //logger.Debug(this.GetType(), string.Format("设备：{0}:{1},Send出错:{2}", _ip, _port, E.Message));
                throw new Exception("Send Date Fail!");
            }
            Dispose();
            return true;
        }
        #endregion

        #region 收取信息
        /// <summary>
        /// 收取信息
        /// </summary>
        /// <param name="ns"></param>
        /// <returns></returns>
        public string Recev()
        {
            if (ns == null) return null;
            byte[] buf = new byte[4096];
            int length = 0;
            try
            {
                length = ns.Read(buf, 0, buf.Length);
            }
            catch (Exception E)
            {
                //logger.Debug(this.GetType(), string.Format("设备：{0}:{1},Recev出错:{2}", _ip, _port, E.Message));
                throw new Exception("Receive data fail!");
            }
            return Encoding.ASCII.GetString(buf, 0, length);
        }
        #endregion
    }

}
