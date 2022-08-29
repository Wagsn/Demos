using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Wagsn.Device.LedScreen
{
    /// <summary>
    /// LED显示控制器
    /// </summary>
    public interface ILedController<TDisplayInfo>
    {
        /// <summary>
        /// 渲染显示
        /// </summary>
        /// <param name="info">显示信息</param>
        /// <returns></returns>
        Bitmap Render(TDisplayInfo info);

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="info">显示信息</param>
        void Display(TDisplayInfo info);
    }
}
