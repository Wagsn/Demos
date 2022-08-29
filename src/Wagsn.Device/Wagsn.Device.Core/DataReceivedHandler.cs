using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    /// <summary>
    /// 数据处理委托
    /// </summary>
    /// <param name="data"></param>
    public delegate void DataReceivedHandler<TReceiveData>(TReceiveData data);
}
