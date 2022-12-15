using ElevenNote.Models.Token;
using ElevenNote.Models.User;
using ElevenNote.Services.Token;
using ElevenNote.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElevenNote.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;
        public UserController(IUserService userService, ITokenService tokenService)
        {
            _userService = userService;
            _tokenService = tokenService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> RegisterUser([FromForm] UserRegister newUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var registerResult = await _userService.RegisterUserAsync(newUser);

            if (registerResult)
            {
                return Ok("User successfully registered.");
            }

            return BadRequest("User NOT registered.");
        }

        [Authorize] 
        [HttpGet, Route("{userID}")]
        public async Task<IActionResult> GetByID(int userID)
        {
            var userDetail = await _userService.GetUserByIDAsync(userID);

            if (userID == 0)
            {
                return NotFound();
            }
            return Ok(userDetail);
        }

        [HttpPost, Route("~/api/Token")]
        public async Task<IActionResult> Token([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tokenResposne = await _tokenService.GetTokenAsync(request);

            if (tokenResposne is null)
            {
                return BadRequest("Invalid username or password");
            }

            return Ok(tokenResposne);
        }
    }
}