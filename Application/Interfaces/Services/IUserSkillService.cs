using MyApp1.Application.DTOs.Skill;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IUserSkillService
    {
        Task<IEnumerable<UserSkill>> GetUserSkillsAsync(int userId);
        Task<IEnumerable<SkillDto>> GetTeachingSkillsForUserAsync(int userId);
        Task<bool> AddUserSkillAsync(int userId, AddUserSkillDto dto);
        Task<bool> RemoveUserSkillAsync(int userId, int skillId);
    }
}
