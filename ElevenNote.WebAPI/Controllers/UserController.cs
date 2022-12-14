using ElevenNote.Models.User;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegister newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _service.RegisterUserAsync(newUser);

            if (registerResult)
            {
                return Ok("User successfully registered.");
            }

            return BadRequest("User NOT registered.");
        }

        [HttpGet, Route("{userID:int}")]
        public async Task<IActionResult> GetByID([FromForm] int userID)
        {
            var userDetail = await _service.GetUserByIDAsync(userID);

            if (userID == 0)
            {
                return NotFound();
            }
            return Ok(userDetail);
        }
    }
}