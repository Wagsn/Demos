using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace TCZNGB.Service.ZNGB_PlayLedMesg
{
    public class LedSettings
    {
        public LedSettings(int sectionIndex)
        {
            ScreenIndex = sectionIndex;
            section = string.Format("地址：{0}", sectionIndex - 1);
            noText = "0";
            iniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EQ2008_Dll_Set.ini");
            MaterialGridStyle = new GridStyle();

            //ReloadSettingDBModel();
        }

        public LedSettings()
        {
            ScreenIndex = 0;
            section = "地址：0";
            noText = "0";
            iniFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "EQ2008_Dll_Set.ini");
            MaterialGridStyle = new GridStyle();
        }

 

        public GridStyle MaterialGridStyle { get; set; }

        public string section { get; set; }
        public string noText { get; set; }
        public string iniFilePath { get; set; }
        public int ScreenIndex { get; set; }

        /// <summary>
        /// 显示设置是从配置文件来,还是从数据库里来
        /// </summary>
 

        //控制卡类型
        public string CardType
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "CardType", noText, iniFilePath);
            }
        }

        //控制卡地址
        public string CardAddress
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "CardAddress", noText, iniFilePath);
            }
        }

        //通讯模式
        public string CommunicationMode
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "CommunicationMode", noText, iniFilePath);
            }
        }

        //屏幕高度
        public int ScreemHeight
        {
            get
            {
                var screemHeight = OperateIniFile.ReadIniData(section, "ScreemHeight", noText, iniFilePath);
                if (!string.IsNullOrEmpty(screemHeight))
                {
                    return Convert.ToInt32(screemHeight);
                }
                else { return 48; }
            }
        }

        //屏幕宽度
        public int ScreemWidth
        {
            get
            {
                var screemWidth = OperateIniFile.ReadIniData(section, "ScreemWidth", noText, iniFilePath);
                if (!string.IsNullOrEmpty(screemWidth))
                {
                    return Convert.ToInt32(screemWidth);
                }
                else { return 96; }
            }
        }

        public string SerialBaud
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "SerialBaud", noText, iniFilePath);
            }
        }

        public string SerialNum
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "SerialNum", noText, iniFilePath);
            }
        }

        //端口
        public string NetPort
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "NetPort", noText, iniFilePath);
            }
        }

        //IP地址1
        public string IpAddress0
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "IpAddress0", noText, iniFilePath);
            }
        }

        //IP地址2
        public string IpAddress1
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "IpAddress1", noText, iniFilePath);
            }
        }

        //IP地址3
        public string IpAddress2
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "IpAddress2", noText, iniFilePath);
            }
        }

        //IP地址4
        public string IpAddress3
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "IpAddress3", noText, iniFilePath);
            }
        }

        //颜色类型
        public string ColorStyle
        {
            get
            {
                return OperateIniFile.ReadIniData(section, "ColorStyle", noText, iniFilePath);
            }
        }

        //固定高度
        public int FixedHeight
        {
            get
            {

                var fixedHeight = OperateIniFile.ReadIniData(section, "固定区行高", noText, iniFilePath);
                if (!string.IsNullOrEmpty(fixedHeight))
                {
                    return Convert.ToInt32(fixedHeight);
                }
                else { return 48; }
            }
        }

        //固定宽度
        public string FixedFont
        {
            get
            {

                var fixedFont = OperateIniFile.ReadIniData(section, "固定区字体", noText, iniFilePath);
                if (!string.IsNullOrEmpty(fixedFont))
                {
                    return fixedFont;
                }
                else { return "宋体"; }
            }
        }

        //固定字体大小
        public int FixedFontSize
        {
            get
            {

                var fixedFontSize = OperateIniFile.ReadIniData(section, "固定区字号", noText, iniFilePath);
                if (!string.IsNullOrEmpty(fixedFontSize))
                {
                    return Convert.ToInt32(fixedFontSize);
                }
                else { return 9; }
            }
        }

        //固定字体是否加宽
        public bool FixedbFontBold
        {
            get
            {

                var bFontBold = OperateIniFile.ReadIniData(section, "固定区粗体", noText, iniFilePath);
                if (!string.IsNullOrEmpty(bFontBold))
                {
                    return Convert.ToBoolean(bFontBold == "1" ? "True" : "False");
                }
                else { return false; }
            }
        }

        //机组标题高度
        public int JiZuTitleHeight
        {
            get
            {

                var JiZuTitleHeight = OperateIniFile.ReadIniData(section, "机组标题行高", noText, iniFilePath);
                if (!string.IsNullOrEmpty(JiZuTitleHeight))
                {
                    return Convert.ToInt32(JiZuTitleHeight);
                }
                else { return 48; }
            }
        }

        //机组标题字体大小
        public int JiZuTitleFontSize
        {
            get
            {

                var jiZuTitleFontSize = OperateIniFile.ReadIniData(section, "机组标题字号", noText, iniFilePath);
                if (!string.IsNullOrEmpty(jiZuTitleFontSize))
                {
                    return Convert.ToInt32(jiZuTitleFontSize);
                }
                else { return 9; }
            }
        }

        //左槽陆佰
        public int PadingLeft
        {
            get
            {

                var padingLeft = OperateIniFile.ReadIniData(section, "固定区左边距", noText, iniFilePath);
                if (!string.IsNullOrEmpty(padingLeft))
                {
                    return Convert.ToInt32(padingLeft);
                }
                else { return 0; }
            }
        }

        //等待后面空白
        public int WaitAfterEmpty
        {
            get
            {

                var witAfterEmpty = OperateIniFile.ReadIniData(section, "等待后面空白", noText, iniFilePath);
                if (!string.IsNullOrEmpty(witAfterEmpty))
                {
                    return Convert.ToInt32(witAfterEmpty);
                }
                else { return 0; }
            }
        }

        //是否显示滚动字幕
        public bool GunDongIsShow
        {
            get
            {
                //查询数据库是否滚动字幕
                //var isStop = SettingHelper.GetAppSettingFromDB("QueueScreenRollingSubtitle1");
                var isStop = "0";
                if (string.IsNullOrEmpty(isStop) || isStop == "1")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        //滚动字体
        public string GunDongFont
        {
            get
            {

                var gunDongFont = OperateIniFile.ReadIniData(section, "滚动信息字体", noText, iniFilePath);
                if (!string.IsNullOrEmpty(gunDongFont))
                {
                    return gunDongFont;
                }
                else { return "宋体"; }
            }
        }

        //滚动字体大小
        public int GunDongFontSize
        {
            get
            {
  
                var jiZuTitleFontSize = OperateIniFile.ReadIniData(section, "滚动信息字号", noText, iniFilePath);
                if (!string.IsNullOrEmpty(jiZuTitleFontSize))
                {
                    return Convert.ToInt32(jiZuTitleFontSize);
                }
                else { return 9; }
            }
        }

        //滚动字体是否加宽
        public bool GunDongbFontBold
        {
            get
            {

                var bFontBold = OperateIniFile.ReadIniData(section, "滚动信息粗体", noText, iniFilePath);
                if (!string.IsNullOrEmpty(bFontBold))
                {
                    return Convert.ToBoolean(bFontBold == "1" ? "True" : "False");
                }
                else { return false; }
            }
        }

        //滚动条高度
        public int GunDongHeight
        {
            get
            {

                var gunDongHeight = OperateIniFile.ReadIniData(section, "滚动信息行高", noText, iniFilePath);
                if (!string.IsNullOrEmpty(gunDongHeight))
                {
                    return Convert.ToInt32(gunDongHeight);
                }
                else { return 48; }
            }

        }

        //滚动方式
        public int GunDongiActionType
        {
            get
            {

                var iActionType = OperateIniFile.ReadIniData(section, "滚动信息变化模式", noText, iniFilePath);
                if (!string.IsNullOrEmpty(iActionType))
                {
                    return Convert.ToInt32(iActionType);
                }
                else { return 2; }
            }
        }

        //滚动速度
        public int GunDongiActionSpeed
        {
            get
            {

                var iActionSpeed = OperateIniFile.ReadIniData(section, "滚动信息速度", noText, iniFilePath);
                if (!string.IsNullOrEmpty(iActionSpeed))
                {
                    return Convert.ToInt32(iActionSpeed);
                }
                else { return 5; }
            }
        }

        //超时时间
        public int PingTimeOut
        {
            get
            {
                var pingTimeOut = OperateIniFile.ReadIniData(section, "PingTimeOut", noText, iniFilePath);
                if (!string.IsNullOrEmpty(pingTimeOut))
                {
                    return Convert.ToInt32(pingTimeOut);
                }
                else { return 200; }
            }
        }

        //回写INI文件
        public void writeTextIni(string key, string writeText)
        {
            //OperateIniFile.WriteIniData(section, key, writeText, iniFilePath);
        }

        //初始化标识牌相关数据内容
        public void InitMaterialGridStyle()
        {

                string strFontSize = OperateIniFile.ReadIniData(section, "表格字体大小", noText, iniFilePath);
                string strLeftMargin = OperateIniFile.ReadIniData(section, "表格字体左边偏移", noText, iniFilePath);
                string strTopMargin = OperateIniFile.ReadIniData(section, "表格字体上方偏移", noText, iniFilePath);
                string strMaxCharNum = OperateIniFile.ReadIniData(section, "表格字数上限", noText, iniFilePath);
                if (!string.IsNullOrEmpty(strFontSize))
                {
                    MaterialGridStyle.fontSize = int.Parse(strFontSize);
                }
                if (!string.IsNullOrEmpty(strLeftMargin))
                {
                    MaterialGridStyle.leftMargin = int.Parse(strLeftMargin);
                }
                if (!string.IsNullOrEmpty(strTopMargin))
                {
                    MaterialGridStyle.topMargin = int.Parse(strTopMargin);
                }
                if (!string.IsNullOrEmpty(strMaxCharNum))
                {
                    MaterialGridStyle.MaxCharNum = int.Parse(strMaxCharNum);
                }
                MaterialGridStyle.GridWidth = ScreemWidth;
                MaterialGridStyle.GridHeight = ScreemHeight;
            
        }
    }

}
