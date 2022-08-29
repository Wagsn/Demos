using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    public interface IDeviceDataEncoder<TDataInner, TDataOuter>
    {
        /// <summary>
        /// 编码
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        TDataOuter Encode(TDataInner data);
    }
}
