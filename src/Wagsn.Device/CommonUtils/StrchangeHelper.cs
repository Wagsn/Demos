using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class StrchangeHelper
    {
        /// <summary>
        /// 字符串转byte
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] strToHexByte(string hexString)
        {
            hexString = hexString.Replace(" ", "");
            if ((hexString.Length % 2) != 0)
                hexString += " ";
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }



        /// <summary>
        /// 把字符串转换成二进制 用空格分开
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string StringtoByte(string data)
        {
            byte[] byteDate = Encoding.Unicode.GetBytes(data);
            StringBuilder stringBuilder = new StringBuilder(byteDate.Length * 8);
            foreach (byte b in byteDate)
            {
                stringBuilder.Append(Convert.ToString(b, 2));
                stringBuilder.Append(" ");
            }
            return stringBuilder.ToString();
        }



        /// <summary>
        /// 十进制转十六进制
        /// </summary>
        /// <param name="dec"></param>
        /// <returns></returns>
        public static string dec2hex(int dec)
        {
            return dec.ToString("X2");
        }


        /// <summary>
        /// 将字符串转换成ASCII码
        /// </summary>
        /// <param name="mHex">转换的字符串</param>
        /// <param name="num">返回的十六进制字符串的位数
        /// 
        /// </param>
        /// <returns>例：“10”若为1则返回A，若为2则返回0A</returns>
        public static string HexToStr(string mHex, int num)
        {
            if (string.IsNullOrEmpty(mHex))
            {
                return "";
            }
            mHex = mHex.Replace(" ", "");
            if (mHex.Length <= 0)
            {
                return null;
            }
            byte[] vBytes = Encoding.Default.GetBytes(mHex);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in vBytes)
            {
                string str = (num == 1) ? b.ToString("X") : b.ToString("X2");
                sb.Append(str);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 将Base64字符串转换为Image对象
        /// </summary>
        /// <param name="base64Code">base64字符串</param>
        /// <returns></returns>
        public static Bitmap Base64ToImg(string base64Code)
        {
            Bitmap bitmap = null;

            try
            {
                byte[] arr = Convert.FromBase64String(base64Code);
                MemoryStream ms = new MemoryStream(arr);
                Bitmap bmp = new Bitmap(ms);
                ms.Close();
                bitmap = bmp;
            }
            catch (Exception ex)
            {
            }

            return bitmap;
        }

        /// <summary>
        /// CRC异或校验 
        /// </summary>
        /// <param name="cmdString">命令字符串</param>
        /// <returns></returns>
        public static string CRC(string cmdString)
        {
            try
            {
                //CRC寄存器
                int CRCCode = 0;
                //将字符串拆分成为16进制字节数据然后两位两位进行异或校验
                for (int i = 1; i < cmdString.Length / 2; i++)
                {
                    string cmdHex = cmdString.Substring(i * 2, 2);
                    if (i == 1)
                    {
                        string cmdPrvHex = cmdString.Substring((i - 1) * 2, 2);
                        CRCCode = (byte)Convert.ToInt32(cmdPrvHex, 16) ^ (byte)Convert.ToInt32(cmdHex, 16);
                    }
                    else
                    {
                        CRCCode = (byte)CRCCode ^ (byte)Convert.ToInt32(cmdHex, 16);
                    }
                }
                return Convert.ToString(CRCCode, 16).ToUpper();//返回16进制校验码
            }
            catch
            {
                throw;
            }
        }
    }
}
