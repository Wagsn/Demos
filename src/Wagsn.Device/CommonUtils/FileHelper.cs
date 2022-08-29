using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class FileHelper
    {

        public static bool CreatFile(string dict, string subFolder)
        {
            string pathString = Path.Combine(dict, subFolder);
            // 新文件的完整路径
            try
            {
                if (!File.Exists(pathString))
                {
                    Directory.CreateDirectory(pathString); //创建子文件夹
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}
