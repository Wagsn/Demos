using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    /// <summary>
    /// 设备数据接收器接口
    /// </summary>
    /// <typeparam name="TReceiveData">接收数据</typeparam>
    public interface IDeviceDataReceiver<TReceiveData>
    {
        /// <summary>
        /// 接收数据转换为内部对象
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        TReceiveData ReceiveConvert(object frame);

        /// <summary>
        /// 数据处理触发事件
        /// </summary>
        event DataReceivedHandler<TReceiveData> DataReceived;
    }
}
