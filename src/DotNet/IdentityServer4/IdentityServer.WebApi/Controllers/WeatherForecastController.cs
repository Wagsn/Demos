using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    //[Authorize(policy: "")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            var user = User;
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [Route("summaries")]
        [HttpGet]
        public IActionResult GetSummaries()
        {
            var rng = new Random();
            var user = User;
            var id = user.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // 根据UserID查询用户订阅的摘要提醒
            if (id != null)
            {
                return Ok(new List<string>
                {
                    Summaries[rng.Next(Summaries.Length)],
                    Summaries[rng.Next(Summaries.Length)],
                    Summaries[rng.Next(Summaries.Length)],
                });
            }
            else
            {
                return Unauthorized();
            }
        }

    }
}
