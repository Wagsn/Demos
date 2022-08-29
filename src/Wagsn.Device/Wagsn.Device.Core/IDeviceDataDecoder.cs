using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    public interface IDeviceDataDecoder<TDataInner, TDataOuter>
    {
        /// <summary>
        /// 解码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TDataInner Decode(TDataOuter data);
    }
}
