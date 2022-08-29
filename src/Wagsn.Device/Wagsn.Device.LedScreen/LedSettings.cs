using System;
using System.Collections.Generic;
using System.Text;
using Wagsn.Device.Core;

namespace Wagsn.Device.LedScreen
{
    /// <summary>
    /// LED屏设置（包含连接方式和显示设置）
    /// </summary>
    public class LedSettings: NetSeeting, ILedRenderSettings
    {
        /// <summary>
        /// 启用LED屏
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// LED屏宽度，默认64
        /// </summary>
        public int Width { get; set; } = 64;

        /// <summary>
        /// LED屏高度，默认128
        /// </summary>
        public int Height { get; set; } = 128;

        /// <summary>
        /// LED屏颜色类型，默认2 <br/>
        /// 灵信：1.单色  2.双基色  3.三基色     注：C卡全彩参数为3      X系列卡参数固定为 4
        /// </summary>
        public int ColorType { get; set; } = 1;

        /// <summary>
        /// 文本显示X轴偏移，默认-3
        /// </summary>
        public int OffsetX { get; set; } = -3;

        /// <summary>
        /// 字体族名，默认宋体
        /// </summary>
        public string FontFamilyName { get; set; } = "宋体";

        /// <summary>
        /// 字体大小，默认16
        /// </summary>
        public int FontSize { get; set; } = 16;
    }
}
