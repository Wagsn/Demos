using System;

namespace Net5Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://localhost:5000";
            var client = new System.Net.Http.HttpClient();
            var list = new Weather.WeatherApiClient(url, client).WeatherForecastAsync().Result;

            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(list));
        }
    }
}
