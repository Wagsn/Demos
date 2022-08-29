using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.LedScreen
{
    /// <summary>
    /// EQ2008 LED屏控制
    /// </summary>
    public class EQ2008_Dll
    {
        #region //==========================1、节目操作函数======================//
        /// <summary>
        /// 添加节目
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="bWaitToEnd"></param>
        /// <param name="iPlayTime"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern int User_AddProgram(int CardNum, Boolean bWaitToEnd, int iPlayTime);

        /// <summary>
        /// 删除所有节目
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_DelAllProgram(int CardNum);

        /// <summary>
        /// 添加单行文本区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pSingleText"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern int User_AddSingleText(int CardNum, ref User_SingleText pSingleText, int iProgramIndex);

        /// <summary>
        /// 添加文本区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pText"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern int User_AddText(int CardNum, ref User_Text pText, int iProgramIndex);

        /// <summary>
        /// 添加时间区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pdateTime"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern int User_AddTime(int CardNum, ref User_DateTime pdateTime, int iProgramIndex);

        /// <summary>
        /// 添加图文区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pBmp"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern int User_AddBmpZone(int CardNum, ref User_Bmp pBmp, int iProgramIndex);

        /// <summary>
        /// 指定图像句柄添加图片
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="iBmpPartNum"></param>
        /// <param name="hBitmap"></param>
        /// <param name="pMoveSet"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern bool User_AddBmp(int CardNum, int iBmpPartNum, IntPtr hBitmap, ref User_MoveSet pMoveSet, int iProgramIndex);

        /// <summary>
        /// 指定图像路径添加图片
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="iBmpPartNum"></param>
        /// <param name="strFileName"></param>
        /// <param name="pMoveSet"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern bool User_AddBmpFile(int CardNum, int iBmpPartNum, string strFileName, ref User_MoveSet pMoveSet, int iProgramIndex);

        /// <summary>
        /// 添加计时区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pTimeCount"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern int User_AddTimeCount(int CardNum, ref User_Timer pTimeCount, int iProgramIndex);

        /// <summary>
        /// 添加温度区
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="pTemperature"></param>
        /// <param name="iProgramIndex"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern int User_AddTemperature(int CardNum, ref User_Temperature pTemperature, int iProgramIndex);

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_SendToScreen(int CardNum);
        #endregion

        #region //=======================2、实时发送数据（高频率发送）=================//
        /// <summary>
        /// 实时建立连接
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_RealtimeConnect(int CardNum);

        /// <summary>
        /// 实时发送图片数据
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="hBitmap"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_RealtimeSendData(int CardNum, int x, int y, int iWidth, int iHeight, IntPtr hBitmap);

        /// <summary>
        /// 实时发送图片文件
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="strFileName"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_RealtimeSendBmpData(int CardNum, int x, int y, int iWidth, int iHeight, string strFileName);

        /// <summary>
        /// 实时发送文本
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="iWidth"></param>
        /// <param name="iHeight"></param>
        /// <param name="strText"></param>
        /// <param name="pFontInfo"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_RealtimeSendText(int CardNum, int x, int y, int iWidth, int iHeight, string strText, ref User_FontSet pFontInfo);

        /// <summary>
        /// 实时关闭连接
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_RealtimeDisConnect(int CardNum);

        /// <summary>
        /// 实时发送清屏
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_RealtimeScreenClear(int CardNum);
        #endregion

        #region //==========================3、显示屏控制函数组=======================//
        /// <summary>
        /// 校正时间
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_AdjustTime(int CardNum);

        /// <summary>
        /// 开屏
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_OpenScreen(int CardNum);

        /// <summary>
        /// 关屏
        /// </summary>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_CloseScreen(int CardNum);

        /// <summary>
        /// 亮度调节
        /// </summary>
        /// <param name="CardNum"></param>
        /// <param name="iLightDegreen"></param>
        /// <returns></returns>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern Boolean User_SetScreenLight(int CardNum, int iLightDegreen);

        /// <summary>
        /// Reload参数文件
        /// </summary>
        /// <param name="strEQ2008_Dll_Set_Path"></param>
        [DllImport("EQ2008_Dll.dll", CharSet = CharSet.Ansi)]
        public static extern void User_ReloadIniFile(string strEQ2008_Dll_Set_Path);
        #endregion
    }

    /// <summary>
    /// 节目区域参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_PartInfo
    {
        public int iX;                    //窗口的起点X
        public int iY;                    //窗口的起点Y
        public int iWidth;                //窗体的宽度
        public int iHeight;               //窗体的高度
        public int iFrameMode;            //边框的样式
        public int FrameColor;            //边框颜色
    }

    /// <summary>
    /// 字体参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_FontSet
    {
        public string strFontName;       //字体的名称
        public int iFontSize;            //字体的大小
        public bool bFontBold;           //字体是否加粗
        public bool bFontItaic;          //字体是否是斜体
        public bool bFontUnderline;      //字体是否带下划线
        public int colorFont;            //字体的颜色
        public int iAlignStyle;          //左右对齐方式，0－ 左对齐，1－居中，2－右对齐
        public int iVAlignerStyle;       //上下对齐方式，0-顶对齐，1-上下居中，2-底对齐    
        public int iRowSpace;            //行间距
    }

    /// <summary>
    /// 动画方式参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_MoveSet
    {
        public int iActionType;             //节目变换方式
        public int iActionSpeed;            //节目的播放速度
        public bool bClear;                 //是否需要清除背景
        public int iHoldTime;               //在屏幕上停留的时间
        public int iClearSpeed;		        //清除显示屏的速度
        public int iClearActionType;	    //节目清除的变换方式
        public int iFrameTime;              //每帧时间
    }

    /// <summary>
    /// 日期时间区参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_DateTime
    {
        public User_PartInfo PartInfo;   //分区信息
        public int BkColor;              //背景颜色
        public User_FontSet FontInfo;    //字体设置
        public int iDisplayType;         //显示风格 0－"3/10/2006 星期六10:20:30",1－"2006-03-10星期六10:20:30",2－"2006年3月10日 星期六10点20分30秒"
        public string chTitle;           //添加显示文字
        public bool bYearDisType;        //年份位数0－4,1－2位
        public bool bMulOrSingleLine;    //单行还是多行,0－单行1－多行
        public bool bYear;               //是否显示年
        public bool bMouth;              //是否显示月
        public bool bDay;                //是否显示天
        public bool bWeek;               //是否显示星期
        public bool bHour;               //是否显示小时
        public bool bMin;                //是否显示分钟
        public bool bSec;                //是否显示秒
    }

    /// <summary>
    /// 单行文本区参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_SingleText
    {
        public string chContent;            //显示内容
        public User_PartInfo PartInfo;      //分区信息
        public int BkColor;                 //背景颜色
        public User_FontSet FontInfo;       //字体设置
        public User_MoveSet MoveSet;        //动作方式设置
    }

    /// <summary>
    /// 文本区参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_Text
    {
        public string chContent;            //显示内容
        public User_PartInfo PartInfo;      //分区信息
        public int BkColor;                 //背景颜色
        public User_FontSet FontInfo;       //字体设置
        public User_MoveSet MoveSet;        //动作方式设置
    }

    /// <summary>
    /// 计时区参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_Timer
    {
        public User_PartInfo PartInfo;	//分区信息
        public int BkColor;			    //背景颜色
        public User_FontSet FontInfo;	//字体设置
        public int ReachTimeYear;		//到达年
        public int ReachTimeMonth;	    //到达月
        public int ReachTimeDay;		//到达日
        public int ReachTimeHour;		//到达时
        public int ReachTimeMinute;	    //到达分
        public int ReachTimeSecond;	    //到达秒
        public bool bDay;				//是否显示天 0－不显示 1－显示
        public bool bHour;				//是否显示小时
        public bool bMin;				//是否显示分钟
        public bool bSec;				//是否显示秒
        public bool bMulOrSingleLine;	//单行还是多行
        public string chTitle;			//添加显示文字
    }

    /// <summary>
    /// 温度区参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_Temperature
    {
        public User_PartInfo PartInfo;		//分区信息
        public int BkColor;			        //背景颜色
        public User_FontSet FontInfo;		//字体设置
        public string chTitle;			    //标题
        public int DisplayType;		        //显示格式：0－度 1－C
    }

    /// <summary>
    /// 图文区参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_Bmp
    {
        public User_PartInfo PartInfo;		//分区信息
    }

    /// <summary>
    /// RTF文件区参数
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct User_RTF
    {
        public string strFileName;      //RTF文件名
        public User_PartInfo PartInfo;	//分区信息
        public User_MoveSet MoveSet;	//动作方式设置
    }
}
