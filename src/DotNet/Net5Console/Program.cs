using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Net5Console
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1
            var url = "http://localhost:5000";
            var client = new System.Net.Http.HttpClient();
            var list = new WeatherSdk.WeatherApiClient(url, client).WeatherForecastAsync().Result;
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(list));

            // 2
            var apiClient = new WeatherSdk.WeatherApiClient();
            var data = apiClient.WeatherForecastAsync().Result;
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(data));
        }
    }

}

namespace WeatherSdk
{
    public partial class WeatherApiClient
    {
        public WeatherApiClient() : this("http://localhost:5000", new System.Net.Http.HttpClient()) { }
    }
}
