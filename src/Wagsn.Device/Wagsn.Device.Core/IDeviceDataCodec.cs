using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    /// <summary>
    /// 设备数据编解码器
    /// </summary>
    /// <typeparam name="TDataInner">内部结构：ValueType、RefrenceType</typeparam>
    /// <typeparam name="TDataOuter">序列化结构：byte[]、JSON String、Form、XML</typeparam>
    public interface IDeviceDataCodec<TDataInner, TDataOuter>: IDeviceDataDecoder<TDataInner, TDataOuter>, IDeviceDataEncoder<TDataInner, TDataOuter>
    {

    }
}
