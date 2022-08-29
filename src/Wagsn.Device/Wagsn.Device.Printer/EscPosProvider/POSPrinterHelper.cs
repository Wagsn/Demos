using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.Printer.EpsPosProvider
{
    public class POSPrinterHelper
    {
        public string CunGen = "(存根)";
        public string PingZheng = "(凭证)";


        public bool POS_SUCCESS = false;
        public string POS_Init = "1B40";// 初始化打印机
        public string POS_FeedLine = "0A";// 打印并走纸一行
        public string POS_FeedLine_n = "1B4A";// 打印并走纸N行
        public string POS_SetBarcodeQR = "1D6F000C0002";
        public string POS_cutpaper = "1D56";//切纸

        public string POS_PageMode = "1B4C";//页模式
        public string POS_P_FeedLine = "0C";//页模式下打印缓冲缓冲区里的内容
        public string CompanyTitle = "中建西部建设有限公司";//公司
        public string Line = "5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F5F ";//横线
        public string Data = "C8D5C6DAA3BA ";//日期
        public string TIme = "";//时间
        public string PlantStation = "B9FDB0F5B5A5";//厂站
        public string WareHouse = "B2D6BFE2A3BA";//仓库
        public string Suppliers = "B9A9D3A6C9CCB5A5CEBBA3BA";//供应商单位
        public string FormNumber = "B9FDB0F5B5A5BAC5A3BA ";//过磅单号
        public string RawMaterials = "D4ADB2C4C1CFC3FBB3C6A3BA";//原材料名称
        public string CarLicnese = "B3B5BAC5A3BA";//车号
        public string Specifications = "B9E6B8F1D0CDBAC5A3BA";//规格型号
        public string TotalWeight = "C3ABD6D8A3BA";//毛重
        public string CarWeight = "C6A4D6D8A3BA";//皮重
        public string MaterialsWeight = "BEBBD6D8A3BA";//净重
        public string SignWeight = "C7A9CAD5C1BFA3BA";//签收量
        public string DUN = "B6D6";//吨
        public string DeliverWeight = "BFCDBBA7B7A2BBF5C1BFA3BA";//客户发货量
        public string FirstWeighingTime = " B3C6D6D8CAB1BCE4A3BA";//称重时间
        public string LastWeighingTime = "C8A5C6A4CAB1BCE4A3BA";//去皮时间
        public string Remarks = "B1B8D7A2A3BA ";//备注
        public string QualityInspector = "D6CABCECD4B1A3BA ";//质检员
        public string Materialman = "B2C4C1CFD4B1A3BA";//材料员
        public string Driver = "CBBEBBFAA3BA";//司机

  

        /// <summary>
        /// 断开打印机
        /// </summary>
        /// <returns></returns>
        public bool POS_Close()
        {
            return POS_SUCCESS = false;
        }


        /// <summary>
        /// 打印N行
        /// </summary>
        /// <returns></returns>
        public string POS_FeedLineN(int n)
        {

            return POS_FeedLine_n + StrchangeHelper.dec2hex(n);
        }

        /// <summary>
        ///切纸
        /// </summary>
        /// <param name="mode">切纸模式
        /// 0：全切
        /// 1：半切</param>
        /// <returns></returns>
        public string POS_CutPaper(int mode)
        {
            string str = null;
            switch (mode)
            {
                case 0: str = "00"; break;
                case 1: str = "01"; break;
                default: break;
            }
            return POS_cutpaper + str;
        }

        /// <summary>
        /// 进纸并且半切
        /// </summary>
        /// <param name="mode">切纸模式为66</param>
        /// <param name="move">进纸尺寸</param>
        /// <returns></returns>
        public string POS_CutPaper(int mode, int move)
        {
            string str = null;
            if (mode == 66)
            {
                str = StrchangeHelper.dec2hex(mode);
                str += StrchangeHelper.dec2hex(move);
            }
            return POS_cutpaper + str;
        }

        /// <summary>
        /// 打印二维码
        /// </summary>
        /// <returns></returns>
        public string POS_S_BarcodeQR(string num)
        {

            return "1D6B0B51412C" + StrchangeHelper.HexToStr(num,1) + "00";
        }

        /// <summary>
        /// 设置条码属性
        /// </summary>
        /// <returns></returns>
        public string POS_Set_BarcodeQR()
        {
            return "1D6F000C0002";
        }

        /// <summary>
        /// 打印文本
        /// </summary>
        /// <returns></returns>
        public string POS_S_TextOut(string text)
        {
            return StrchangeHelper.HexToStr(text,1);
        }

        /// <summary>
        /// 设置字符大小
        /// </summary>
        /// <param name="n">
        /// 00 正常
        /// 11 1倍
        /// 取值范围00~55
        /// </param>
        /// <returns></returns>
        public string POS_Set_CharacterSize(int n)
        {
            string str = "1D21";
            switch (n)
            {
                case 0: str += "00"; break;
                case 1: str += "11"; break;
                case 2: str += "22"; break;
                case 3: str += "33"; break;
                case 4: str += "44"; break;
                case 5: str += "55"; break;
                default: break;
            }
            return str;
        }

        /// <summary>
        /// 设置页模式下的打印方向
        /// </summary>
        /// <param name="direction">
        /// 0：由左到右
        /// 1：由下到上
        /// 2：由右到左
        /// 3：由上到下
        /// </param>
        /// <returns></returns>
        public string POS_P_PrintingDirection(int direction)
        {
            string str = "1B54";
            switch (direction)
            {
                case 0: str += "00"; break;
                case 1: str += "01"; break;
                case 2: str += "02"; break;
                case 3: str += "03"; break;
                default: break;
            }
            return str;
        }

        /// <summary>
        /// 页模式设置打印区域
        /// </summary>
        /// <param name="xL"></param>
        /// <param name="xH"></param>
        /// <param name="yL"></param>
        /// <param name="yH"></param>
        /// <param name="dxL"></param>
        /// <param name="dxH"></param>
        /// <param name="dyL"></param>
        /// <param name="dyH"></param>
        /// <returns></returns>
        public string POS_P_SetArea(int xL, int xH, int yL, int yH, int dxL, int dxH, int dyL, int dyH)
        {
            string str = "1B57";
            return str += StrchangeHelper.dec2hex(xL) + StrchangeHelper.dec2hex(xH) + StrchangeHelper.dec2hex(yL) + StrchangeHelper.dec2hex(yH) + StrchangeHelper.dec2hex(dxL) + StrchangeHelper.dec2hex(dxH) + StrchangeHelper.dec2hex(dyL) + StrchangeHelper.dec2hex(dyH);
        }

        /// <summary>
        /// 设置页模式下的纵向绝对位置
        /// </summary>
        /// <param name="nL"></param>
        /// <param name="nH"></param>
        /// <returns></returns>
        public string POS_P_VerticalAbsolute(int nL, int nH)
        {
            string str = "1D24";
            return str += StrchangeHelper.dec2hex(nL) + StrchangeHelper.dec2hex(nH);
        }

        /// <summary>
        /// 设置页模式下的纵向绝对位置
        /// </summary>
        /// <param name="nL"></param>
        /// <param name="nH"></param>
        /// <returns></returns>
        public string POS_P_TransverseAbsolute(int nL, int nH)
        {
            string str = "1B24";
            return str += StrchangeHelper.dec2hex(nL) + StrchangeHelper.dec2hex(nH);
        }
    }

}
