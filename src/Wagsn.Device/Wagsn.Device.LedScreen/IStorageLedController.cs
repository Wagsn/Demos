using IntelligentHardware.Domain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.LedScreen
{
    /// <summary>
    /// 料仓LED显示控制器
    /// </summary>
    public interface IStorageLedController: ILedController<StorageDisplayInfo>
    {
    }
}
