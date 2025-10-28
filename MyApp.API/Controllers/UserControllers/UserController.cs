using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGenericService<User> _userGenericService;
        private readonly IOtpService _otpService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IGenericService<User> userGenericService, IOtpService otpService)
        {
            _userService = userService;
            _mapper = mapper;
            _userGenericService = userGenericService;
            _otpService = otpService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);

            if (user == null)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(ApiResponse<UserDto>.Ok(userDto, "User fetched successfully"));
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetUserProfile()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userService.GetUserByIdAsync(userId);
            if (user == null) return NotFound(ApiResponse<string>.Fail("User not found"));

            var currentUserDto = _mapper.Map<UserDto>(user);
            return Ok(ApiResponse<UserDto>.Ok(currentUserDto, "Profile fetched successfully"));
        }
        [HttpPut("me")]
        public async Task<IActionResult> UpdateOwnProfile([FromBody] UpdateUserDto updateDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var user = await _userGenericService.GetByIdAsync(userId);
            if (user == null) return NotFound(ApiResponse<string>.Fail("User not found"));

            _mapper.Map(updateDto, user);

            var success = await _userService.UpdateUserAsync(user);
            if (!success) return BadRequest(ApiResponse<string>.Fail("Failed to update profile"));

            return Ok(ApiResponse<string>.Ok(null, "Profile updated successfully"));
        }

        [HttpPost("user/change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0"); 
                await _otpService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword, dto.ConfirmPassword);
                return Ok(ApiResponse<string>.Ok(null,"Password changed successfully."));
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponse<string>.Fail(ex.Message));
            }
        }
        [HttpPost("search")]
        public async Task<IActionResult> SearchUsers([FromBody] SearchUserDto filterDto)
        {
            var users = await _userService.SearchUsersAsync(filterDto);
            var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
            return Ok(ApiResponse<IEnumerable<UserDto>>.Ok(userDtos, "Users fetched"));
        }
        [HttpPost("apply-mentor")]
        public async Task<IActionResult> ApplyMentor([FromBody] MentorApplicationDto mentorDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _userService.ApplyMentorAsync(userId, mentorDto);

            if (!success)
                return BadRequest(ApiResponse<string>.Fail("Mentor application failed or already applied"));

            return Ok(ApiResponse<string>.Ok(null, "Mentor application submitted successfully"));
        }
        [HttpGet("mentor-status")]
        public async Task<IActionResult> GetMentorStatus()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var status = await _userService.GetMentorApplicationStatusAsync(userId);

            if (string.IsNullOrEmpty(status))
                return NotFound(ApiResponse<string>.Fail("Mentor profile not found"));

            return Ok(ApiResponse<string>.Ok(status, "Mentor status fetched successfully"));
        }
        [HttpGet("{id:int}/badges")]
        public async Task<IActionResult> GetUserBadges(int id)
        {
            var badges = await _userService.GetUserBadgesAsync(id);
            var badgesDto = _mapper.Map<IEnumerable<UserBadgeDto>>(badges);

            return Ok(ApiResponse<IEnumerable<UserBadgeDto>>.Ok(badgesDto, "Badges fetched successfully"));
        }


    }
}
