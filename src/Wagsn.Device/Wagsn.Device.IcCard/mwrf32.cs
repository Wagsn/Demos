using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.IcCard
{
    /// <summary>
    /// mwrf32.dll 接口封装
    /// 明华非接触式IC卡读写器无驱USB接口函数库
    /// </summary>
    public class Mwrf32
    {
        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="port">串口号</param>
        /// <param name="baud">波特率9600～115200</param>
        /// <returns>通讯设备标识符</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_init", SetLastError = true,
                CharSet = CharSet.Auto, ExactSpelling = false,
                CallingConvention = CallingConvention.StdCall)]
        public static extern int rf_init(Int16 port, int baud);

        /// <summary>
        /// 初始化USB
        /// </summary>
        /// <returns>通讯设备标识符</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_usbinit", SetLastError = true,
               CharSet = CharSet.Auto, ExactSpelling = false,
               CallingConvention = CallingConvention.StdCall)]
        public static extern int rf_usbinit();

        /// <summary>
        /// 断开与读卡器的连接
        /// </summary>
        /// <param name="icdev"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_exit", SetLastError = true,
               CharSet = CharSet.Auto, ExactSpelling = false,
               CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_exit(int icdev);

        /// <summary>
        /// 蜂鸣
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="msec"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_beep", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_beep(int icdev, int msec);

        /// <summary>
        /// 取得读写器硬件版本号
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="state"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_get_status", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_get_status(int icdev, [MarshalAs(UnmanagedType.LPArray)] byte[] state);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="lenth"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_srd_snr", SetLastError = true,
      CharSet = CharSet.Auto, ExactSpelling = false,
      CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_srd_snr(int icdev, int lenth, [MarshalAs(UnmanagedType.LPArray)] byte[] state);

        /// <summary>
        /// 将密码装入读写模块RAM块中
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="mode"></param>
        /// <param name="secnr"></param>
        /// <param name="keybuff"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_load_key", SetLastError = true,
               CharSet = CharSet.Auto, ExactSpelling = false,
               CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_load_key(int icdev, int mode, int secnr, [MarshalAs(UnmanagedType.LPArray)] byte[] keybuff);

        /// <summary>
        /// 向读写器装入十六进制密码
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="mode"></param>
        /// <param name="secnr"></param>
        /// <param name="keybuff"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_load_key_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_load_key_hex(int icdev, int mode, int secnr, [MarshalAs(UnmanagedType.LPArray)] byte[] keybuff);


        [DllImport("mwrf32.dll", EntryPoint = "a_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 a_hex([MarshalAs(UnmanagedType.LPArray)] byte[] asc, [MarshalAs(UnmanagedType.LPArray)] byte[] hex, int len);

        [DllImport("mwrf32.dll", EntryPoint = "hex_a", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 hex_a([MarshalAs(UnmanagedType.LPArray)] byte[] hex, [MarshalAs(UnmanagedType.LPArray)] byte[] asc, int len);

        /// <summary>
        /// 射频读写模式复位
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="msec"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_reset", SetLastError = true, CharSet = CharSet.Auto,
            ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_reset(int icdev, int msec);

        /// <summary>
        /// 寻卡
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="mode">寻卡模式</param>
        /// <param name="tagtype">卡类型值：0x0004为M1卡，0x0010为ML卡</param>
        /// <returns>成功则返回 0</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_request", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_request(int icdev, int mode, out UInt16 tagtype);

        /// <summary>
        /// 防止卡冲突，返回卡的序列号
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="bcnt">预选卡所用的位数，标准值为0（不考虑序列号）</param>
        /// <param name="snr">返回的卡序列号地址</param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_anticoll", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_anticoll(int icdev, int bcnt, out uint snr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_select", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_select(int icdev, uint snr, out byte size);

        /// <summary>
        /// 返回卡的序列号---- 寻卡
        /// 相当于连续调用 rf_request rf_anticoll rf_select
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="mode"></param>
        /// <param name="snr"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_card", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_card(int icdev, int mode, [MarshalAs(UnmanagedType.LPArray)] byte[] snr);

        /// <summary>
        /// 初始化块值---在进行值操作时，必须先执行初始化值函数，然后才可以读、减、加的操作
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="mode"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_initval", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        public static extern Int16 rf_initval(int icdev, int mode, byte date);

        /// <summary>
        /// 读块值
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="adr"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_initval", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        public static extern Int16 rf_initval(Int16 icdev, Int16 adr, ulong date);

        /// <summary>
        /// 验证卡某一扇区密码
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="mode"></param>
        /// <param name="secnr"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_authentication", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_authentication(int icdev, int mode, int secnr);

        [DllImport("mwrf32.dll", EntryPoint = "rf_authentication_2", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_authentication_2(int icdev, int mode, int keynr, int blocknr);

        /// <summary>
        /// 检测指定数据是否与卡中数据一致
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="snr"></param>
        /// <param name="authmode"></param>
        /// <param name="adr"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_check_write", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]

        public static extern Int16 rf_check_write(Int16 icdev, int snr, int authmode, int adr, byte date);

        /// <summary>
        /// 读取卡中数据
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="blocknr"></param>
        /// <param name="databuff"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_read", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_read(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)] byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_read_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_read_hex(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)] byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_write_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_write_hex(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)] byte[] databuff);

        /// <summary>
        /// 向卡中写数据
        /// </summary>
        /// <param name="icdev"></param>
        /// <param name="blocknr"></param>
        /// <param name="databuff"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_write", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_write(int icdev, int blocknr, [MarshalAs(UnmanagedType.LPArray)] byte[] databuff);

        /// <summary>
        /// 终止该卡操作 ---挂起
        /// </summary>
        /// <param name="icdev"></param>
        /// <returns></returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_halt", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_halt(int icdev);

        [DllImport("mwrf32.dll", EntryPoint = "rf_changeb3", SetLastError = true,
            CharSet = CharSet.Auto, ExactSpelling = false,
            CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_changeb3(int icdev, int sector, [MarshalAs(UnmanagedType.LPArray)] byte[] keya, int B0, int B1,
              int B2, int B3, int Bk, [MarshalAs(UnmanagedType.LPArray)] byte[] keyb);

        [DllImport("mwrf32.dll", EntryPoint = "rf_clr_control_bit", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_clr_control_bit(int icdev, int _b);


        [DllImport("mwrf32.dll", EntryPoint = "rf_set_control_bit", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_set_control_bit(int icdev, int _b);

        [DllImport("mwrf32.dll", EntryPoint = "rf_disp8", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_disp8(int icdev, short mode, [MarshalAs(UnmanagedType.LPArray)] byte[] disp);

        [DllImport("mwrf32.dll", EntryPoint = "rf_disp", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_disp(int icdev, short mode, int digit);

        [DllImport("mwrf32.dll", EntryPoint = "rf_encrypt", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_encrypt([MarshalAs(UnmanagedType.LPArray)] byte[] key, [MarshalAs(UnmanagedType.LPArray)] byte[] ptrsource, int len, [MarshalAs(UnmanagedType.LPArray)] byte[] ptrdest);

        [DllImport("mwrf32.dll", EntryPoint = "rf_decrypt", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_decrypt([MarshalAs(UnmanagedType.LPArray)] byte[] key, [MarshalAs(UnmanagedType.LPArray)] byte[] ptrsource, int len, [MarshalAs(UnmanagedType.LPArray)] byte[] ptrdest);

        [DllImport("mwrf32.dll", EntryPoint = "rf_srd_eeprom", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_srd_eeprom(int icdev, int offset, int len, [MarshalAs(UnmanagedType.LPArray)] byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_swr_eeprom", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_swr_eeprom(int icdev, int offset, int len, [MarshalAs(UnmanagedType.LPArray)] byte[] databuff);

        [DllImport("mwrf32.dll", EntryPoint = "rf_setport", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_setport(int icdev, byte _byte);

        [DllImport("mwrf32.dll", EntryPoint = "rf_getport", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_getport(int icdev, out byte _byte);

        [DllImport("mwrf32.dll", EntryPoint = "rf_gettime", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_gettime(int icdev, [MarshalAs(UnmanagedType.LPArray)] byte[] time);


        [DllImport("mwrf32.dll", EntryPoint = "rf_gettime_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_gettime_hex(int icdev, [MarshalAs(UnmanagedType.LPArray)] byte[] time);

        [DllImport("mwrf32.dll", EntryPoint = "rf_settime_hex", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_settime_hex(int icdev, [MarshalAs(UnmanagedType.LPArray)] byte[] time);

        [DllImport("mwrf32.dll", EntryPoint = "rf_settime", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_settime(int icdev, [MarshalAs(UnmanagedType.LPArray)] byte[] time);

        [DllImport("mwrf32.dll", EntryPoint = " rf_setbright", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_setbright(int icdev, byte bright);

        [DllImport("mwrf32.dll", EntryPoint = "rf_ctl_mode", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_ctl_mode(int icdev, int mode);

        [DllImport("mwrf32.dll", EntryPoint = "rf_disp_mode", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_disp_mode(int icdev, int mode);

        [DllImport("mwrf32.dll", EntryPoint = "lib_ver", SetLastError = true,
             CharSet = CharSet.Auto, ExactSpelling = false,
             CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 lib_ver([MarshalAs(UnmanagedType.LPArray)] byte[] ver);
    }
}
