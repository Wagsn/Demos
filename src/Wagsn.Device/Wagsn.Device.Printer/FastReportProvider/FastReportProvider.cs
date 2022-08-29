using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.Printer.FastReportProvider
{
    /// <summary>
    /// FastReport
    /// </summary>
    public class FastReportProvider : AbstractPrinter
    {
        public bool PrintBitmap(Bitmap bitmap)
        {
            throw new NotImplementedException();
        }

        public bool PrintPdf(string pdfFilePath)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 打印模板
        /// </summary>
        /// <param name="templateName">frx文件名称</param>
        /// <param name="data">填充数据</param>
        /// <returns></returns>
        public bool PrintTemplate(string templateName, Dictionary<string, string> data)
        {
            try
            {
                // frx文件准备
                var frxFileName = templateName;

                // 数据集准备
                var tableName = "Data";
                System.Data.DataSet ds = new System.Data.DataSet();
                ds.Tables.Add(new System.Data.DataTable());
                System.Data.DataTable dt = new System.Data.DataTable
                {
                    TableName = tableName
                };
                foreach (KeyValuePair<string, string> item in data)
                {
                    dt.Columns.Add(item.Key, typeof(string));
                }
                System.Data.DataRow dr = dt.NewRow();
                foreach (KeyValuePair<string, string> item in data)
                {
                    dr[item.Key] = item.Value;
                }
                dt.Rows.Add(dr);

                // 打印
                using (FastReport.Report report = new FastReport.Report())
                {
                    report.Load(frxFileName);
                    report.RegisterData(ds, tableName);
                    report.Prepare();

                    using (FastReport.Export.Pdf.PDFExport pdfExport = new FastReport.Export.Pdf.PDFExport())
                    {
                        var targetFileName = "";
                        report.Export(pdfExport, targetFileName);
                    }

                    // 打印机
                    report.Prepare();
                    report.PrintSettings.Printer = "我的打印机";
                    report.PrintSettings.Copies = 1;
                    report.PrintSettings.ShowDialog = false;
                    report.Print();
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
