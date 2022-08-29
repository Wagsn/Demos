using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.Weighbridge
{
    /// <summary>
    /// 编解码器配置
    /// </summary>
    public interface IWeighbridgeCodecSetting
    {
        /// <summary>
        /// 开始字符
        /// </summary>
        string Prefix { get; set; }

        /// <summary>
        /// 结束字符
        /// </summary>
        string Suffix { get; set; }

        /// <summary>
        /// 是否反序
        /// </summary>
        bool Reverse { get; set; }

        /// <summary>
        /// 报文长度
        /// </summary>
        int PacketLength { get; set; }

        /// <summary>
        /// 吨数转换系数
        /// </summary>
        decimal TonnageConversionFactor { get; set; }
    }
}
