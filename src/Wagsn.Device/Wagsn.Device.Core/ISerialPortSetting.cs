using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    /// <summary>
    /// 串口配置
    /// </summary>
    public interface ISerialPortSetting
    {
        string PortName { get; set; }
        int? BaudRate { get; set; }
        Parity? Parity { get; set; }
        int? DataBits { get; set; }
        StopBits? StopBits { get; set; }
        Handshake? Handshake { get; set; }
        int? WriteTimeout { get; set; }
        int? ReadTimeout { get; set; }
        bool? RtsEnable { get; set; }
    }
}
