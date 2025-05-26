// AuthController.cs (Refactored)
using Microsoft.AspNetCore.Mvc;
using JobPortal.Api.DTOs;
using JobPortal.Api.Services.Interfaces;

namespace JobPortal.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto dto)
        {
            var (success, token, error) = await _authService.RegisterAsync(dto);
            if (!success)
                return BadRequest(error);

            return Ok(new { token });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var (success, token, error) = await _authService.LoginAsync(dto);
            if (!success)
                return Unauthorized(error);

            return Ok(new { token });
        }
    }
}
