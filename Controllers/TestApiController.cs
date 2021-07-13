using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace voteCollector.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class TestApiController : ControllerBase
    {
        [HttpGet("test")]
        public IActionResult Testing()
        {            
           return Ok("Тест пройден");
        }
    }
}
