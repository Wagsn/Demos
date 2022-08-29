using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using TCZNGB.Core.Domain.Response;
using TCZNGB.Core.Infrastucture;
using TCZNGB.Core.Log;

namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    //LED屏幕类型
    public enum LedScreenType
    {
        TankedQueue,        //车辆排队
        MaterialSpace       //料仓
    }



    /// <summary>
    /// LED屏幕的基本操作
    /// </summary>
    public class LedScreen
    {
        ILogger logger = ServiceContainer.Resolve<ILogger>();

        public LedSettings Setting { get; set; }

        public LedScreenType screenType = LedScreenType.TankedQueue; //LED屏幕 默认为车辆排队大屏幕

        public int CardNum = 1;         //控制卡地址
        public int LedGreen = 0xFF00;   //绿色
        public int LedYellow = 0xFFFF;  //黄色
        public int LedRed = 0x00FF;     //红色

        public int ProgramIndex = 0;    //节目序号

        //public int jizuWidth = 0;
        private static string LEDNULL = "                     ";
        private static string LEDMesg = "请下车联系材料员";



        public LedScreen(LedScreenType _screenType, int _cardNum)
        {
            screenType = _screenType;
            CardNum = _cardNum;
            Setting = new LedSettings(_cardNum);
            if (_screenType == LedScreenType.MaterialSpace)
            {
                Setting.InitMaterialGridStyle();
            }
        }




        /// <summary>
        /// 尝试PING屏幕
        /// </summary>
        /// <returns>尝试结果</returns>
        private bool PingIp()
        {
            var ip = Setting.IpAddress0 + '.' + Setting.IpAddress1 + '.' + Setting.IpAddress2 + '.' + Setting.IpAddress3;
            try
            {
                var ipSuccess = NetUtil.PingIp(ip, Setting.PingTimeOut);
                return ipSuccess;
            }
            catch (Exception ex)
            {
                logger.Error(typeof(NetUtil).GetType(), string.Format("ip: {0}连接错误：{1}" ,ip,ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 发送欢迎信息
        /// </summary>
        public void SendWelcomeInfo()
        {
            try
            {
                lock (ScreenEnableSetting.GlobalLockObj)
                {

                    if (PingIp() == false)
                    {
                        return;
                    }

                    int x = 0;
                    var y = (Setting.ScreemHeight - Setting.FixedHeight * 2) / 2;
                    //一行字12-----y
                    //两行字26-----y
                    //三行字39-----y
                    SendText(x, y, Setting.ScreemWidth, Setting.FixedHeight * 2, "砼创事业部(2秒后消失),\n当前屏幕为:" + CardNum.ToString(), 1, 1, Setting.FixedFontSize);
                }
            }
            catch (Exception ex)
            {
                logger.Error(this.GetType(), string.Format("发送欢迎信息错误：{0}", ex.Message));
            }
        }


        

        /// <summary>
        /// 清楚欢迎信息
        /// </summary>
        public void ClearWelcomeInfo()
        {
            lock (ScreenEnableSetting.GlobalLockObj)
            {
                if (PingIp() == false)
                {
                    return;
                }

                int x = 0;
                var y = (Setting.ScreemHeight - Setting.FixedHeight * 2) / 2;
                SendText(x, y, Setting.ScreemWidth, Setting.FixedHeight * 2, " ", 1, 1, Setting.FixedFontSize);
            }
        }


        public bool SendWeigh(PlayLedMesgModel model)
        {
            lock (ScreenEnableSetting.GlobalLockObj)
            {
                string message1 = "";
                string message2 = "";
                string message3 = "";
                string message4 = "";
                switch (model.tips)
                {
                    case "1":
                        {
                             message1 = string.Format("车牌号:{0}", model.CarNum);
                             message2 = string.Format("重  量:{0}", model.Weigh);
                             message3 = string.Format("材  料:{0}", model.RawMaterials);
                             message4 = string.Format("请前往:{0} 卸料", model.WareHouse);
                        }; break;
                    case "2": 
                        {
                             message1 = "未关联到计划";
                             message2 = LEDNULL;
                             message3 = LEDMesg;
                             message4 = LEDNULL;
                        }; break;
                    case "3":
                        {
                            message1 = "未识别到车牌";
                            message2 = LEDNULL;
                            message3 = LEDMesg;
                            message4 = LEDNULL;
                        }; break;
                    case "4":
                        {
                             message1 = "过磅失败";
                             message2 = "当前车辆没有分配仓位";
                             message3 = LEDMesg;
                             message4 = LEDNULL;
                        } break;
                }


                if (PingIp() == false)
                {
                    return false;
                }

                int x_carnum = 0;
                var y_carnum = 0;
                bool SendSuccess1 = SendText(x_carnum, y_carnum, Setting.ScreemWidth, Setting.FixedHeight , message1, 0, 0, Setting.FixedFontSize);

                int x_weigh = 0;
                var y_weigh = 16;
                bool SendSuccess2 = SendText(x_weigh, y_weigh, Setting.ScreemWidth, Setting.FixedHeight, message2, 0, 0, Setting.FixedFontSize);

                int x_rawmaterials = 0;
                var y_rawmaterials = 32;
                bool SendSuccess3 = SendText(x_rawmaterials, y_rawmaterials, Setting.ScreemWidth, Setting.FixedHeight, message3, 0, 0, Setting.FixedFontSize);

                int x_warehouse = 0;
                var y_warehouse = 48;
                bool SendSuccess4 = SendText(x_warehouse, y_warehouse, Setting.ScreemWidth, Setting.FixedHeight, message4, 0, 0, Setting.FixedFontSize);
                return (SendSuccess1 && SendSuccess2 && SendSuccess3 && SendSuccess4);
            }
        }

        public bool SendWeigh_wc(PlayLedMesgModel model)
        {
            lock (ScreenEnableSetting.GlobalLockObj)
            {
                string message1 = "";
                string message2 = "";
                string message3 = "";
                string message4 = "";
                switch (model.tips)
                {
                    case "1":
                        {
                            message1 = string.Format("车牌:{0}", model.CarNum);
                            message2 = string.Format("重量:{0}", model.Weigh);
                            message3 = string.Format("请前往:{0}卸料", model.WareHouse);
                           
                        }; break;
                    case "2":
                        {
                            message1 = "未关联到计划";
                            message2 = LEDMesg;
                            message3 = LEDNULL;
                          
                        }; break;
                    case "3":
                        {
                            message1 = "未识别到车牌";
                            message2 = LEDMesg;
                            message3 = LEDNULL;
                          
                        }; break;
                    case "4":
                        {
                            message1 = "过磅失败";
                            message2 = "当前车辆没有分配仓位";
                            message3 = LEDMesg;
                           
                        }
                        break;
                }


                if (PingIp() == false)
                {
                    return false;
                }

                int x_carnum = 0;
                var y_carnum = 0;
                bool SendSuccess1 = SendText(x_carnum, y_carnum, Setting.ScreemWidth, Setting.FixedHeight, message1, 0, 0, Setting.FixedFontSize);

                int x_weigh = 0;
                var y_weigh = 16;
                bool SendSuccess2 = SendText(x_weigh, y_weigh, Setting.ScreemWidth, Setting.FixedHeight, message2, 0, 0, Setting.FixedFontSize);

                int x_rawmaterials = 0;
                var y_rawmaterials = 32;
                bool SendSuccess3 = SendText(x_rawmaterials, y_rawmaterials, Setting.ScreemWidth, Setting.FixedHeight, message3, 0, 0, Setting.FixedFontSize);

                return (SendSuccess1 && SendSuccess2 && SendSuccess3);
            }
        }



        public bool CleanSendWeigh(string str)
        {

            lock (ScreenEnableSetting.GlobalLockObj)
            {
                string message1 = LEDNULL;
                string message2 = LEDNULL;
                string message3 = LEDNULL;
                string message4 = LEDNULL;

                if (PingIp() == false)
                {
                    return false;
                }
                int x_carnum = 0;
                var y_carnum = 0;
                bool SendSuccess1 = SendText(x_carnum, y_carnum, Setting.ScreemWidth, Setting.FixedHeight, message1,0 , 0, Setting.FixedFontSize);

                int x_weigh = 0;
                var y_weigh = 16;
                bool SendSuccess2 = SendText(x_weigh, y_weigh, Setting.ScreemWidth, Setting.FixedHeight, message2, 0, 0, Setting.FixedFontSize);

                int x_rawmaterials = 0;
                var y_rawmaterials = 32;
                bool SendSuccess3 = SendText(x_rawmaterials, y_rawmaterials, Setting.ScreemWidth, Setting.FixedHeight, message3, 0, 0, Setting.FixedFontSize);

                int x_warehouse = 0;
                var y_warehouse = 48;
                bool SendSuccess4 = SendText(x_warehouse, y_warehouse, Setting.ScreemWidth, Setting.FixedHeight, message4, 0, 0, Setting.FixedFontSize);
                return (SendSuccess1 && SendSuccess2 && SendSuccess3 && SendSuccess4);
            }
        }
        /// <summary>
        /// 发送文本内容
        /// </summary>
        /// <param name="message"></param>
        public bool Sendmessage(string message)
        {
            lock (ScreenEnableSetting.GlobalLockObj)
            {
                int count = 0;
                if (PingIp() == false)
                {
                    return false;
                }
                if (message.Length > 10)
                {
                    int n = message.Length / 10;
                    for (int i = 1; i <= n; i++)
                    {
                    
                        message = message.Insert(11 * i - 1, "\n");

                    }

                    count = message.Split(new string[] { "\n" }, StringSplitOptions.None).Length - 1;
                }
                int lines = count < 7 ? 1 + count : 7;
                int x = 0;
                var y = (Setting.ScreemHeight) < (Setting.FixedHeight * lines) ?  0 : ((Setting.ScreemHeight - Setting.FixedHeight * lines) / 2);
                bool SendSuccess = SendText(x, y, Setting.ScreemWidth, Setting.FixedHeight * lines, message, 0, 0, Setting.FixedFontSize);
                return SendSuccess;
            }
        }



        /// <summary>  
        /// 关闭屏幕
        /// </summary>
        public bool  CloseScreen()
        {
            try
            {
                lock (ScreenEnableSetting.GlobalLockObj)
                {
                    if (PingIp() == false)
                    {
                        return false;
                    }
                    if (LedAPI.User_CloseScreen(CardNum) == false)
                    {
                        logger.Error(this.GetType(), string.Format("关闭显示屏失败！"));
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(this.GetType(), string.Format("关闭屏幕错误：{0}", ex.Message));
                return false;
            }

        }



        /// <summary>
        /// 打开屏幕
        /// </summary>
        public bool OpenScreen()
        {
            try
            {
                lock (ScreenEnableSetting.GlobalLockObj)
                {
                    if (PingIp() == false)
                    {
                        logger.Error(this.GetType(), string.Format("网络不通"));
                        return false;
                    }

                    if (LedAPI.User_OpenScreen(CardNum) == false)
                    {
                        logger.Error(this.GetType(), string.Format("打开显示屏失败！"));
                        return false;

                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(this.GetType(), string.Format("打开屏幕错误：{0}", ex.ToString()));
                return false;
            }
        }

        /// <summary>
        /// 清空屏幕
        /// </summary>
        public bool ClearScreen()
        {
            try
            {
                lock (ScreenEnableSetting.GlobalLockObj)
                {
                    if (PingIp() == false)
                    {
                        return false;
                    }

                    if (!LedAPI.User_RealtimeScreenClear(CardNum))
                    {
                        logger.Error(this.GetType(), string.Format("清空控制卡节目失败"));
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                logger.Error(this.GetType(), string.Format("清空屏幕错误：{0}", ex.Message));
                return false;
            }
        }

        /// <summary>
        /// 把文字加入到节目中去
        /// </summary>
        /// <param name="chContent"></param>
        private void AddTextToProgram(string chContent)
        {
            try
            {
                lock (ScreenEnableSetting.GlobalLockObj)
                {
                    User_Text Text = new User_Text();

                    Text.BkColor = 0;
                    Text.chContent = chContent;

                    Text.PartInfo.FrameColor = 0;
                    Text.PartInfo.iFrameMode = 0;
                    Text.PartInfo.iHeight = Setting.GunDongHeight;
                    Text.PartInfo.iWidth = Setting.ScreemWidth;
                    Text.PartInfo.iX = 0;
                    Text.PartInfo.iY = Setting.ScreemHeight - Setting.GunDongHeight;

                    Text.FontInfo.bFontBold = Setting.GunDongbFontBold;
                    Text.FontInfo.bFontItaic = false;
                    Text.FontInfo.bFontUnderline = false;
                    Text.FontInfo.colorFont = LedYellow;
                    Text.FontInfo.iFontSize = Setting.GunDongFontSize;
                    Text.FontInfo.strFontName = "宋体";// LedSettings.GunDongFont;
                    Text.FontInfo.iAlignStyle = 0;
                    Text.FontInfo.iVAlignerStyle = 0;
                    Text.FontInfo.iRowSpace = 0;

                    Text.MoveSet.bClear = false;
                    Text.MoveSet.iActionSpeed = Setting.GunDongiActionSpeed;
                    Text.MoveSet.iActionType = Setting.GunDongiActionType;
                    Text.MoveSet.iHoldTime = 0;
                    Text.MoveSet.iFrameTime = 20;

                    if (-1 == LedAPI.User_AddText(CardNum, ref Text, ProgramIndex))
                    {
                        logger.Error(this.GetType(), string.Format("AddTextToProgram-添加文本失败"));
                    }
                }
            }
            catch (Exception ex)
            {

                logger.Error(this.GetType(), string.Format("把文字加入到节目中错误：{0}", ex.Message));
            }
        }

        /// <summary>
        /// 发送节目
        /// </summary>
        /// <param name="chContent">节目内容</param>
        public void SendProgram(string chContent)
        {
            try
            {
                lock (ScreenEnableSetting.GlobalLockObj)
                {
                    if (!Setting.GunDongIsShow)
                    {
                        return;
                    }

                    if (PingIp() == false)
                    {
                        return;
                    }

                    //Setting.ReloadSettingDBModel();

                    if (LedAPI.User_DelAllProgram(CardNum) == false)
                    {
                        logger.Error(this.GetType(), string.Format("删除节目失败"));
                        return;
                    }

                    ProgramIndex = LedAPI.User_AddProgram(CardNum, false, 10);

                    AddTextToProgram(chContent);

                    if (LedAPI.User_SendToScreen(CardNum) == false)
                    {
                        logger.Error(this.GetType(), string.Format("User_SendToScreen-发送节目失败"));
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(this.GetType(), string.Format("发送节目错误：{0}", ex.Message));
            }

        }



        /// <summary>
        /// 发送文字
        /// </summary>
        /// <param name="iX">文字开始的X位置</param>
        /// <param name="iY">文字开始的Y位置</param>
        /// <param name="iWidth">更新区域的宽</param>
        /// <param name="iHeight">更新区域的高</param>
        /// <param name="strText">文字</param>
        /// <param name="iAlignStyle">上下对齐方式</param>
        /// <param name="iVAlignerStyle">水平对齐方式</param>
        /// <param name="fontSize">字体大小</param>
        private bool SendText(int iX, int iY, int iWidth, int iHeight, string strText, int iAlignStyle, int iVAlignerStyle, int fontSize)
        {
            var mes = string.Format("SendText:iX={0},iY={1},iWidth={2},iHeight={3},strText={4}", iX, iY, iWidth, iHeight, strText);

            //logger.Error(this.GetType(), string.Format("发送文字：{0}", mes));

            lock (ScreenEnableSetting.GlobalLockObj)
            {
                User_FontSet FontInfo = new User_FontSet();
                FontInfo.bFontBold = Setting.FixedbFontBold;
                FontInfo.bFontItaic = false;
                FontInfo.bFontUnderline = false;
                FontInfo.colorFont = LedRed;
                FontInfo.iFontSize = fontSize;
                FontInfo.strFontName = "宋体";// LedSettings.FixedFont;
                FontInfo.iAlignStyle = iAlignStyle;
                FontInfo.iVAlignerStyle = iVAlignerStyle;
                FontInfo.iRowSpace = 0;

                if (LedAPI.User_RealtimeConnect(CardNum))
                {
                    if (LedAPI.User_RealtimeSendText(CardNum, iX, iY, iWidth, iHeight, strText, ref FontInfo))
                    {
                        if (!LedAPI.User_RealtimeDisConnect(CardNum))
                        {
                            logger.Error(this.GetType(), string.Format("错误：SendText.User_RealtimeDisConnect-断开失败"));
                            return false;
                        }
                    }
                    else 
                    {
                        logger.Error(this.GetType(), string.Format("错误：SendText.User_RealtimeSendText-发送实时文本失败"));
                        return false;
                    }
                }
                else
                {
                    logger.Error(this.GetType(), string.Format("错误：SendText.User_RealtimeConnect-连接失败"));
                    return false;
                }
                return true;

            }
        }





    }
}
