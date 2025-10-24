using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.DTOs.Auth;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _authService.RegisterAsync(request);
                return Ok(new { Message = "User registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var tokenResponse = await _authService.LoginAsync(request);
                return Ok(tokenResponse);
            }
            catch (Exception ex)
            {
                return Unauthorized(new { Error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            try
            {
                var tokens = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);
                return Ok(new { token = tokens.Token, refreshToken = tokens.RefreshToken });
            }
            catch
            {
                return Unauthorized();
            }
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> Revoke([FromBody] RevokeRequest request)
        {
            await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
            return NoContent();
        }
    }
}
