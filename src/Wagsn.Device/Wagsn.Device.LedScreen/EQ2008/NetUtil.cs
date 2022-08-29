using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using TCZNGB.Core.Infrastucture;
using TCZNGB.Core.Log;

namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    public class NetUtil
    {
     
        public static bool PingIp(string ip, int timeout = 200)
        {
            ILogger logger = ServiceContainer.Resolve<ILogger>();
            try
            {
                if (string.IsNullOrEmpty(ip))
                {
                    return false;
                }
                using (Ping pingSender = new Ping())
                {
                    PingReply reply = pingSender.Send(ip, timeout);//第一个参数为ip地址，第二个参数为ping的时间 
                    if (reply.Status == IPStatus.Success)
                    {
                        //ping的通 
                        return true;
                    }
                    else
                    {
                        //ping不通 
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(typeof(NetUtil).GetType(), string.Format("ip: {0}地址错误{1}", ip, ex.Message));
                return false;
            }
        }
    }

}
