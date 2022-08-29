using IntelligentHardware.Domain.Request;
using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.LedScreen
{
    /// <summary>
    /// 过磅LED显示控制
    /// </summary>
    public interface IWeighingLedController : ILedController<WeighingInfo>
    {
    }
}
