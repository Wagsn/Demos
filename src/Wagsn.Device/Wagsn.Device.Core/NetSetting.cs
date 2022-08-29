using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

namespace Wagsn.Device.Core
{
    /// <summary>
    /// 网络设置
    /// </summary>
    public class NetSeeting : INetSeeting
    {
        public int NetMode { get; set; }
        public string LocalIp { get; set; }
        public ushort LocalPort { get; set; }
        public string RemoteIp { get; set; }
        public ushort RemotePort { get; set; }
        public string PortName { get; set; }
        public int? BaudRate { get; set; }
        public Parity? Parity { get; set; }
        public int? DataBits { get; set; }
        public StopBits? StopBits { get; set; }
        public Handshake? Handshake { get; set; }
        public int? WriteTimeout { get; set; }
        public int? ReadTimeout { get; set; }
        public bool? RtsEnable { get; set; }
    }
}
