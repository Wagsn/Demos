using IntelligentHardware.Domain.Request;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

namespace Wagsn.Device.LedScreen.Lingxin
{
    /// <summary>
    /// 博硕料仓信息LED渲染器
    /// </summary>
    public class LingxinStorageLedRender : ILedRender<StorageDisplayInfo>
    {
        // 显示画面 128 * 64
        // -------------------------------
        // | 料仓编号 ①  | 进场日期 ⑤  |
        // -------------------------------
        // | 材料名称 ②  | 进场数量 ⑥  |
        // -------------------------------
        // | 规格型号 ③  | 检验状态 ⑦  |
        // -------------------------------
        // | 生产厂家 ④  | 库存数量 ⑧  |
        // -------------------------------

        // 内部编排 64 * 128
        // ----------------
        // | 料仓编号 ①  |
        // ----------------
        // | 材料名称 ②  |
        // ----------------
        // | 规格型号 ③  |
        // ----------------
        // | 生产厂家 ④  |
        // ----------------
        // | 进场日期 ⑤  |
        // ----------------
        // | 进场数量 ⑥  |
        // ----------------
        // | 检验状态 ⑦  |
        // ----------------
        // | 库存数量 ⑧  |
        // ----------------

        private readonly ILedRenderSettings _ledSettings;

        public LingxinStorageLedRender(ILedRenderSettings ledSettings)
        {
            _ledSettings = ledSettings;
        }

        /// <summary>
        /// 生成图片
        /// </summary>
        /// <returns></returns>
        public Bitmap RenderImage(StorageDisplayInfo info)
        {
            string fixedFont = _ledSettings.FontFamilyName;
            int fontSize = _ledSettings.FontSize;
            int offsetX = _ledSettings.OffsetX;
            int width = _ledSettings.Width;
            int height = _ledSettings.Height;

            Bitmap bitmap = new Bitmap(width, height);
            Graphics graph = Graphics.FromImage(bitmap);
            Font font = new Font(fixedFont, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);

            // 清空为黑色
            graph.Clear(Color.Black);

            // 字符串格式
            StringFormat format = new StringFormat();
            format.Alignment = StringAlignment.Near;
            format.LineAlignment = StringAlignment.Near;

            // 绘制内容
            graph.DrawString(info.StorageNo, font, Brushes.White, offsetX, 0 * fontSize, format);
            graph.DrawString(info.MaterialName, font, Brushes.White, offsetX, 1 * fontSize, format);
            graph.DrawString(info.MaterialSpec, font, Brushes.White, offsetX, 2 * fontSize, format);
            graph.DrawString(info.Supplier, font, Brushes.White, offsetX, 3 * fontSize, format);
            graph.DrawString(info.MobilizationDate, font, Brushes.White, offsetX, 4 * fontSize, format);
            graph.DrawString(info.MobilizationQuantity, font, Brushes.White, offsetX, 5 * fontSize, format);
            graph.DrawString(info.InspectionStatus, font, Brushes.White, offsetX, 6 * fontSize, format);
            graph.DrawString(info.InventoryQuantity, font, Brushes.White, offsetX, 7 * fontSize, format);

            graph.Save();

            return bitmap;
        }
    }
}
