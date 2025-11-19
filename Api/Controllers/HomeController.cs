using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        // GET: api/
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Welcome to the Home API!");
        }
    }
}
