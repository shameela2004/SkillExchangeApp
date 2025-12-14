using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminSkillController : ControllerBase
    {
        private readonly ISkillService _skillService;
        private readonly IMapper _mapper;

        public AdminSkillController(ISkillService skillService, IMapper mapper)
        {
            _skillService = skillService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _skillService.GetAllAsync();
            var dto = _mapper.Map<IEnumerable<SkillDto>>(skills);
            return Ok(ApiResponse<IEnumerable<SkillDto>>.SuccessResponse(
                dto, StatusCodes.Status200OK, "Skills fetched"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var skill = await _skillService.GetByIdAsync(id);
            if (skill == null)
                return NotFound(ApiResponse<string>.FailResponse(
                    StatusCodes.Status404NotFound, "Skill not found"));

            var dto = _mapper.Map<SkillDto>(skill);
            return Ok(ApiResponse<SkillDto>.SuccessResponse(
                dto, StatusCodes.Status200OK, "Skill details"));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSkillDto dto)
        {
            var id = await _skillService.CreateAsync(dto);
            return Ok(ApiResponse<int>.SuccessResponse(
                id, StatusCodes.Status201Created, "Skill created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillDto dto)
        {
            var ok = await _skillService.UpdateAsync(id, dto);
            if (!ok)
                return BadRequest(ApiResponse<string>.FailResponse(
                    StatusCodes.Status400BadRequest, "Update failed"));

            return Ok(ApiResponse<string>.SuccessResponse(
                "Updated", StatusCodes.Status200OK, "Skill updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _skillService.DeleteAsync(id);
            if (!ok)
                return BadRequest(ApiResponse<string>.FailResponse(
                    StatusCodes.Status400BadRequest, "Delete failed"));

            return Ok(ApiResponse<string>.SuccessResponse(
                "Deleted", StatusCodes.Status200OK, "Skill deleted"));
        }
    }
}
