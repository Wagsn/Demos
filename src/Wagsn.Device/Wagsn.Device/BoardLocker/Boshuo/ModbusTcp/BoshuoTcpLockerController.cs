using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Wagsn.Device.BoardLocker.Boshuo
{
    /// <summary>
    /// 博硕ModbusTcp协议粉仓锁控制
    /// </summary>
    public class BoshuoTcpLockerController : IBoardLockerController
    {
        public int ControlBoardSwitchCount { get; } = 16;

        private readonly TcpClient tcpClient;

        private readonly string _ip;
        private readonly int _port;

        public BoshuoTcpLockerController(string ip = "192.168.7.58", int port = 8605)
        {
            _ip = ip;
            _port = port;
        }

        public void Close(int lockerIndex)
        {
            Command(lockerIndex, 0);
        }

        public void Open(int lockerIndex)
        {
            Command(lockerIndex, 1);
        }

        public void Command(int lockerIndex, int command)
        {
            // 开01仓 00000000000910101260000102005A
            // 开08仓 00000000000910101267000102005A
            // 开16仓 0000000000091010126F000102005A
            // 模拟响应: 00 01 00 00 00 09 10 10 12 60 00 01 02 00 01
            var buffer = new byte[] {
                /**标识符任意值**/0x00, 0x00, 0x00, 0x00,
                /**长度**/0x00, 0x09,
                /**设备地址 功能码**/0x10, 0x10,
                /**寄存器起始地址 1260~126F **/0x12, 0x60,
                /**寄存器数量、按实际需要写入的仓数量**/0x00, 0x01,
                /**字节数、仓数量*2**/0x02,
                /**电子门禁指令定义 0x00A5：开锁 0x005A：关锁 0x0055：禁止开锁**/0x00, 0xA5,
            };
            if (lockerIndex <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(lockerIndex), "参数值超出范围");
            }
            if (command != 0 && command != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(command), "参数值超出范围");
            }
            var value = (ushort)(command == 1 ? 0x00A5 : 0x005A);

            using (TcpClient tcpClient = new TcpClient(_ip, _port))
            {
                var modbusFactory = new NModbus.ModbusFactory();
                using (var master = modbusFactory.CreateMaster(tcpClient))
                {
                    master.Transport.WriteTimeout = 5000;
                    master.Transport.Retries = 3;
                    master.WriteMultipleRegisters(0x10, (ushort)(0x125F + lockerIndex), new ushort[] { value });
                    tcpClient.Client.Shutdown(SocketShutdown.Send);
                }
                tcpClient.Close();
            }
        }
    }
}
