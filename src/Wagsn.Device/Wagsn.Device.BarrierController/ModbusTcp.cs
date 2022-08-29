using Modbus.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using TCZNGB.Core.Domain.Response;

namespace TCZNGB.Service.ZNGB_ControlRoadgate
{
    public class ModbusTcp
    {
        TcpClient tcpClient;
        ModbusIpMaster master;

        /// <summary> 
        /// 监听的IP地址
        /// </summary>
        public string Address { get; private set; }
        /// <summary>
        /// 监听的端口
        /// </summary>
        public int Port { get; private set; }

        [DllImport("wininet.dll", CharSet = CharSet.Auto)]
        static extern bool InternetGetConnectedState(ref InternetConnectionState lpdwFlags, int dwReserved);
        enum InternetConnectionState : int
        {
            INTERNET_CONNECTION_MODEM = 0x1,
            INTERNET_CONNECTION_LAN = 0x2,
            INTERNET_CONNECTION_PROXY = 0x4,
            INTERNET_RAS_INSTALLED = 0x10,
            INTERNET_CONNECTION_OFFLINE = 0x20,
            INTERNET_CONNECTION_CONFIGURED = 0x40
        }

        public ModbusTcp()
        {
        }


        public ModbusTcp(string localIPAddress, int listenPort) 
        {
            this.Address = localIPAddress;
            this.Port = listenPort;         
        }

        public bool Start()
        {
            if (master != null)
                master.Dispose();
            if (tcpClient != null)
                tcpClient.Close();
            if (CheckInternet())
            {
                try
                {
                    tcpClient = new TcpClient();
                    IAsyncResult asyncResult = tcpClient.BeginConnect(Address, Port, null, null);
                    asyncResult.AsyncWaitHandle.WaitOne(3000, true); //wait for 3 sec
                    if (!asyncResult.IsCompleted)
                    {
                        tcpClient.Close();
                       // Console.WriteLine(DateTime.Now.ToString() + ":Cannot connect to server.");

                        //textBox_receive.Text += DateTime.Now.ToString() + commu.GetWrongInformation(Communication.ConnectWrong);
                        return false;

                    }
                    // create Modbus TCP Master by the tcpclient
                    master = ModbusIpMaster.CreateIp(tcpClient);
                    master.Transport.Retries = 0; //don't have to do retries
                    master.Transport.ReadTimeout = 1500;
                    //Console.WriteLine(DateTime.Now.ToString() + ":Connect to server.");

                    //textBox_receive.Text += DateTime.Now.ToString() + commu.GetWrongInformation(Communication.ConnectSuccess);
                    return true;
                }
                catch (Exception ex)
                {
                    //Console.WriteLine(DateTime.Now.ToString() + ":Connect process " + ex.StackTrace + "==>" + ex.Message);
                    return false;
                }
            }
            return false;
        }

        public async void ExecuteFunction(ModelbusModel model)
        {
            try
            {
                switch (model.FuncTionCode)
                {
                    case 1:
                        await master.ReadCoilsAsync(model.SlaveId, model.StartAddress, model.NumInputs);
                        break;
                    case 2:
                        await master.ReadInputsAsync(model.SlaveId, model.StartAddress, model.NumInputs);
                        break;
                    case 3:
                        await master.ReadHoldingRegistersAsync(model.SlaveId, model.StartAddress, model.NumInputs);
                        break;
                    case 4:
                        await master.ReadInputRegistersAsync(model.SlaveId, model.StartAddress, model.NumInputs);
                        break;
                    case 5:
                        await master.WriteSingleCoilAsync(model.SlaveId, model.StartAddress, model.GetInputCoils()[0]);
                        break;
                    case 6:
                        await master.WriteSingleRegisterAsync(model.SlaveId, model.StartAddress, model.GetInputValue()[0]);
                        break;
                    case 7:
                        await master.WriteMultipleCoilsAsync(model.SlaveId, model.StartAddress, model.GetInputCoils());
                        break;
                    case 8:
                        await master.WriteMultipleRegistersAsync(model.SlaveId, model.StartAddress, model.GetInputValue());
                        break;
                    default: break;
                }
            }
            catch (Exception ex)
            {
            }
        }


        //检查网络
        private bool CheckInternet()
        {
            InternetConnectionState flag = InternetConnectionState.INTERNET_CONNECTION_LAN;
            return InternetGetConnectedState(ref flag, 0);
        }
    }   
}
