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
    public class SerialPortSetting: ISerialPortSetting
    {
        public virtual string PortName { get; set; }
        public virtual int? BaudRate { get; set; }
        public virtual Parity? Parity { get; set; }
        public virtual int? DataBits { get; set; }
        public virtual StopBits? StopBits { get; set; }
        public virtual Handshake? Handshake { get; set; }
        public virtual int? WriteTimeout { get; set; }
        public virtual int? ReadTimeout { get; set; }
        public virtual bool? RtsEnable { get; set; }
    }
}
