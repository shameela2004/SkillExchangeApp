using MyApp1.Application.DTOs.Skill;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface ISkillService
    {
        Task<IEnumerable<Skill>> GetAllAsync();
        Task<Skill?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateSkillDto dto);
        Task<bool> UpdateAsync(int id, UpdateSkillDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
