using Microsoft.AspNetCore.Mvc;
using BLL.Interfaces.Services;
using DAL.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace OrderManagmentSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST /api/users/register
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _authService.RegisterAsync(registerDto);
            if (user == null)
                return BadRequest("Registration failed. Username or email may already exist.");

            return Ok(user);
        }

        // POST /api/users/login
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var loginResult = await _authService.LoginAsync(loginDto);
            if (loginResult == null)
                return Unauthorized("Invalid username or password.");

            return Ok(loginResult);
        }
    }
} 