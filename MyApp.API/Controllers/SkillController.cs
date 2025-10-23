using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        return Ok(response);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var skill = await _genericService.GetByIdAsync(id);
        if (skill == null)
        {
            return NotFound();
        }
        var response = new SkillResponse
        {
            Id = skill.Id,
            Name = skill.Name,
            CreatedAt = skill.CreatedAt,
            LastUpdatedAt = skill.LastUpdatedAt,
            IsActive = skill.IsActive
        };
        return Ok(response);
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
        return Ok(response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillRequest request)
    {
        await _skillService.UpdateAsync(id, request);
        var updatedSkill = await _genericService.GetByIdAsync(id);
        var response = new SkillResponse
        {
            Id = updatedSkill.Id,
            Name = updatedSkill.Name,
            LastUpdatedAt = updatedSkill.LastUpdatedAt,
            IsActive = updatedSkill.IsActive
        };
        return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _genericService.DeleteAsync(id);
        return NoContent();
    }
}
