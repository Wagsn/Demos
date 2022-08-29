using IntelligentHardware.Domain.Request;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Wagsn.Device.Core;

namespace Wagsn.Device.LedScreen.Yangbang
{
    /// <summary>
    /// 仰邦 BX-6系列 LED显示 过磅显示控制
    /// </summary>
    public class YangbangWeighingLedController : IWeighingLedController
    {
        private readonly LedSettings _ledSettings;

        public YangbangWeighingLedController(LedSettings ledSettings)
        {
            _ledSettings = ledSettings;
        }

        /// <summary>
        /// 显示文本
        /// </summary>
        /// <param name="info"></param>
        public void Display(WeighingInfo info)
        {
            if (!_ledSettings.Enabled)
            {
                Console.WriteLine("已关闭过磅LED显示");
                return;
            }
            var dir = AppDomain.CurrentDomain.BaseDirectory + $"temp\\image";
            var path = AppDomain.CurrentDomain.BaseDirectory + $"temp\\image\\{DateTime.Now:yyyyMMddHHmmssfff}.png";
            try
            {
                using (var displayImg = Render(info))
                {
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }
                    displayImg.Save(path);
                }
                SendToDevice(_ledSettings, path);
            }
            catch (Exception ex)
            {
                throw new Exception($"发送到仰邦LED屏失败（配置：{Newtonsoft.Json.JsonConvert.SerializeObject(_ledSettings)}，内容：{Newtonsoft.Json.JsonConvert.SerializeObject(info)}）", ex);
            }
            finally
            {
                Thread thread = new Thread(a => DeleteImage(path));
                thread.Start();
            }
        }


