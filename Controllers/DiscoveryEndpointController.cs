using Microsoft.AspNetCore.Mvc;

namespace MyAPI.Controllers
{
    [Route("api")]
    [ApiController]
    public class DiscoveryEndpointController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            var welcome = new
            {
                _links = new
                {
                    users = new
                    {
                        href = "/api/users"
                    }
                },
                message = "Welcome to the My API!",
            };
            return Ok(welcome);
        }
    }
}