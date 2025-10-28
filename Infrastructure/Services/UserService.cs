using Microsoft.EntityFrameworkCore;
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
    public class UserService :IUserService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<UserSkill> _userSkillRepository;
        private readonly IGenericRepository<MentorProfile> _mentorProfileRepository;
        private readonly IGenericRepository<UserBadge> _userBadgeRepository;

        public UserService(
            IGenericRepository<User> userRepository,
            IGenericRepository<UserSkill> userSkillRepository,
            IGenericRepository<MentorProfile> mentorProfileRepository,
            IGenericRepository<UserBadge> userBadgeRepository)
        {
            _userRepository = userRepository;
            _userSkillRepository = userSkillRepository;
            _mentorProfileRepository = mentorProfileRepository;
            _userBadgeRepository = userBadgeRepository;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.Table
                .Include(u => u.UserSkills)
                    .ThenInclude(us => us.Skill)
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<bool> UpdateUserAsync(User updatedUser)
        {
            var user = await _userRepository.GetByIdAsync(updatedUser.Id);
            if (user == null || user.IsDeleted)
                return false;

            // Update allowed fields
            user.Name = updatedUser.Name;
            user.Bio = updatedUser.Bio;
            user.Location = updatedUser.Location;
            user.ProfilePictureUrl = updatedUser.ProfilePictureUrl;
            user.LastUpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(string? skill, string? location)
        {
            var query = _userRepository.Table.AsQueryable().Where(u => !u.IsDeleted);

            if (!string.IsNullOrWhiteSpace(skill))
            {
                query = query.Where(u => u.UserSkills.Any(us => us.Skill.Name.Contains(skill)));
            }

            if (!string.IsNullOrWhiteSpace(location))
            {
                query = query.Where(u => u.Location != null && u.Location.Contains(location));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> ApplyMentorAsync(MentorProfile mentorProfile)
        {
            // Check if user already has a mentor profile
            var existingProfile = await _mentorProfileRepository.Table
                .FirstOrDefaultAsync(mp => mp.UserId == mentorProfile.UserId);

            if (existingProfile != null)
                return false; // Already applied or approved

            // Insert new mentor profile with Pending status managed elsewhere
            await _mentorProfileRepository.AddAsync(mentorProfile);
            return true;
        }

        public async Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId)
        {
            return await _userBadgeRepository.Table
                .Include(ub => ub.Badge)
                .Include(ub => ub.Skill)
                .Where(ub => ub.UserId == userId)
                .ToListAsync();
        }
    }
}
