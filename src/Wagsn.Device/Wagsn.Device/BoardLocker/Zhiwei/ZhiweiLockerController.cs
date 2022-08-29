using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device.BoardLocker
{
    /// <summary>
    /// 志伟板控锁控制器
    /// </summary>
    public class ZhiweiLockerController: IBoardLockerController
    {
        public int ControlBoardSwitchCount { get; } = 30;

        public void Close(int lockerIndex)
        {
            throw new NotSupportedException("小票柜不支持关锁操作");
        }

        public void Open(int lockerIndex)
        {
            string sendCmd = string.Empty;
            try
            {
                int boardIndex = 1;
                if (lockerIndex > ControlBoardSwitchCount)
                {
                    boardIndex = 2;
                    lockerIndex -= ControlBoardSwitchCount;
                }
                byte head = 0x8a;
                byte board = (byte)boardIndex;
                byte lock_ = (byte)lockerIndex;
                byte command = 0x11;
                byte checkbyte = (byte)(head ^ board ^ lock_ ^ command);
                byte[] byteOp = { head, board, lock_, command, checkbyte };

                sendCmd = ToHexStrFromByte(byteOp);

                if (boxConnector == null || !boxConnector.IsOpen)
                {
                    InitConnect();
                }

                boxConnector.Write(byteOp, 0, byteOp.Length);
            }
            catch (Exception ex)
            {
                throw new Exception("开锁失败，开锁命令为：" + sendCmd, ex);
            }
        }

        private SerialPort boxConnector { get; set; } = null;
        private string PortName { get; set; }

        // 静态构造,在有成员被调用之前会被先调用
        public ZhiweiLockerController(int switchCount, string portName)
        {
            ControlBoardSwitchCount = switchCount;
            PortName = portName;

            InitConnect();
        }

        /// <summary>
        /// 初始化连接
        /// </summary>
        private void InitConnect()
        {
            if (!string.IsNullOrEmpty(PortName))
            {
                boxConnector = new SerialPort();
                boxConnector.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                boxConnector.PortName = PortName ?? "COM1";  // 端口名称，默认COM1
                boxConnector.BaudRate = 9600;  // 波特率，默认9600
                boxConnector.DataBits = 8;  // 数据位，默认8
                boxConnector.Parity = Parity.None;  // 奇偶校验，默认 System.IO.Ports.Parity.None
                boxConnector.StopBits = StopBits.One;  // 停止位，默认 System.IO.Ports.StopBits.One
                boxConnector.Handshake = Handshake.None;  // 握手协议，默认 System.IO.Ports.Handshake.None
                boxConnector.RtsEnable = true;  // 是否启用请求发送RTS信号，默认 false
                boxConnector.Open();
                Console.WriteLine("连接成功");
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (boxConnector == null || !boxConnector.IsOpen) return;
                int len = boxConnector.BytesToRead;
                if (len != 0)
                {
                    byte[] buff = new byte[len];
                    boxConnector.Read(buff, 0, len);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("接收反馈失败" + ex);
            }
        }

        /// <summary>
        /// 字节数组转16进制字符串：空格分隔
        /// </summary>
        /// <param name="byteDatas"></param>
        /// <returns></returns>
        private string ToHexStrFromByte(byte[] byteDatas)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteDatas.Length; i++)
            {
                builder.Append(string.Format("{0:X2} ", byteDatas[i]));
            }
            return builder.ToString().Trim();
        }

        public void Command(int lockerIndex, int command)
        {
            throw new NotImplementedException();
        }
    }
}
