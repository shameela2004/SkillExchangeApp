using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.User;
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
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Role != "Admin");
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

        public async Task<bool> ApplyMentorAsync(int userId, MentorApplicationDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return false;

            if (user.MentorStatus == "Pending" || user.MentorStatus == "Approved")
                return false; // Already applied or mentor

            var mentorProfile = new MentorProfile
            {
                UserId = userId,
                PhoneNumber = dto.PhoneNumber,
                AadhaarImageUrl = dto.AadhaarImageUrl,
                SocialProfileUrl = dto.SocialProfileUrl,
                Availabilities = new List<MentorAvailability>()
                // Add default or empty availabilities or accept via DTO later
            };

            await _mentorProfileRepository.AddAsync(mentorProfile);

            user.MentorStatus = "Pending";

            await _userRepository.UpdateAsync(user);
            return true;
        }
        public async Task<string> GetMentorApplicationStatusAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);

            if (user == null || user.IsDeleted)
                return string.Empty;

            // Return MentorStatus: None / Pending / Approved / Rejected or default "None"
            return user.MentorStatus ?? "None";
        }

        public async Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId)
        {
            return await _userBadgeRepository.Table
                .Include(ub => ub.Badge)
                .Include(ub => ub.Skill)
                .Where(ub => ub.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<User>> SearchUsersAsync(SearchUserDto filter)
        {
            var query = _userRepository.Table
                .Include(u => u.UserSkills)
                    .ThenInclude(us => us.Skill)
                .Where(u => !u.IsDeleted && u.Role != "Admin");

            if (!string.IsNullOrWhiteSpace(filter.Skill))
            {
                query = query.Where(u => u.UserSkills.Any(us => us.Skill.Name.Contains(filter.Skill)));
            }

            if (!string.IsNullOrWhiteSpace(filter.Location))
            {
                query = query.Where(u => u.Location != null && u.Location.Contains(filter.Location));
            }

            if (!string.IsNullOrWhiteSpace(filter.Role))
            {
                if (filter.Role.Equals("Mentor", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(u => u.MentorStatus == "Approved"); // Only approved mentors
                }
                else if (filter.Role.Equals("Learner", StringComparison.OrdinalIgnoreCase))
                {
                    query = query.Where(u => u.Role == "Learner");
                }
                // For “Both” or empty skip filtering
            }
            return await query.ToListAsync();
        }

    }
}
