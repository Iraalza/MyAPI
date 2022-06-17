using Microsoft.AspNetCore.Mvc;
using MyAPI.Entities;
using MyAPI.HAL;
using MyAPI.MyData;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecController : Controller
    {
        private readonly IMyStorage _db;

        public SpecController(IMyStorage db)
        {
            this._db = db;
        }

        [HttpGet]
        public IEnumerable<Spec> Get()
        {
            return _db.ListSpecs();
        }

        [HttpGet("{id}")]
        public IActionResult Get(string id)
        {
            var userSpec = _db.FindSpecl(id);
            if (userSpec == default) return NotFound();
            var resource = userSpec.ToDynamic();
            /*resource._actions = new
            {
                create = new
                {
                    href = $"/api/specs/{id}",
                    type = "application/json",
                    method = "POST",
                    name = $"Create a new {userSpec.Manufacturer.Name} {vehicleModel.Name}"
                }
            };*/
            return Ok(resource);
        }
    }
}
