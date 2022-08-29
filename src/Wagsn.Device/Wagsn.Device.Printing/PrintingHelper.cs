using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wagsn.Device.Printing
{
    /// <summary>
    /// 打印机帮助器
    /// </summary>
    public class PrintingHelper
    {
        /// <summary>
        /// 打印PDF文件
        /// </summary>
        /// <param name="pdfUri">
        /// PDF文件URI，格式如下：<br/>
        /// http://localhost:7855/userresource/text.pdf <br/>
        /// file:///E:/Code/text.pdf</param>
        /// <param name="printerName">打印机名称（应用部署所在电脑能访问的打印机）</param>
        public static void Print(string pdfUri, string printerName)
        {
            if (string.IsNullOrWhiteSpace(pdfUri))
            {
                throw new ArgumentNullException(nameof(pdfUri), "PDF文件URI不能为空");
            }
            if (string.IsNullOrWhiteSpace(printerName))
            {
                throw new ArgumentNullException(nameof(printerName), "打印机名称不能为空");
            }
            if (!Uri.IsWellFormedUriString(pdfUri, UriKind.RelativeOrAbsolute))
            {
                throw new ArgumentException(nameof(pdfUri), "PDF文件URI格式错误");
            }

            using (Spire.Pdf.PdfDocument pdfDoc = new Spire.Pdf.PdfDocument())
            {
                // Spire.Pdf 打印PDF文件
                using (WebClient webClient = new WebClient())
                {
                    var pdfBytes = webClient.DownloadData(pdfUri);
                    pdfDoc.LoadFromBytes(pdfBytes); // FreeSpire.PDF 仅支持10页PDF打印
                }
                pdfDoc.PrintSettings.PrintController = new StandardPrintController();
                pdfDoc.PrintSettings.PrinterName = printerName;
                pdfDoc.PrintSettings.Color = false; // 黑白打印
                pdfDoc.PrintSettings.Copies = 1; // 打印份数
                //pdfDoc.PrintSettings.Collate  // 逐份打印
                pdfDoc.PrintSettings.Landscape = false; // 纵向打印
                // 支持的尺寸要看打印机属性  new PrintDocument().PrinterSettings.PaperSizes
                // 单位转换  220mm*110mm, 1in=25.4mm, 72dpi, ppi/dpi: dot per in.  A3 在72dpi下是 841*1190, 10mm对应28px
                pdfDoc.PrintSettings.PaperSize = new PaperSize("949W300H", (int)(2200 / 25.4 * 100), (int)(1100 / 25.4 * 100));
                using (PrintDocument printDoc = new PrintDocument())
                {
                    foreach (PaperSize pageSize in printDoc.PrinterSettings.PaperSizes) // 
                    {
                        if (pageSize.PaperName == "949W300H")
                        {
                            pdfDoc.PrintSettings.PaperSize = pageSize;
                        }
                    }
                }
                pdfDoc.Print();
            }

            #region << 其它参考代码 >>
            // 网络打印 无法访问
            //var bytes = File.ReadAllBytes(pdfUrl);
            //SendPdf(bytes, printerName);

            // 命令打印 无效
            //PrintDocument pd = new PrintDocument();
            //pd.PrinterSettings.PrinterName = printerName;
            //Process p = new Process
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        CreateNoWindow = false,
            //        WindowStyle = ProcessWindowStyle.Hidden,
            //        UseShellExecute = true,
            //        FileName = pdfUrl,//文件路径
            //        Verb = "print",
            //        Arguments = @"/p /h """ + pdfUrl + "\" \"" + pd.PrinterSettings.PrinterName + "\""
            //    }
            //};
            //p.Start();

            // PdfiumPrinter 打印
            //var printer = new PdfiumPrinter.PdfPrinter(printerName);
            //var printFile = pdfUrl; //The path to the pdf which needs to be printed;
            //printer.Print(printFile);

            // System.Drawing.Printing 打印出来为空白
            //PrintDocument printDocument = new PrintDocument();
            //printDocument.DocumentName = pdfUrl;
            //printDocument.PrintController = new StandardPrintController();
            //printDocument.PrinterSettings.PrinterName = printerName;
            //printDocument.PrinterSettings.PrintFileName = pdfUrl;
            //printDocument.PrinterSettings.PrintRange = PrintRange.AllPages;
            //printDocument.Print();
            #endregion
        }

        /// <summary>
        /// 获取打印机列表
        /// </summary>
        /// <returns></returns>
        public static List<string> GetPrinters()
        {
            var printers = new List<string>();
            foreach (string strprinter in PrinterSettings.InstalledPrinters)
            {
                printers.Add(strprinter);
            }
            return printers;
        }

        public void SetupPrintHandler()
        {
            PrintDocument printDoc = new PrintDocument();
            printDoc.PrintPage += new PrintPageEventHandler(OnPrintPage);

            printDoc.Print();
        }

        private void OnPrintPage(object sender, PrintPageEventArgs args)
        {
            using (Image image = Image.FromFile(@"C:\file.jpg"))
            {
                Graphics g = args.Graphics;
                g.DrawImage(image, 0, 0);
            }
        }

        ////Send data to local IP (Raw printer port)
        //static void SendPdf(byte[] text, string ipAddress)
        //{

        //    IPEndPoint ip = new IPEndPoint(IPAddress.Parse(ipAddress), 9100);
        //    Socket server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        //    try
        //    {
        //        server.Connect(ip);
        //    }
        //    catch (SocketException e)
        //    {
        //        Console.WriteLine($"Error connecting to {ipAddress}");
        //        return;
        //    }

        //    server.Send(text);
        //    Thread.Sleep(1000); //Some printers need a bit late before shutdown the connection
        //    server.Shutdown(SocketShutdown.Both);
        //    server.Close();
        //}
    }
}
