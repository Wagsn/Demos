using System;

// 1
var url = "http://localhost:5000";
var client = new System.Net.Http.HttpClient();
var list = await new WeatherSdk.WeatherApiClient(url, client).WeatherForecastAsync();
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(list));

// 2
var apiClient = new WeatherSdk.WeatherApiClient();
var data = await apiClient.WeatherForecastAsync();
Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(data));

namespace WeatherSdk
{
    public partial class WeatherApiClient
    {
        public WeatherApiClient() : this("http://localhost:5000", new System.Net.Http.HttpClient()) { }
    }
}
