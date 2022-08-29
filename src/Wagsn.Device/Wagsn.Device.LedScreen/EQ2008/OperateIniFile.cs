using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TCZNGB.Core.Infrastucture;
using TCZNGB.Core.Log;

namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    public class OperateIniFile
    {
        #region API函数声明

        [DllImport("kernel32")]//返回0表示失败，非0为成功
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]//返回取得字符串缓冲区的长度
        private static extern long GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);


        #endregion

        #region 读ini文件


        static ConfigFile configFile;// = new ConfigFile("Test.ini");

        public static string ReadIniData(string Section, string Key, string NoText, string iniFilePath)
        {

            if (configFile == null)
            {
                if (File.Exists(iniFilePath))
                {
                    configFile = new ConfigFile(iniFilePath);
                }
                else
                {
                    ILogger logger = ServiceContainer.Resolve<ILogger>();
                    logger.Error(typeof(OperateIniFile), string.Format("EQ2008_Dll_Set.ini 不可用"));
                }
            }

            var value = configFile[Section][Key].AsString();

            return value;


        }

        #endregion

        #region 写ini文件

        public static bool WriteIniData(string Section, string Key, string Value, string iniFilePath)
        {
            try
            {
                configFile = new ConfigFile(iniFilePath);
                configFile[Section][Key].SetValue(Value);
                configFile.Save(iniFilePath);
                return true;
            }
            catch (Exception ex)
            {
                ILogger logger = ServiceContainer.Resolve<ILogger>();
                logger.Error(typeof(OperateIniFile), string.Format("保存{0}时 出错了{1}",Key, ex.Message));
                return false;
            }

        }

        #endregion


    }
}
