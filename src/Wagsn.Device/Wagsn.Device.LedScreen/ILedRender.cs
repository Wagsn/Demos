using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Wagsn.Device.LedScreen
{
    /// <summary>
    /// LED屏位图渲染器
    /// </summary>
    public interface ILedRender<TDisplayInfo>
    {
        /// <summary>
        /// 生成图片
        /// </summary>
        /// <param name="info">显示信息</param>
        /// <returns></returns>
        Bitmap RenderImage(TDisplayInfo info);
    }
}
