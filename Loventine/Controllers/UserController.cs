using Loventine.Models;
using Loventine.Services;
using Microsoft.AspNetCore.Mvc;

namespace Loventine.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MongoDBService _mongoDBService;

        public UserController(MongoDBService mongoDBService)
        {
            _mongoDBService = mongoDBService;
        }
        [HttpGet]
        public async Task<List<User>> Get()
        {
            return await _mongoDBService.GetUserAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(string id)
        {
            var user = await _mongoDBService.GetUserByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
        [HttpPost()]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            await _mongoDBService.CreateUserAsync(user);
            return CreatedAtAction(nameof(Get), new { id = user.Id }, user);
        }
        [HttpPost("loginWithEmail")]
        public async Task<IActionResult> LoginWithEmail([FromBody] LoginRequest loginRequest)
        {
            var loginSuccess = await _mongoDBService.LoginUserAsync(loginRequest.Email, loginRequest.Password);

            if (loginSuccess != null)
            {
                // Successful login, you can generate and return a token, set cookies, etc.
                return Ok(new { Message = "Login successful" });
            }
            return BadRequest(new { Message = "Invalid login credentials" });
        }
    }
}
