using System;
using System.Collections.Generic;
using System.Text;

namespace Wagsn.Device.LedScreen
{
    /// <summary>
    /// LED渲染设置
    /// </summary>
    public interface ILedRenderSettings
    {
        /// <summary>
        /// LED屏宽度，默认64
        /// </summary>
        int Width { get; set; }

        /// <summary>
        /// LED屏高度，默认128
        /// </summary>
        int Height { get; set; }

        /// <summary>
        /// 文本显示X轴偏移，默认-3
        /// </summary>
        int OffsetX { get; set; }

        /// <summary>
        /// 字体族名，默认宋体
        /// </summary>
        string FontFamilyName { get; set; }

        /// <summary>
        /// 字体大小，默认16
        /// </summary>
        int FontSize { get; set; }
    }
}
