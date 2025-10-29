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
            try
            {
                await _authService.RegisterAsync(request);
                return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status201Created, "User registered successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, ex.Message));
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var tokenResponse = await _authService.LoginAsync(request);
                return Ok(ApiResponse<JwtTokenResponse>.SuccessResponse(tokenResponse, StatusCodes.Status200OK));
            }
            catch (Exception ex)
            {
                return Unauthorized(ApiResponse<string>.FailResponse(StatusCodes.Status401Unauthorized, ex.Message));
            }
        }

        [HttpPost("request-otp")]
        public async Task<IActionResult> RequestOtp([FromBody] OtpRequestDto dto)
        {
            await _otpService.GenerateAndSendOtpAsync(dto.Email, dto.Purpose);
            return Ok(ApiResponse<string>.SuccessResponse(dto.Email,StatusCodes.Status200OK, "OTP sent successfully."));
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerifyDto dto)
        {
            bool isValid = await _otpService.VerifyOtpAsync(dto.UserId, dto.OtpCode, dto.Purpose);
            if (!isValid)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest,"Invalid or expired OTP."));

            return Ok(ApiResponse<string>.SuccessResponse("Success",StatusCodes.Status200OK, "OTP verified successfully."));
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        {
            var tokens = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);
            return Ok(ApiResponse<object>.SuccessResponse(new { token = tokens.Token, refreshToken = tokens.RefreshToken },StatusCodes.Status200OK,"Refresh Token Created Successfully"));
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
            return Ok(ApiResponse<string>.SuccessResponse(dto.Email, StatusCodes.Status200OK,  "Password reset OTP sent."));
        }

        [HttpPost("forgot-password/reset")]
        public async Task<IActionResult> ResetPasswordWithOtp([FromBody] ResetPasswordDto dto)
        {
            await _otpService.ResetPasswordAsync(dto.UserId, dto.OtpCode, dto.NewPassword, dto.ConfirmPassword);
            return Ok(ApiResponse<string>.SuccessResponse("success", StatusCodes.Status200OK, "Password has been reset successfully."));
        }

    }
}
