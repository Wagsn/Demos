using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.Core
{
    /// <summary>
    /// IP网络配置
    /// </summary>
    public interface IIpAddressSeeting
    {
        /// <summary>
        /// 本地IP
        /// </summary>
        string LocalIp { get; set; }

        /// <summary>
        /// 本地端口
        /// </summary>
        ushort LocalPort { get; set; }

        /// <summary>
        /// 远程端口
        /// </summary>
        string RemoteIp { get; set; }

        /// <summary>
        /// 远程端口
        /// </summary>
        ushort RemotePort { get; set; }
    }
}
