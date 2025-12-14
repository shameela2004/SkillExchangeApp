using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Exceptions;
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
        private readonly IGenericRepository<Skill> _skillRepo;

        public SkillService(IGenericRepository<Skill> skillRepo)
        {
            _skillRepo = skillRepo;
        }

        public async Task<IEnumerable<Skill>> GetAllAsync()
        {
            return await _skillRepo.Table
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.Name)
                .ToListAsync();
        }

        public async Task<Skill?> GetByIdAsync(int id)
        {
            var skill = await _skillRepo.GetByIdAsync(id);
            return skill == null || skill.IsDeleted ? null : skill;
        }

        public async Task<int> CreateAsync(CreateSkillDto dto)
        {
            var name = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name is required");

            var skill = new Skill { Name = name };
            await _skillRepo.AddAsync(skill);
            await _skillRepo.SaveChangesAsync();
            return skill.Id;
        }

        public async Task<bool> UpdateAsync(int id, UpdateSkillDto dto)
        {
            var skill = await _skillRepo.GetByIdAsync(id);
            if (skill == null || skill.IsDeleted) return false;

            var name = dto.Name.Trim();
            if (string.IsNullOrWhiteSpace(name)) return false;

            skill.Name = name;
            skill.IsDeleted = !dto.IsActive;

            await _skillRepo.UpdateAsync(skill);
            await _skillRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var skill = await _skillRepo.GetByIdAsync(id);
            if (skill == null || skill.IsDeleted) return false;

            // soft delete
            skill.IsDeleted = true;
            await _skillRepo.UpdateAsync(skill);
            await _skillRepo.SaveChangesAsync();
            return true;
        }
    }
}