        /// <summary>
        /// BX-6代控制卡发送节目文本
        /// </summary>
        /// <param name="setting">网络配置</param>
        /// <param name="text">文本</param>
        public static void Send_program_txt_6(INetSeeting setting, string text)
        {
            // 参数
            byte[] ip = Encoding.GetEncoding("GBK").GetBytes(setting?.RemoteIp ?? "192.168.0.199");
            ushort port = setting?.RemotePort ?? 5005;
            string displayText = text;
            byte[] com = Encoding.GetEncoding("GBK").GetBytes(setting?.PortName ?? "COM3");
            // 串口波特率 1：9600  2：57600
            byte baudRate = (byte)(setting?.BaudRate == 9600 ? 1 : 2);
            // 通讯方式 true=网口  false=串口
            bool check = setting?.NetMode != 4;

            //初始化动态库
            int err = bxdualsdk.bxDual_InitSdk();

            //指定IP ping控制卡获取控制卡数据，屏参相关参数已知的情况可省略该步骤
            bxdualsdk.Ping_data data = new bxdualsdk.Ping_data();
            if (check)
            {
                err = bxdualsdk.bxDual_cmd_tcpPing(ip, port, ref data);
            }
            else
            {
                err = bxdualsdk.bxDual_cmd_uart_searchController(ref data, com);
            }
            Console.WriteLine("ControllerType:0x" + data.ControllerType.ToString("X2"));
            Console.WriteLine("FirmwareVersion:V" + System.Text.Encoding.Default.GetString(data.FirmwareVersion));
            Console.WriteLine("ipAdder:" + System.Text.Encoding.Default.GetString(data.ipAdder));
            Console.WriteLine("ScreenWidth:" + data.ScreenWidth.ToString());
            Console.WriteLine("ScreenHeight:" + data.ScreenHeight.ToString());
            Console.WriteLine("cmb_ping_Color:" + data.Color.ToString());
            Console.WriteLine("\r\n");

            //显示屏屏基色
            byte cmb_ping_Color = 1;
            if (data.Color == 1) { cmb_ping_Color = 1; }
            else if (data.Color == 3) { cmb_ping_Color = 2; }
            else if (data.Color == 7) { cmb_ping_Color = 3; }
            else { cmb_ping_Color = 4; }

            //第一步.设置屏幕参数相关  发送节目必要接口，发送动态区可忽略
            err = bxdualsdk.bxDual_program_setScreenParams_G56((bxdualsdk.E_ScreenColor_G56)cmb_ping_Color, data.ControllerType, bxdualsdk.E_DoubleColorPixel_G56.eDOUBLE_COLOR_PIXTYPE_1);
            Console.WriteLine("bxDual_program_setScreenParams_G56:" + err);

            //第二步，创建节目，设置节目属性
            bxdualsdk.EQprogramHeader_G6 header;
            header.FileType = 0x00;
            header.ProgramID = 0;
            header.ProgramStyle = 0x00;
            header.ProgramPriority = 0x00;
            header.ProgramPlayTimes = 1;
            header.ProgramTimeSpan = 0;
            header.SpecialFlag = 0;
            header.CommExtendParaLen = 0x00;
            header.ScheduNum = 0;
            header.LoopValue = 0;
            header.Intergrate = 0x00;
            header.TimeAttributeNum = 0x00;
            header.TimeAttribute0Offset = 0x0000;
            header.ProgramWeek = 0xff;
            header.ProgramLifeSpan_sy = 0xffff;
            header.ProgramLifeSpan_sm = 0x03;
            header.ProgramLifeSpan_sd = 0x14;
            header.ProgramLifeSpan_ey = 0xffff;
            header.ProgramLifeSpan_em = 0x03;
            header.ProgramLifeSpan_ed = 0x14;
            header.PlayPeriodGrpNum = 0;
            err = bxdualsdk.bxDual_program_addProgram_G6(ref header);
            Console.WriteLine("bxDual_program_addProgram_G6:" + err);

            //第三步，创建显示分区，设置区域显示位置，示例创建一个区域编号为0，区域大小64*32的图文分区
            bxdualsdk.EQareaHeader_G6 aheader;
            aheader.AreaType = 0;
            aheader.AreaX = 16;
            aheader.AreaY = 0;
            aheader.AreaWidth = (ushort)(data.ScreenWidth - 16); // 软件设置屏参最小宽度80
            aheader.AreaHeight = data.ScreenHeight;
            aheader.BackGroundFlag = 0x00;
            aheader.Transparency = 101;
            aheader.AreaEqual = 0x00;
            bxdualsdk.EQSound_6G stSoundData = new bxdualsdk.EQSound_6G();//该语音属性在节目无效
            stSoundData.SoundFlag = 0;
            stSoundData.SoundVolum = 0;
            stSoundData.SoundSpeed = 0;
            stSoundData.SoundDataMode = 0;
            stSoundData.SoundReplayTimes = 0;
            stSoundData.SoundReplayDelay = 0;
            stSoundData.SoundReservedParaLen = 0;
            stSoundData.Soundnumdeal = 0;
            stSoundData.Soundlanguages = 0;
            stSoundData.Soundwordstyle = 0;
            stSoundData.SoundDataLen = 0;
            byte[] t = new byte[1];
            t[0] = 0;
            stSoundData.SoundData = IntPtr.Zero;
            aheader.stSoundData = stSoundData;
            err = bxdualsdk.bxDual_program_addArea_G6(0, ref aheader);  //添加图文区域
            Console.WriteLine("bxDual_program_addArea_G6:" + err);

            //第四步，添加显示内容，此处为图文分区0添加字符串
            byte[] Font = Encoding.GetEncoding("GBK").GetBytes("宋体");
            IntPtr font = Marshal.AllocHGlobal(Font.Length);
            Marshal.Copy(Font, 0, font, Font.Length);
            byte[] strAreaTxtContent = Encoding.GetEncoding("GBK").GetBytes(displayText);
            IntPtr str = Marshal.AllocHGlobal(strAreaTxtContent.Length);
            Marshal.Copy(strAreaTxtContent, 0, str, strAreaTxtContent.Length);
            bxdualsdk.EQpageHeader_G6 pheader;
            pheader.PageStyle = 0x00;
            pheader.DisplayMode = 0x01;//移动模式
            pheader.ClearMode = 0x01;
            pheader.Speed = 15;//速度
            pheader.StayTime = 500;//停留时间
            pheader.RepeatTime = 1;
            pheader.ValidLen = 0;
            pheader.CartoonFrameRate = 0x00;
            pheader.BackNotValidFlag = 0x00;
            pheader.arrMode = bxdualsdk.E_arrMode.eMULTILINE;
            pheader.fontSize = 12; // 10 以下会糊
            pheader.color = 0x02;
            pheader.fontBold = 0;
            pheader.fontItalic = 0;
            pheader.tdirection = bxdualsdk.E_txtDirection.pNORMAL;
            pheader.txtSpace = 0;
            pheader.Valign = 1;
            pheader.Halign = 1;
            err = bxdualsdk.bxDual_program_picturesAreaAddTxt_G6(0, strAreaTxtContent, Font, ref pheader);
            Console.WriteLine("bxDual_program_picturesAreaAddTxt_G6:" + err);

            //第五步，发送节目到显示屏
            bxdualsdk.EQprogram_G6 program = new bxdualsdk.EQprogram_G6();
            err = bxdualsdk.bxDual_program_IntegrateProgramFile_G6(ref program);
            Console.WriteLine("bxDual_program_IntegrateProgramFile_G6:" + err);
            err = bxdualsdk.bxDual_program_deleteProgram_G6();
            Console.WriteLine("bxDual_program_deleteProgram_G6:" + err);

            if (check)//网口
            {
                err = bxdualsdk.bxDual_cmd_ofsStartFileTransf(ip, port);
                Console.WriteLine("bxDual_cmd_ofsStartFileTransf:" + err);

                err = bxdualsdk.bxDual_cmd_ofsWriteFile(ip, port, program.dfileName, program.dfileType, program.dfileLen, 1, program.dfileAddre);
                Console.WriteLine("bxDual_cmd_ofsWriteFile:" + err);
                if (err != 0) { return; }
                err = bxdualsdk.bxDual_cmd_ofsWriteFile(ip, port, program.fileName, program.fileType, program.fileLen, 1, program.fileAddre);
                Console.WriteLine("bxDual_cmd_ofsWriteFile:" + err);
                err = bxdualsdk.bxDual_cmd_ofsEndFileTransf(ip, port);
                Console.WriteLine("bxDual_cmd_ofsEndFileTransf:" + err);
            }
            else//串口
            {
                err = bxdualsdk.bxDual_cmd_uart_ofsStartFileTransf(com, baudRate);
                Console.WriteLine("bxDual_cmd_uart_ofsStartFileTransf:" + err);

                err = bxdualsdk.bxDual_cmd_uart_ofsWriteFile(com, baudRate, program.dfileName, program.dfileType, program.dfileLen, 1, program.dfileAddre);
                Console.WriteLine("bxDual_cmd_uart_ofsWriteFile:" + err);
                err = bxdualsdk.bxDual_cmd_uart_ofsWriteFile(com, baudRate, program.fileName, program.fileType, program.fileLen, 1, program.fileAddre);
                Console.WriteLine("bxDual_cmd_uart_ofsWriteFile:" + err);
                err = bxdualsdk.bxDual_cmd_uart_ofsEndFileTransf(com, baudRate);
            }

            err = bxdualsdk.bxDual_program_freeBuffer_G6(ref program);
            Console.WriteLine("bxDual_program_freeBuffer_G6:" + err);

            //释放动态库
            bxdualsdk.bxDual_ReleaseSdk();
        }

