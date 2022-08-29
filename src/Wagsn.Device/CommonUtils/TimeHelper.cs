using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class TimeHelper
    {
        private const long lLeft = 621355968000000000;

        /// <summary>
        /// Unix时间戳:是从1970年1月1日（UTC/GMT的午夜）开始所经过的秒数，不考虑闰秒
        /// </summary>
        public static long UTCNow
        {
            //Microsoft .NET / C#:epoch = (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000
            //MySQL:SELECT unix_timestamp(now())
            //Java:time
            //JavaScript: Math.round(new Date().getTime()/1000) getTime()返回数值的单位是毫秒
            //SQL Server : SELECT DATEDIFF(s, '1970-01-01 00:00:00', GETUTCDATE())
            get
            {
                return (DateTime.Now.ToUniversalTime().Ticks - lLeft) / 10000000;
            }
        }

        /// <summary>
        /// 将时间转换为时间戳
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static long ToUTCTimeStamp(DateTime time)
        {
            return (time.ToUniversalTime().Ticks - lLeft) / 10000000;
        }

        public static DateTime ToLocalDateTime(long rtime)
        {
            long Eticks = (long)(rtime * 10000000) + lLeft;
            DateTime dt = new DateTime(Eticks).ToLocalTime();
            return dt;
        }

        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static long ConvertDateTimeToInt(System.DateTime time)
        {
            //System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1, 0, 0, 0, 0));
            //long t = (time.Ticks - startTime.Ticks) / 10000;   //除10000调整为13位      
            long t = (time.Ticks - 621356256000000000) / 10000;
            return t;
        }


    }
}
