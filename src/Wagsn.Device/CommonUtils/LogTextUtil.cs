using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
   public class LogTextUtil
    {
        StreamWriter sw;

        public LogTextUtil(string fileName)
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileName);

            if (File.Exists(file))
            {
                File.Delete(file);
            }
            sw = File.AppendText(file);
        }

        public void AppendText(string text)
        {
            text += " " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            sw.WriteLine(text);
        }

        public void Close()
        {
            sw.Flush();
            sw.Dispose();
        }


    }
}
