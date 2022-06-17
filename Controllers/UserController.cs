using EasyNetQ;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Entities;
using MyAPI.HAL;
using MyAPI.Models;
using MyAPI.MyData;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMyStorage _db;

        public UserController(IMyStorage db)
        {
            this._db = db;
        }

        const int PAGE_SIZE = 25;

        // GET: api/users
        [HttpGet]
        [Produces("application/hal+json")]
        public IActionResult Get(int index = 0, int count = PAGE_SIZE)
        {
            var items = _db.ListUsers().Skip(index).Take(count)
                .Select(u => u.ToResource());
            var total = _db.CountUsers();
            var _links = HAL.HAL.PaginateAsDynamic("/api/users", index, count, total);
            /*var _links = HAL.PaginateAsDictionary("/api/users", index, count, total);*/
            var result = new
            {
                _links,
                count,
                total,
                index,
                items
            };
            return Ok(result);
        }

        // GET api/users/ABC
        [HttpGet("{id}")]
        [Produces("application/hal+json")]
        public IActionResult Get(string id)
        {
            var user = _db.FindUser(id);
            if (user == default) return NotFound();
            var resource = user.ToResource();
            resource._actions = new
            {
                delete = new
                {
                    href = $"/api/users/{id}",
                    method = "DELETE",
                    name = $"Delete {id} from the database"
                }
            };
            return Ok(resource);
        }

        // PUT api/users/ABC
        [HttpPut("{id}")]
        public IActionResult Put(string id, [FromBody] UserDto dto)
        {
            var userSpec = _db.FindSpecl(dto.SpecClass);
            var user = new User
            {
                Name = dto.Name,
                SpecClass = dto.SpecClass,
                Range = dto.Range,
                HomeRace = dto.HomeRace,
            };
            _db.UpdateUser(user);
            return Ok(dto);
        }

        // POST api/users
        [HttpPost("{id}")]
        public async Task<IActionResult> Post(string id, [FromBody] UserDto dto)
        {
            var existing = _db.FindUser(dto.Name);
            if (existing != default)
                return Conflict($"Sorry, there is already a vehicle with registration {dto.Name} in the database.");
            var classSpec = _db.FindSpecl(dto.SpecClass);
            var user = new User
            {
                Name = dto.Name,
                SpecClass = dto.SpecClass,
                Range = dto.Range,
                HomeRace = dto.HomeRace,
            };
            _db.CreateUser(user);
            return Created($"/api/user/{user.Name}", user.ToResource());
        }

        // DELETE api/users/ABC123
        [HttpDelete("{id}")]
        public IActionResult Delete(string id)
        {
            var user = _db.FindUser(id);
            if (user == default) return NotFound();
            _db.DeleteUser(user);
            return NoContent();
        }

    }
}
