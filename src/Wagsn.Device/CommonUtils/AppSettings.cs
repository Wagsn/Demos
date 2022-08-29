using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonUtils
{
    public class AppSettings
    {
        private static SqlHelper sqlHelper = new SqlHelper();
        /// <summary>
        /// webApi地址
        /// </summary>
        public static string ApiHostPort
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiHostPort"];
            }
        }

        /// <summary>
        /// webApi地址
        /// </summary>
        public static string ApiHostIP
        {
            get
            {
                return ConfigurationManager.AppSettings["ApiHostIP"];
            }
        }
        /// <summary>
        /// 服务名称
        /// </summary>
        public static string ServiceName
        {
            get
            {
                return ConfigurationManager.AppSettings["ServiceName"];
            }
        }


        /// <summary>
        /// 司机签名图片路径 
        /// </summary>
        public static string DriverNameFliePath
        {
            get
            {
                return ConfigurationManager.AppSettings["DriverNameFliePath"];
            }

        }



        /// <summary>
        /// 智能过磅后端扫码数据url 
        /// </summary>
        public static string ZNGB_QRCode_ApiUrl
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_QRCode_ApiUrl"];
            }

        }

        /// <summary>
        /// 智能过磅串口服务器磅表IP
        /// </summary>
        public static string ZNGB_Scales_IP
        {
            get
            {
                string usrip = string.Empty;
                string sql = string.Format(@"select beh.IP from Biz_Equipment_Hardware beh where beh.State = 20301 AND beh.TypeID = 20001");
                DataTable dt = sqlHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                     usrip = dt.Rows[0]["IP"].ToString();
                }

                return usrip;
            }
     
        }
    /*        public static string ZNGB_Scales_IP
            {
                get
                {
                    return ConfigurationManager.AppSettings["ZNGB_Scales_IP"];
                }

            }*/


    /// <summary>
    /// 智能过磅串口服务器电磁锁扫码IP
    /// </summary>
    public static string ZNGB_ElecLockQRCode_IP
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_ElecLockQRCode_IP"];
            }
        }


        /// <summary>
        /// 智能过磅串口服务器电机锁扫码IP
        /// </summary>
        public static string ZNGB_MotorLockQRCode_IP
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_MotorLockQRCode_IP"];
            }

        }


        /// <summary>
        /// 智能过磅TCPSocket扫码Port
        /// </summary>
        public static string ZNGB_LockQRCode_Port
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_LockQRCode_Port"];
            }

        }


        /// <summary>
        /// 智能过磅TCPSocket磅表1Port
        /// </summary>
        public static string ZNGB_Scales_Port1
        {
            get
            {
                string usrport1 = string.Empty;
                string sql = string.Format(@"select beh.Port from Biz_Equipment_Hardware beh where beh.State=20301 AND beh.TypeID=20001");
                DataTable dt = sqlHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    usrport1 = dt.Rows[0]["Port"].ToString();
                }

                return usrport1;
            }

        }
/*        public static string ZNGB_Scales_Port1
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_Scales_Port1"];
            }

        }*/

        /// <summary>
        /// 智能过磅TCPSocket磅表2Port
        /// </summary>
        public static string ZNGB_Scales_Port2
        {
            get
            {
                string usrport2= string.Empty;
                string sql = string.Format(@"select beh.Port from Biz_Equipment_Hardware beh where beh.State=20301 AND beh.TypeID=20001");
                DataTable dt = sqlHelper.GetDataTable(sql);
                if (dt.Rows.Count > 1)//多个数据
                {
                    usrport2 = dt.Rows[1]["Port"].ToString();//两个端口取第二个
                }

                return usrport2;
            }

        }
/*        public static string ZNGB_Scales_Port2
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_Scales_Port2"];
            }

        }*/


        /// <summary>
        /// 智能过磅tcp监听端口
        /// </summary>
        public static string ZNGB_Listen_Port
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_Listen_Port"];
            }

        }


        /// <summary>
        /// 智能过磅WEBSocket端口
        /// </summary>
        public static string ZNGB_WEBSocket_Port
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_WEBSocket_Port"];
            }

        }

        /// <summary>
        /// 智能过磅仓位锁控制板板号
        /// </summary>
        public static string ZNGB_LockBoardID
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_LockBoardID"];
            }

        }
        /// <summary>
        /// 智能过磅仓位锁电磁锁
        /// </summary>
        public static string ZNGB_ElecLockID
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_ElecLockID"];
            }

        }


        /// <summary>
        /// 智能过磅仓位锁电机锁
        /// </summary>
        public static string ZNGB_MotorLock_ID
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_MotorLock_ID"];
            }

        }


        /// <summary>
        /// 智能过磅仓位锁控制板端口
        /// </summary>
        public static string ZNGB_LockPort
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_LockPort"];
            }

        }
        /// <summary>
        /// 智能过磅LED显示
        /// </summary>
        public static string ZNGB_LedMesg
        {
            get
            {
                return ConfigurationManager.AppSettings["ZNGB_LedMesg"];
            }

        }
        public static string ZNGB_WebPcUrl 
        {
            get 
            {
                string url = string.Empty;
                string sql = string.Format(@"SELECT GUID,ParameterValue1,Remark from Biz_Stuff_SystemParameterSetting WHERE ParameterName='WebPCUrl' AND Type_2='API'");
                DataTable dt = sqlHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    url = dt.Rows[0]["ParameterValue1"].ToString();
                }

                return url;
            }
        }


        /// <summary>
        /// 智能过磅磅表解析方式
        /// </summary>
        public static string ZNGB_PoundWatch_Analysis
        {
            get
            {
                string usrip = string.Empty;
                string sql = string.Format(@"SELECT GUID,ParameterValue1,Remark from Biz_Stuff_SystemParameterSetting WHERE ParameterName='PoundWatch_Analysis' AND Type_2='API'");
                DataTable dt = sqlHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    usrip = dt.Rows[0]["ParameterValue1"].ToString();
                }

                return usrip;
            }

        }

        /// <summary>
        /// 智能过磅打印单据抬头
        /// </summary>
        public static string ZNGB_Ticket_Title
        {
            get
            {
                string ticketTitle = string.Empty;
                string sql = string.Format(@"SELECT GUID,ParameterValue1,Remark from Biz_Stuff_SystemParameterSetting WHERE ParameterName='TicketTitle' AND Type_2='API'");
                DataTable dt = sqlHelper.GetDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    ticketTitle = dt.Rows[0]["ParameterValue1"].ToString();
                }

                return ticketTitle;
            }
        }

    }
}
