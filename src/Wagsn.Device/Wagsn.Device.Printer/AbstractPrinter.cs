using System;

namespace Wagsn.Device.Printer
{
    /// <summary>
    /// 抽象打印机
    /// </summary>
    public interface AbstractPrinter
    {
        /// <summary>
        /// 打印位图
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        bool PrintBitmap(System.Drawing.Bitmap bitmap);

        /// <summary>
        /// 打印PDF
        /// </summary>
        /// <param name="pdfFilePath"></param>
        /// <returns></returns>
        bool PrintPdf(string pdfFilePath);

        /// <summary>
        /// 打印模板
        /// </summary>
        /// <param name="templateName">模板名称</param>
        /// <param name="data">填充数据</param>
        /// <returns></returns>
        bool PrintTemplate(string templateName, System.Collections.Generic.Dictionary<string, string> data);
    }
}
