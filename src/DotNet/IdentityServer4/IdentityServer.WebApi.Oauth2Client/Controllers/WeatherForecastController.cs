using IdentityModel.Client;
using IdentityServer.WebApi.Oauth2Client.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.WebApi.Oauth2Client.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IIdentityServer4Service _identityServer4Service;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IIdentityServer4Service identityServer4Service)
        {
            _logger = logger;
            _identityServer4Service = identityServer4Service;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var OAuth2Token = await _identityServer4Service.GetToken("weatherApi.read");
            using (var client = new System.Net.Http.HttpClient())
            {
                client.SetBearerToken(OAuth2Token.AccessToken);
                var result = await client.GetAsync("https://localhost:44332/weatherforecast");
                if (result.IsSuccessStatusCode)
                {
                    var model = await result.Content.ReadAsStringAsync();
                    return System.Text.Json.JsonSerializer.Deserialize<List<WeatherForecast>>(model);
                }
                else
                {
                    throw new Exception("Some Error while fetching Data");
                }
            }
        }
    }
}
