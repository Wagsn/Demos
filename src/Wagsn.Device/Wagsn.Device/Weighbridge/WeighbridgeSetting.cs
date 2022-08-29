using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.Weighbridge
{
    /// <summary>
    /// 地磅配置
    /// </summary>
    public class WeighbridgeSetting: ISerialPortSetting, IWeighbridgeCodecSetting
    {
        public string Name { get; set; } = "";
        public bool Wnabled { get; set; } = false;

        public string PortName { get; set; }
        public int? BaudRate { get; set; }
        public Parity? Parity { get; set; }
        public int? DataBits { get; set; }
        public StopBits? StopBits { get; set; }
        public Handshake? Handshake { get; set; }
        public int? WriteTimeout { get; set; }
        public int? ReadTimeout { get; set; }
        public bool? RtsEnable { get; set; }

        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public bool Reverse { get; set; } = false;
        public int PacketLength { get; set; }
        public decimal TonnageConversionFactor { get; set; } = 1;
    }
}
