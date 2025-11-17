using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Auth;
using MyApp1.Application.DTOs.OtpDtos;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Services;
using MyApp1.Domain.Entities;

namespace MyApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IOtpService _otpService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IOtpService otpService, IUserService userService)
        {
            _authService = authService;
            _otpService = otpService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            try
            {
                await _authService.RegisterAsync(request);
                var user= await _userService.GetUserByEmailAsync(request.Email);
                return Ok(ApiResponse<object>.SuccessResponse(user, StatusCodes.Status201Created, "User registered successfully"));
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
                var user= await _userService.GetUserByEmailAsync(request.Email);
                var tokenResponse = await _authService.LoginAsync(request);
                Response.Cookies.Append("accessToken", tokenResponse.Token, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.None,
                    Expires = tokenResponse.Expiration
                });
                // Set refreshToken in HTTP-only cookie if you wish
                Response.Cookies.Append("refreshToken", tokenResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = Request.IsHttps,
                    SameSite = SameSiteMode.None,
                    Expires = DateTimeOffset.Now.AddDays(7)
                });
                return Ok(ApiResponse<Object>.SuccessResponse(new { user = user,tokenResponse =tokenResponse}, StatusCodes.Status200OK));
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

        //[HttpPost("refresh")]
        //public async Task<IActionResult> Refresh([FromBody] TokenRequest request)
        //{
        //    var tokens = await _authService.RefreshTokenAsync(request.Token, request.RefreshToken);
        //    return Ok(ApiResponse<object>.SuccessResponse(new { token = tokens.Token, refreshToken = tokens.RefreshToken },StatusCodes.Status200OK,"Refresh Token Created Successfully"));
        //}

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var accessToken = Request.Cookies["accessToken"];

            if (string.IsNullOrEmpty(refreshToken))
                return Unauthorized(ApiResponse<string>.FailResponse(StatusCodes.Status401Unauthorized, "No refresh token"));

            // Validate/issue new tokens
            var tokens = await _authService.RefreshTokenAsync(accessToken, refreshToken);

            Response.Cookies.Append("accessToken", tokens.Token, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.Now.AddHours(2)
            });
            Response.Cookies.Append("refreshToken", tokens.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.None,
                Expires = DateTimeOffset.Now.AddDays(7)
            });
            return Ok(ApiResponse<object>.SuccessResponse(new { }, StatusCodes.Status200OK, "Refresh Token Created Successfully"));
        }


        //[HttpPost("revoke")]
        //public async Task<IActionResult> Revoke([FromBody] RevokeRequest request)
        //{
        //    await _authService.RevokeRefreshTokenAsync(request.RefreshToken);
        //    Response.Cookies.Delete("accessToken");
        //    Response.Cookies.Delete("refreshToken");
        //    return NoContent();
        //}
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            if (!string.IsNullOrEmpty(refreshToken))
            {
                // Revoke refresh token from DB if it exists
                await _authService.RevokeRefreshTokenAsync(refreshToken);
            }

            // Delete auth cookies
            Response.Cookies.Delete("accessToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.None
            });

            Response.Cookies.Delete("refreshToken", new CookieOptions
            {
                HttpOnly = true,
                Secure = Request.IsHttps,
                SameSite = SameSiteMode.None
            });

            return Ok(ApiResponse<string>.SuccessResponse(
                "Logged out successfully",
                StatusCodes.Status200OK,
                "User logged out"
            ));
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
