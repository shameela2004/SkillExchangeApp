using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Services
{
    public class UserSkillService : IUserSkillService
    {
        private readonly IGenericRepository<UserSkill> _userSkillRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Skill> _skillRepository;

        public UserSkillService(IGenericRepository<UserSkill> userSkillRepo,
                                IGenericRepository<User> userRepo,
                                IGenericRepository<Skill> skillRepo)
        {
            _userSkillRepository = userSkillRepo;
            _userRepository = userRepo;
            _skillRepository = skillRepo;
        }

        public async Task<IEnumerable<UserSkill>> GetUserSkillsAsync(int userId)
        {
            return await _userSkillRepository.Table
                .Where(us => us.UserId == userId && !us.IsDeleted)
                .Include(us => us.Skill)
                .ToListAsync();
        }

        public async Task<bool> AddUserSkillAsync(int userId, AddUserSkillDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            var skill = await _skillRepository.GetByIdAsync(dto.SkillId);
            if (skill == null) return false;

            var exists = await _userSkillRepository.Table
                .AnyAsync(us => us.UserId == userId && us.SkillId == dto.SkillId);

            if (exists) return false;

            var userSkill = new UserSkill
            {
                UserId = userId,
                SkillId = dto.SkillId,
                Type = dto.Type
            };

            await _userSkillRepository.AddAsync(userSkill);
            await _userSkillRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserSkillAsync(int userId, int skillId)
        {
            var userSkill = await _userSkillRepository.Table
                .FirstOrDefaultAsync(us => us.UserId == userId && us.SkillId == skillId);
            if (userSkill == null) return false;

            _userSkillRepository.Remove(userSkill);
            await _userSkillRepository.SaveChangesAsync();
            return true;
        }
    

    }
}
