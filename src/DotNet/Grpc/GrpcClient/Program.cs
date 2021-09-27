using Grpc.Net.Client;
using GrpcGreeter;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.ReadKey();
            /*AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);*/
            //System.Net.Http.SocketsHttpHandler.IsSupported
            // The port number(5001) must match the port of the gRPC server.
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var reply = await client.SayHelloAsync(
                              new HelloRequest { Name = "GreeterClient" });
            Console.WriteLine("Greeting: " + reply.Message);

            /*
            // 子文件夹调用 https://github.com/grpc/grpc-dotnet/issues/880
            var handler = new SubdirectoryHandler(new HttpClientHandler(), "/TestSubdirectory");
            var httpClient = new HttpClient(handler);

            var channel = GrpcChannel.ForAddress("https://localhost:5001",
                new GrpcChannelOptions { HttpClient = httpClient });
            var client = new Greet.GreeterClient(channel);*/

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
            Console.WriteLine("Hello World!");
        }
    }
}
