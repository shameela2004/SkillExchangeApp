using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IGenericService<User> _userGenericService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper, IGenericService<User> userGenericService)
        {
            _userService = userService;
            _mapper = mapper;
            _userGenericService = userGenericService;
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userGenericService.GetByIdAsync(id);

            if (user == null)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            var userDto = _mapper.Map<UserDto>(user);
            return Ok(ApiResponse<UserDto>.Ok(userDto, "User fetched successfully"));
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ApiResponse<string>.Fail("Invalid request"));

            var existingUser = await _userGenericService.GetByIdAsync(id);
            if (existingUser == null)
                return NotFound(ApiResponse<string>.Fail("User not found"));

            _mapper.Map(updateUserDto, existingUser);

            var success = await _userService.UpdateUserAsync(existingUser);
            if (!success)
                return BadRequest(ApiResponse<string>.Fail("Failed to update profile"));

            return Ok(ApiResponse<string>.Ok(null, "Profile updated successfully"));
        }
    }
}
