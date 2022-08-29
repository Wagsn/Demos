using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.Printer.EpsPosProvider
{
    /// <summary>
    /// ESC/POS协议适配
    /// </summary>
    public class EscPosProvider : AbstractPrinter
    {
        public bool PrintBitmap(Bitmap bitmap)
        {
            throw new NotImplementedException();
        }

        public bool PrintPdf(string pdfFilePath)
        {
            throw new NotImplementedException();
        }

        public bool PrintTemplate(string templateName, Dictionary<string, string> data)
        {
            throw new NotImplementedException();
        }
    }
}
