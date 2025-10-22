using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Services
{
    public class SkillService : ISkillService
    {
        private readonly IGenericRepository<Skill> _repository;

        public SkillService(IGenericRepository<Skill> repository)
        {
            _repository = repository;
        }

        public async Task UpdateAsync(int id, UpdateSkillRequest dto)
        {
            var skill = await _repository.GetByIdAsync(id);
            if (skill == null)
                throw new KeyNotFoundException($"Skill with id {id} not found");

            skill.Name = dto.Name;
            skill.LastUpdatedAt = DateTime.UtcNow;
            await _repository.SaveChangesAsync();
        }
    }
}