        /// <summary>
        /// BX-6代控制卡发送节目图片
        /// </summary>
        public static void SendToDevice(INetSeeting setting, string path)
        {
#if NETSTANDARD2_0_OR_GREATER
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#endif
            // 参数
            byte[] ip = Encoding.GetEncoding("GB2312").GetBytes(setting?.RemoteIp ?? "192.168.0.199");
            ushort port = setting?.RemotePort ?? 5005;
            byte[] com = Encoding.GetEncoding("GB2312").GetBytes(setting?.PortName ?? "COM3");
            // 串口波特率 1：9600  2：57600
            byte baudRate = (byte)(setting?.BaudRate == 9600 ? 1 : 2);
            // 通讯方式 true=网口  false=串口
            bool check = setting?.NetMode != 4;

            //初始化动态库
            int err = bxdualsdk.bxDual_InitSdk();

            //指定IP ping控制卡获取控制卡数据，屏参相关参数已知的情况可省略该步骤
            bxdualsdk.Ping_data data = new bxdualsdk.Ping_data();
            err = bxdualsdk.bxDual_cmd_tcpPing(ip, port, ref data);

            //显示屏屏基色
            byte cmb_ping_Color = 1;
            if (data.Color == 1) { cmb_ping_Color = 1; }
            else if (data.Color == 3) { cmb_ping_Color = 2; }
            else if (data.Color == 7) { cmb_ping_Color = 3; }
            else { cmb_ping_Color = 4; }

            //第一步.设置屏幕参数相关  发送节目必要接口，发送动态区可忽略
            err = bxdualsdk.bxDual_program_setScreenParams_G56((bxdualsdk.E_ScreenColor_G56)cmb_ping_Color, data.ControllerType, bxdualsdk.E_DoubleColorPixel_G56.eDOUBLE_COLOR_PIXTYPE_1);
            Console.WriteLine("bxDual_program_setScreenParams_G56:" + err);

            //第二步，创建节目，设置节目属性
            bxdualsdk.EQprogramHeader_G6 header;
            header.FileType = 0x00;
            header.ProgramID = 0;
            header.ProgramStyle = 0x00;
            header.ProgramPriority = 0x00;
            header.ProgramPlayTimes = 1;
            header.ProgramTimeSpan = 0;
            header.SpecialFlag = 0;
            header.CommExtendParaLen = 0x00;
            header.ScheduNum = 0;
            header.LoopValue = 0;
            header.Intergrate = 0x00;
            header.TimeAttributeNum = 0x00;
            header.TimeAttribute0Offset = 0x0000;
            header.ProgramWeek = 0xff;
            header.ProgramLifeSpan_sy = 0xffff;
            header.ProgramLifeSpan_sm = 0x03;
            header.ProgramLifeSpan_sd = 0x14;
            header.ProgramLifeSpan_ey = 0xffff;
            header.ProgramLifeSpan_em = 0x03;
            header.ProgramLifeSpan_ed = 0x14;
            header.PlayPeriodGrpNum = 0;
            err = bxdualsdk.bxDual_program_addProgram_G6(ref header);
            Console.WriteLine("bxDual_program_addProgram_G6:" + err);

            //第三步，创建显示分区，设置区域显示位置，示例创建一个区域编号为0，区域大小64*32的图文分区
            bxdualsdk.EQareaHeader_G6 aheader;
            aheader.AreaType = 0;
            aheader.AreaX = (ushort)((data.ScreenWidth > 80) ? 0 : 16);
            aheader.AreaY = 0;
            aheader.AreaWidth = (ushort)((data.ScreenWidth > 80) ? data.ScreenWidth : (data.ScreenWidth - 16)); // 软件设置屏参最小宽度80
            aheader.AreaHeight = data.ScreenHeight;
            aheader.BackGroundFlag = 0x00;
            aheader.Transparency = 101;
            aheader.AreaEqual = 0x00;
            bxdualsdk.EQSound_6G stSoundData = new bxdualsdk.EQSound_6G();
            stSoundData.SoundFlag = 0;
            stSoundData.SoundVolum = 0;
            stSoundData.SoundSpeed = 0;
            stSoundData.SoundDataMode = 0;
            stSoundData.SoundReplayTimes = 0;
            stSoundData.SoundReplayDelay = 0;
            stSoundData.SoundReservedParaLen = 0;
            stSoundData.Soundnumdeal = 0;
            stSoundData.Soundlanguages = 0;
            stSoundData.Soundwordstyle = 0;
            stSoundData.SoundDataLen = 0;
            byte[] t = new byte[1];
            t[0] = 0;
            stSoundData.SoundData = IntPtr.Zero;
            aheader.stSoundData = stSoundData;
            err = bxdualsdk.bxDual_program_addArea_G6(0, ref aheader);
            Console.WriteLine("bxDual_program_addArea_G6:" + err);

            //第四步，添加显示内容，此处为图文分区0添加图片，该步骤可多次调用，添加多张图片，每张图片用不同的编号
            byte[] img = Encoding.GetEncoding("GB2312").GetBytes(path);
            bxdualsdk.EQpageHeader_G6 pheader;
            pheader.PageStyle = 0x00;
            pheader.DisplayMode = 0x01;//移动模式
            pheader.ClearMode = 0x01;
            pheader.Speed = 15;//速度
            pheader.StayTime = 0;//停留时间
            pheader.RepeatTime = 1;
            pheader.ValidLen = 0;
            pheader.CartoonFrameRate = 0x00;
            pheader.BackNotValidFlag = 0x00;
            pheader.arrMode = bxdualsdk.E_arrMode.eSINGLELINE;
            pheader.fontSize = 12;
            pheader.color = (uint)0x01;
            pheader.fontBold = 0;
            pheader.fontItalic = 0;
            pheader.tdirection = bxdualsdk.E_txtDirection.pNORMAL;
            pheader.txtSpace = 0;
            pheader.Valign = 1;
            pheader.Halign = 1;
            err = bxdualsdk.bxDual_program_pictureAreaAddPic_G6(0, 0, ref pheader, img);
            Console.WriteLine("bxDual_program_pictureAreaAddPic_G6:" + err);

            //第五步，发送节目到显示屏
            bxdualsdk.EQprogram_G6 program = new bxdualsdk.EQprogram_G6();
            err = bxdualsdk.bxDual_program_IntegrateProgramFile_G6(ref program);
            Console.WriteLine("bxDual_program_IntegrateProgramFile_G6:" + err);
            err = bxdualsdk.bxDual_program_deleteProgram_G6();
            Console.WriteLine("bxDual_program_deleteProgram_G6:" + err);

            if (check)//网口
            {
                err = bxdualsdk.bxDual_cmd_ofsStartFileTransf(ip, port);
                Console.WriteLine("bxDual_cmd_ofsStartFileTransf:" + err);

                err = bxdualsdk.bxDual_cmd_ofsWriteFile(ip, port, program.dfileName, program.dfileType, program.dfileLen, 1, program.dfileAddre);
                Console.WriteLine("bxDual_cmd_ofsWriteFile:" + err);
                err = bxdualsdk.bxDual_cmd_ofsWriteFile(ip, port, program.fileName, program.fileType, program.fileLen, 1, program.fileAddre);
                Console.WriteLine("bxDual_cmd_ofsWriteFile:" + err);
                err = bxdualsdk.bxDual_cmd_ofsEndFileTransf(ip, port);
                Console.WriteLine("bxDual_cmd_ofsEndFileTransf:" + err);
            }
            else//串口
            {
                err = bxdualsdk.bxDual_cmd_uart_ofsStartFileTransf(com, baudRate);
                Console.WriteLine("bxDual_cmd_uart_ofsStartFileTransf:" + err);

                err = bxdualsdk.bxDual_cmd_uart_ofsWriteFile(com, baudRate, program.dfileName, program.dfileType, program.dfileLen, 1, program.dfileAddre);
                Console.WriteLine("bxDual_cmd_uart_ofsWriteFile:" + err);
                err = bxdualsdk.bxDual_cmd_uart_ofsWriteFile(com, baudRate, program.fileName, program.fileType, program.fileLen, 1, program.fileAddre);
                Console.WriteLine("bxDual_cmd_uart_ofsWriteFile:" + err);
                err = bxdualsdk.bxDual_cmd_uart_ofsEndFileTransf(com, baudRate);
            }

            err = bxdualsdk.bxDual_program_freeBuffer_G6(ref program);
            Console.WriteLine("bxDual_program_freeBuffer_G6:" + err);

            //释放动态库
            bxdualsdk.bxDual_ReleaseSdk();

        }

