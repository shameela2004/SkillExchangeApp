using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.DTOs.Auth;
using MyApp1.Application.DTOs.OtpDtos;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Services;

namespace MyApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;

        public AuthController(IAuthService authService, IOtpService otpService)
        {
            _authService = authService;
            _otpService = otpService;
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
        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] OtpRequestDto dto)
        {
            await _otpService.GenerateAndSendOtpAsync(dto.Email, dto.Purpose);
            return Ok();
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto dto)
        {
            var isValid = await _otpService.VerifyOtpAsync(dto.UserId, dto.OtpCode, dto.Purpose);
            if (!isValid) return BadRequest("Invalid or expired OTP.");

            return Ok("OTP verified successfully.");
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

        [HttpPost("forgot-password/request-otp")]
        public async Task<IActionResult> RequestForgotPasswordOtp([FromBody] OtpRequestDto dto)
        {
            await _otpService.GenerateAndSendOtpAsync(dto.Email, "ResetPassword");
            return Ok();
        }

        [HttpPost("forgot-password/reset")]
        public async Task<IActionResult> ResetPasswordWithOtp([FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _otpService.ResetPasswordAsync(dto.UserId, dto.OtpCode, dto.NewPassword, dto.ConfirmPassword);
                return Ok("Password has been reset successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
