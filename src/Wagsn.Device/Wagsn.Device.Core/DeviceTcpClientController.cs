using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace Wagsn.Device.Core
{
    /// <summary>
    /// 设备控制
    /// </summary>
    public abstract class DeviceTcpClientController<TRequest, TReceiveData> : IDeviceController<TRequest, TReceiveData>
    {
        public event DataReceivedHandler<TReceiveData> DataReceived;

        private TcpClient client;

        public DeviceTcpClientController()
        {
            // Create a TcpClient.
            // Note, for this client to work you need to have a TcpServer
            // connected to the same address as specified by the server, port
            // combination.
            var server = "127.0.0.1";
            int port = 13000;
            client = new TcpClient(server, port);
        }

        public TReceiveData ReceiveConvert(object frame)
        {
            throw new NotImplementedException("暂不作为接收器");
        }

        public abstract byte[] SendDataConvert(TRequest data);

        public void Send(TRequest data)
        {
            try
            {
                // Translate the passed message into ASCII and store it as a Byte array.
                byte[] buffer = SendDataConvert(data);

                // Get a client stream for reading and writing.
                //  Stream stream = client.GetStream();
                NetworkStream stream = client.GetStream();

                // Send the message to the connected TcpServer.
                stream.Write(buffer, 0, buffer.Length);

                Console.WriteLine("Sent: {0}", data);

                // Receive the TcpServer.response.

                // Buffer to store the response bytes.
                buffer = new byte[256];

                // String to store the response ASCII representation.
                string responseData = string.Empty;

                // Read the first batch of the TcpServer response bytes.
                int bytes = stream.Read(buffer, 0, buffer.Length);
                responseData = System.Text.Encoding.ASCII.GetString(buffer, 0, bytes);
                Console.WriteLine("Received: {0}", responseData);

                // Close everything.
                stream.Close();
                client.Close();
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException: {0}", e);
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }

            Console.WriteLine("\n Press Enter to continue...");
            Console.Read();
        }
    }
}
