using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiServer.AspNetCore.Mvc.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CheckController : Controller
    {
        [HttpGet]
        [HttpHead]
        [HttpOptions]
        public IActionResult Check()
        {
            return Ok();
        }
    }
}
