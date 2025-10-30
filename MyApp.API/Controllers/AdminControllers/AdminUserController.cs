using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminUserController : ControllerBase
    {

        private readonly IUserService _userService;
        private readonly IGenericService<User> _genericUserService;
        private readonly IMapper _mapper;

        public AdminUserController(IUserService userService, IGenericService<User> genericUserService, IMapper mapper)
        {
            _userService = userService;
            _genericUserService = genericUserService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _genericUserService.GetAllAsync();
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
    }
}
