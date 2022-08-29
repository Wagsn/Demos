using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.Printer.FastReportProvider.Demo
{
    public class FastReportProviderDemo
    {
        public static void Print()
        {
            FastReport.Report report = new FastReport.Report();
            report.Load("./FastReportProvider/Demo/Order.frx");
            report.SetParameterValue("客户名", "张三");
            report.SetParameterValue("电话号码", "12345");

            System.Data.DataTable dt = new System.Data.DataTable();
            dt.TableName = "明细";
            dt.Columns.Add("商品名", typeof(string));
            dt.Columns.Add("单价", typeof(string));
            dt.Columns.Add("数量", typeof(string));
            dt.Rows.Add("商品1", 2m, 3m);
            dt.Rows.Add("商品2", 3m, 4m);
            System.Data.DataSet ds = new System.Data.DataSet();
            ds.Tables.Add(dt);

            report.RegisterData(ds);
            report.Prepare();

            // 生成PDF
            var pdfFileName = "./FastReportProvider/Demo/Order.pdf";
            using (FastReport.Export.Pdf.PDFExport pdfExport = new FastReport.Export.Pdf.PDFExport())
            {
                report.Export(pdfExport, pdfFileName);
            }

            // 直接打印
            //report.PrintSettings.Printer = "我的打印机";
            //report.PrintSettings.Copies = 1;
            //report.PrintSettings.ShowDialog = false;
            //report.Print();
        }
    }
}
