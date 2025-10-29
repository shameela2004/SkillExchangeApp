using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Application.Services;
using MyApp1.Domain.Entities;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SkillController : ControllerBase
{
    private readonly IGenericService<Skill> _genericService;
    private readonly ISkillService _skillService;

    public SkillController(IGenericService<Skill> genericService, ISkillService skillService)
    {
        _genericService = genericService;
        _skillService = skillService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var skills = await _genericService.GetAllAsync();
        var response = skills.Select(s => new SkillResponse
        {
            Id = s.Id,
            Name = s.Name,
            CreatedAt = s.CreatedAt,
            LastUpdatedAt = s.LastUpdatedAt,
            IsActive = s.IsActive
        });
        return Ok(ApiResponse<object>.SuccessResponse(response,StatusCodes.Status200OK));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _genericService.GetByIdAsync(id);
        if (skill == null)
        {
            return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound,"Skill not found."));
        }
        var response = new SkillResponse
        {
            Id = skill.Id,
            Name = skill.Name,
            CreatedAt = skill.CreatedAt,
            LastUpdatedAt = skill.LastUpdatedAt,
            IsActive = skill.IsActive
        };
        return Ok(ApiResponse<SkillResponse>.SuccessResponse(response,StatusCodes.Status200OK));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateSkillRequest request)
    {
        var skill = new Skill
        {
            Name = request.Name
        };
        await _genericService.AddAsync(skill);
        var response = new SkillResponse
        {
            Id = skill.Id,
            Name = skill.Name,
            CreatedAt = skill.CreatedAt,
            IsActive = skill.IsActive
        };
        return Ok(ApiResponse<SkillResponse>.SuccessResponse(response,StatusCodes.Status200OK, "Skill created successfully."));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSkill(int id, [FromBody] UpdateSkillRequest dto)
    {
        await _skillService.UpdateSkillAsync(id, dto);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _genericService.DeleteAsync(id);
        return NoContent();
    }
}
