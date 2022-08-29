using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.Core
{
    /// <summary>
    /// 网络设置
    /// </summary>
    public interface INetSeeting: IIpAddressSeeting, ISerialPortSetting
    {
        /// <summary>
        /// 网络模式：1-网口 TcpClient，2-网口 TcpServer，3-Udp，4-串口
        /// </summary>
        int NetMode { get; set; }
    }
}
