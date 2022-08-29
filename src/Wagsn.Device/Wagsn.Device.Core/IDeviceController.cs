using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    /// <summary>
    /// 设备控制器接口
    /// </summary>
    public interface IDeviceController<TRequest, TReceiveData> : IDeviceDataReceiver<TReceiveData>
    {
        /// <summary>
        /// 连接器（UDP、TCP、SDK、HTTP、WCF、MSMQ、GPIO）
        /// </summary>
        //protected IDeviceConnector DeviceConnector { get; set; }

        /// <summary>
        /// 数据发送
        /// </summary>
        /// <typeparam name="TRequest"></typeparam>
        /// <param name="data"></param>
        void Send(TRequest data);
    }
}
