using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wagsn.Device.PowderLevel
{
    /// <summary>
    /// 粉仓料位仪 数据接收器
    /// </summary>
    public class BoshuoPowderLevelReceiver: DeviceUdpReceiver<LwyStructReverse?>
    {
        public BoshuoPowderLevelReceiver(string port) : base(port) { }

        public override LwyStructReverse? ReceiveConvert(object frame)
        {
            try
            {
                byte[] bytRecv = (byte[])frame;
                Array.Reverse(bytRecv, 0, bytRecv.Length);

                int size = Marshal.SizeOf(typeof(LwyStructReverse));
                if (bytRecv.Length != size)
                {
                    Console.WriteLine($"数据帧长度错误（{bytRecv.Length}）");
                    return default;
                }
                if (bytRecv[0] != 0xAA || bytRecv[size - 1] != 0xAA)
                {
                    Console.WriteLine($"数据帧开始结束符错误");
                    return default;
                }

                IntPtr structPtr = Marshal.AllocHGlobal(size);
                Marshal.Copy(bytRecv, 0, structPtr, size);
                var obj = Marshal.PtrToStructure(structPtr, typeof(LwyStructReverse));
                Marshal.FreeHGlobal(structPtr);

                return (LwyStructReverse?)obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine("转换失败：" + ex);
                return default;
            }
        }
    }

    /// <summary>
    /// UDP 报文
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LwyStructReverse
    {
        // byte/char 1字节  ushort 2字节 uint 4字节  ulong 8字节    byte[] 任意字节

        /// <summary>
        /// 包尾
        /// </summary>
        public byte packageTail;

        /// <summary>
        /// crc16
        /// </summary>
        public ushort crc16;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public StorageDataReverse[] storageData;

        /// <summary>
        /// 仓数据长度
        /// </summary>
        public byte storageDataLength;

        /// <summary>
        /// 仓数量
        /// </summary>
        public byte storageNumber;

        /// <summary>
        /// IP末尾
        /// </summary>
        public byte ipEnd;

        /// <summary>
        /// 生产线编号
        /// </summary>
        public byte productionLineNo;

        /// <summary>
        /// 版本号
        /// </summary>
        public byte protocolVersion;

        /// <summary>
        /// 软件版本号
        /// </summary>
        public byte softwareVersion;

        /// <summary>
        /// 包长度 (和供应商确定一下包长度是否固定)
        /// </summary>
        public ushort packageLength;

        /// <summary>
        /// 包头
        /// </summary>
        public byte packageHead;
    }

    /// <summary>
    /// 货仓数据
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct StorageDataReverse
    {
        /// <summary>
        /// 仓 1 预留
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
        public byte[] reserve;

        /// <summary>
        /// 仓顶压力值
        /// </summary>
        public ushort pressureValue;

        /// <summary>
        /// M1 卡序号
        /// </summary>
        public byte m1SerialNumber;

        /// <summary>
        ///  料位传感器
        /// </summary>
        public byte levelSensor;

        /// <summary>
        /// 物料温度
        /// </summary>
        public short levelTemperature;

        /// <summary>
        /// 料位重量
        /// </summary>
        public short levelWeight;

        /// <summary>
        /// 状态 2
        /// </summary>
        public byte stateTwo;

        /// <summary>
        /// 状态 1 
        /// </summary>
        public byte stateOne;
    }
}
