using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    /// <summary>
    /// COM口数据接收器
    /// </summary>
    public abstract class DeviceComReceiver<TReceiveData>: IDeviceDataReceiver<TReceiveData>
    {
        public event DataReceivedHandler<TReceiveData> DataReceived;
        public abstract TReceiveData ReceiveConvert(object frame);

        SerialPort serialPort;

        public DeviceComReceiver(ISerialPortSetting portSetting)
        {
            if (!string.IsNullOrEmpty(portSetting.PortName))
            {
                serialPort = new SerialPort();
                serialPort.DataReceived += DataReceivedHandler;
                serialPort.PortName = portSetting.PortName;                     // 端口名称，默认COM1
                serialPort.BaudRate = portSetting.BaudRate ?? 9600;             // 波特率，默认9600
                serialPort.DataBits = portSetting.DataBits ?? 8;                // 数据位，默认8
                serialPort.Parity = portSetting.Parity ?? Parity.None;          // 奇偶校验，默认 System.IO.Ports.Parity.None
                serialPort.StopBits = portSetting.StopBits ?? StopBits.One;     // 停止位，默认 System.IO.Ports.StopBits.One
                serialPort.Handshake = portSetting.Handshake ?? Handshake.None; // 握手协议，默认 System.IO.Ports.Handshake.None
                serialPort.RtsEnable = portSetting.RtsEnable ?? false;          // 是否启用请求发送RTS信号，默认 false
                serialPort.WriteTimeout = portSetting.WriteTimeout ?? 1000;     // 设置串口写入数据阻塞时长，阻塞到写入数据或超时(这里为1秒)
                serialPort.ReadTimeout = portSetting.ReadTimeout ?? 1000;       // 设置串口读取数据阻塞时长，阻塞到读取数据或超时(这里为1秒)
                serialPort.Open();
            }
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                if (serialPort == null || !serialPort.IsOpen) return;
                int len = serialPort.BytesToRead;
                if (len != 0)
                {
                    byte[] buff = new byte[len];
                    serialPort.Read(buff, 0, len);

                    TReceiveData data = ReceiveConvert(buff);

                    if (data != null && DataReceived != null)
                    {
                        DataReceived(data);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("接收处理数据失败：" + ex);
            }
        }
    }
}
