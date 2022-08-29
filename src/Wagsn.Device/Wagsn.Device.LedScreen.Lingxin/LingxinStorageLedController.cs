using IntelligentHardware.Domain.Request;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading;

namespace Wagsn.Device.LedScreen.Lingxin
{
    public class LingxinStorageLedController : IStorageLedController
    {
        private readonly LedSettings _ledSettings;

        public LingxinStorageLedController(LedSettings ledSettings)
        {
            _ledSettings = ledSettings;
        }

        public void Display(StorageDisplayInfo info)
        {
            var dir = AppDomain.CurrentDomain.BaseDirectory + $"temp\\image";
            var path = AppDomain.CurrentDomain.BaseDirectory + $"temp\\image\\{DateTime.Now:yyyyMMddHHmmssfff}.png";
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                using (Bitmap displayImg = Render(info))
                {
                    displayImg.Save(path);
                }
                SendToDevice(_ledSettings, path);
            }
            catch (Exception ex)
            {
                throw new Exception($"发送到灵信LED屏失败（配置：{Newtonsoft.Json.JsonConvert.SerializeObject(_ledSettings)}，内容：{Newtonsoft.Json.JsonConvert.SerializeObject(info)}）", ex);
            }
            finally
            {
                Thread thread = new Thread(a => DeleteImage(path));
                thread.Start();
            }
        }

        /// <summary>
        /// 向灵信LED屏发送图片内容 (北京厂站屏)
        /// </summary>
        /// <param name="ledIP"></param>
        /// <param name="path"></param>
        private void SendToDevice(LedSettings settings, string path)
        {
            var screemWidth = settings.Width;
            var screemHeight = settings.Height;
            int colorType = settings.ColorType;
            var ledIP = settings.RemoteIp;

            LedAPILingXin.COMMUNICATIONINFO communicationInfo = new LedAPILingXin.COMMUNICATIONINFO();//定义一通讯参数结构体变量用于对设定的LED通讯，具体对此结构体元素赋值说明见COMMUNICATIONINFO结构体定义部份注示
            communicationInfo.LEDType = 0;
            communicationInfo.SendType = 0;//设为固定IP通讯模式，即TCP通讯
            communicationInfo.IpStr = ledIP;//给IpStr赋值LED控制卡的IP
            communicationInfo.LedNumber = 1;//LED屏号为1，注意socket通讯和232通讯不识别屏号，默认赋1就行了，485必需根据屏的实际屏号进行赋值
            IntPtr hProgram = LedAPILingXin.LV_CreateProgramEx(screemWidth, screemHeight, colorType, 0, 3);//1.创建节目对象
            int nResult = LedAPILingXin.LV_AddProgram(hProgram, 0, 0, 1);//2.添加一个节目
            if (nResult != 0)
            {
                string errStr = LedAPILingXin.LS_GetError(nResult);
                throw new Exception("创建节目失败(" + nResult + "): " + errStr);
            }

            LedAPILingXin.AREARECT areaRect = new LedAPILingXin.AREARECT();//区域坐标属性结构体变量
            areaRect.left = 0;
            areaRect.top = 0;
            areaRect.width = screemWidth;
            areaRect.height = screemHeight;

            LedAPILingXin.LV_AddImageTextArea(hProgram, 0, 1, ref areaRect, 1);//3.添加一个图文区域

            LedAPILingXin.PLAYPROP playProp = new LedAPILingXin.PLAYPROP();
            playProp.InStyle = 0;
            playProp.DelayTime = 3;
            playProp.Speed = 4;

            nResult = LedAPILingXin.LV_AddFileToImageTextArea(hProgram, 0, 1, path, ref playProp);//4.添加一个文件到图文区
            nResult = LedAPILingXin.LV_Send(ref communicationInfo, hProgram);//5.发送
            if (nResult != 0)//如果失败则可以调用LV_GetError获取中文错误信息
            {
                string errStr = LedAPILingXin.LS_GetError(nResult);
                throw new Exception("发送图片失败(" + nResult + "): " + errStr);
            }
        }

        /// <summary>
        /// 删除生成的临时图片
        /// </summary>
        /// <param name="path"></param>
        private void DeleteImage(string path)
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

        public Bitmap Render(StorageDisplayInfo info)
        {
            return new LingxinStorageLedRender(_ledSettings).RenderImage(info);
        }
    }
}
