using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Collections.Generic;
using System.Security.Claims;

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
    [HttpGet]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userGenericService.GetAllAsync();
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(userDtos, StatusCodes.Status200OK, "Users fetched"));
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userService.GetUserByIdAsync(id);

        if (user == null)
            return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "User not found"));

        var userDto = _mapper.Map<UserDto>(user);
        return Ok(ApiResponse<UserDto>.SuccessResponse(userDto, StatusCodes.Status200OK, "User fetched successfully"));
    }

    [HttpGet("me")]
    public async Task<IActionResult> GetUserProfile()
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var user = await _userService.GetUserByIdAsync(userId);
        if (user == null)
            return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "User not found"));

        var currentUserDto = _mapper.Map<UserDto>(user);
        return Ok(ApiResponse<UserDto>.SuccessResponse(currentUserDto, StatusCodes.Status200OK, "Profile fetched successfully"));
    }

    [HttpPut("me")]
    public async Task<IActionResult> UpdateOwnProfile([FromBody] UpdateUserDto updateDto)
    {
        var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        var user = await _userGenericService.GetByIdAsync(userId);
        if (user == null)
            return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "User not found"));

        _mapper.Map(updateDto, user);

        var success = await _userService.UpdateUserAsync(user);
        if (!success)
            return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to update profile"));

        return Ok(ApiResponse <UpdateUserDto>.SuccessResponse(updateDto, StatusCodes.Status200OK, "Profile updated successfully"));
    }

    [HttpPost("user/change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("Id")?.Value ?? "0");
            await _otpService.ChangePasswordAsync(userId, dto.CurrentPassword, dto.NewPassword, dto.ConfirmPassword);
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Password changed successfully."));
        }
        catch (Exception ex)
        {
            return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, ex.Message));
        }
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchUsers([FromBody] SearchUserDto filterDto)
    {
        var users = await _userService.SearchUsersAsync(filterDto);
        var userDtos = _mapper.Map<IEnumerable<UserDto>>(users);
        return Ok(ApiResponse<IEnumerable<UserDto>>.SuccessResponse(userDtos, StatusCodes.Status200OK, "Users fetched"));
    }

    [HttpGet("{id:int}/badges")]
    public async Task<IActionResult> GetUserBadges(int id)
    {
        var badges = await _userService.GetUserBadgesAsync(id);
        var badgesDto = _mapper.Map<IEnumerable<UserBadgeDto>>(badges);

        return Ok(ApiResponse<IEnumerable<UserBadgeDto>>.SuccessResponse(badgesDto, StatusCodes.Status200OK, "Badges fetched successfully"));
    }
}
