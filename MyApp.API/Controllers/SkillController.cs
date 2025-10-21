using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;

namespace MyApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private readonly IGenericService<Skill> _skillService;

        public SkillController(IGenericService<Skill> skillService)
        {
            _skillService = skillService;
        }

        // GET: api/Skill
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var skills = await _skillService.GetAllAsync();

            // Map entities to response DTOs
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

        // POST: api/Skill
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSkillRequest request)
        {
            var skill = new Skill
            {
                Name = request.Name
            };

            await _skillService.AddAsync(skill);

            // Return response DTO
            var response = new SkillResponse
            {
                Id = skill.Id,
                Name = skill.Name,
                CreatedAt = skill.CreatedAt,
                IsActive = skill.IsActive
            };

            return Ok(response);
        }

        // PUT: api/Skill/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSkillRequest request)
        {
            var skill = new Skill
            {
                Id = id,
                Name = request.Name,
                LastUpdatedAt = DateTime.UtcNow
            };

            await _skillService.UpdateAsync(id, skill);

            var response = new SkillResponse
            {
                Id = skill.Id,
                Name = skill.Name,
                LastUpdatedAt = skill.LastUpdatedAt,
                IsActive = skill.IsActive
            };

            return Ok(response);
        }

    }
}
