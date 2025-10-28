using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
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
            await _authService.RegisterAsync(request);
            return Ok(ApiResponse<string>.Ok(null, "User registered successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var tokenResponse = await _authService.LoginAsync(request);
            return Ok(ApiResponse<JwtTokenResponse>.Ok(tokenResponse));
        }

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] OtpRequestDto dto)
        {
            await _otpService.GenerateAndSendOtpAsync(dto.Email, dto.Purpose);
            return Ok(ApiResponse<string>.Ok(null, "OTP sent successfully."));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto dto)
        {
            bool isValid = await _otpService.VerifyOtpAsync(dto.UserId, dto.OtpCode, dto.Purpose);
            if (!isValid)
                return BadRequest(ApiResponse<string>.Fail("Invalid or expired OTP."));

            return Ok(ApiResponse<string>.Ok(null, "OTP verified successfully."));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            var tokens = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return Ok(ApiResponse<object>.Ok(new { token = tokens.Token, refreshToken = tokens.RefreshToken }));
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
            return Ok(ApiResponse<string>.Ok(null, "Password reset OTP sent."));
        }

        [HttpPost("forgot-password/reset")]
        public async Task<IActionResult> ResetPasswordWithOtp([FromBody] ResetPasswordDto dto)
        {
            await _otpService.ResetPasswordAsync(dto.UserId, dto.OtpCode, dto.NewPassword, dto.ConfirmPassword);
            return Ok(ApiResponse<string>.Ok(null, "Password has been reset successfully."));
        }

    }
}