        /// <summary>
        /// 删除生成的临时图片
        /// </summary>
        /// <param name="path"></param>
        private static void DeleteImage(string path)
        {
            try
            {
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("删除图片失败！" + path + "，" + ex);
                //MyLog4NetInfo.Error(GetType(), ex);
            }
        }

        public Bitmap Render(WeighingInfo info)
        {// 绘制字体
            string fixedFont = _ledSettings.FontFamilyName;
            int fontSize = _ledSettings.FontSize;
            var offsetX = _ledSettings.OffsetX;
            // 960*480-p5  640*320-p10
            var width = _ledSettings.Width;
            var height = _ledSettings.Height;

            Bitmap displayImg = new Bitmap(width, height); // 640*320-p10
            // 画布
            Graphics _G = Graphics.FromImage(displayImg);
            _G.Clear(Color.Black);

            StringFormat _format = new StringFormat();
            _format.Alignment = StringAlignment.Near;
            _format.LineAlignment = StringAlignment.Near;
            _format.Trimming = StringTrimming.EllipsisPath;

            var _font = new Font(fixedFont, fontSize, FontStyle.Regular, GraphicsUnit.Pixel);

            if (width == 64)
            {
                _G.DrawString("聚羧酸剂", _font, Brushes.White, offsetX, 0, _format);
                _G.DrawString("19:30:55", _font, Brushes.White, offsetX, 16, _format);
            }
            else if (width == 192)
            {
                // 16-3 18-5 20-5 22-6 24-6 26-6 28-7 30-8 32-8
                _G.DrawString($"材  料：{info.MaterialName}", _font, Brushes.White, offsetX, 0, _format);
                _G.DrawString($"重  量：{info.Weight}", _font, Brushes.White, offsetX, 16, _format);
                _G.DrawString($"时  间：{info.WeighingTime.ToString("yy-MM-dd HH:mm")}", _font, Brushes.White, offsetX, 32, _format);
                _G.DrawString($"料  仓：{info.StorageName}", _font, Brushes.White, offsetX, 48, _format);
                _G.DrawString($"供应商：{info.Supplier}", _font, Brushes.White, offsetX, 64, _format);
                _G.DrawString($"提  示：{info.LedNote}", _font, Brushes.White, offsetX, 80, _format);
            }
            _G.Save();
            return displayImg;
        }
    }
}
