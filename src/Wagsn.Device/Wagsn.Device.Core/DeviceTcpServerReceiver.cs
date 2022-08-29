using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wagsn.Device
{
    /// <summary>
    /// 设备TCP模式接收器
    /// </summary>
    public abstract class DeviceTcpServerReceiver<TReceiveData> : IDeviceDataReceiver<TReceiveData>
    {
        public event DataReceivedHandler<TReceiveData> DataReceived;
        public abstract TReceiveData ReceiveConvert(object frame);

        TcpListener listener;
        Thread thread;
        int bufferLength = 10240;

        public DeviceTcpServerReceiver()
        {
            IPEndPoint localEp = new IPEndPoint(IPAddress.Any, 86);
            listener = new TcpListener(localEp);
            listener.Start();


            thread = new Thread(x =>
            {
                byte[] buffer = new byte[bufferLength];
                while (true)
                {
                    // Step 0: Client connection     
                    if (!listener.Pending())
                    {
                        Thread.Sleep(500); // choose a number (in milliseconds) that makes sense
                        continue; // skip to next iteration of loop
                    }
                    else // Enter here only if have pending clients
                    {

                        TcpClient client = listener.AcceptTcpClient();
                        NetworkStream strem = client.GetStream();

                        // TODO Loop to receive all the data sent by the client.
                        int count = strem.Read(buffer, 0, bufferLength);

                        byte[] bytes = new byte[count];
                        Buffer.BlockCopy(bytes, 0, bytes, 0, count);

                        TReceiveData data = ReceiveConvert(bytes);
                        if (data != null && DataReceived != null)
                        {
                            DataReceived(data);
                        }
                        strem.Dispose();
                        client.Close();
                    }
                }
            });
            thread.IsBackground = true;
            thread.Start();
        }
    }
}
