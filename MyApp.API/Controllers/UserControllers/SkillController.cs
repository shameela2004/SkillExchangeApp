using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly IGenericService<Skill> _genericSkillService;
        private readonly IUserSkillService _userSkillService;
        private readonly IMapper _mapper;

        public SkillController(IGenericService<Skill> genericSkillService, IUserSkillService userSkillService, IMapper mapper)
        {
            _genericSkillService = genericSkillService;
            _userSkillService = userSkillService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetSkills()
        {
            var skills = await _genericSkillService.GetAllAsync();
            var skillDtos = _mapper.Map<IEnumerable<SkillResponseDto>>(skills);
            return Ok(ApiResponse<IEnumerable<SkillResponseDto>>.SuccessResponse(skillDtos, StatusCodes.Status200OK, "Skills fetched successfully"));
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserSkills()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var userSkills = await _userSkillService.GetUserSkillsAsync(userId);
            var userSkillDtos = _mapper.Map<IEnumerable<UserSkillResponseDto>>(userSkills);
            return Ok(ApiResponse<IEnumerable<UserSkillResponseDto>>.SuccessResponse(userSkillDtos, StatusCodes.Status200OK, "User skills fetched"));
        }

        [HttpPost("user/")]
        public async Task<IActionResult> AddUserSkill([FromBody] AddUserSkillDto addDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _userSkillService.AddUserSkillAsync(userId, addDto);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to add skill"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Skill added successfully"));
        }
        [Authorize]
        [HttpGet("my/teaching-skills")]
        public async Task<IActionResult> GetMyTeachingSkills()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var skills = await _userSkillService.GetTeachingSkillsForUserAsync(userId);
            return Ok(ApiResponse<IEnumerable<SkillDto>>.SuccessResponse(skills, StatusCodes.Status200OK, "Teaching skills fetched"));
        }
        [HttpDelete("user/{skillId}")]
        public async Task<IActionResult> RemoveUserSkill(int skillId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _userSkillService.RemoveUserSkillAsync(userId, skillId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to remove skill"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Skill removed successfully"));
        }
    }
}
