using AutoMapper;
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
        private readonly IGenericRepository<UserBadge> _userBadgeRepository;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;

        public UserService(
            IGenericRepository<User> userRepository,
            IGenericRepository<UserSkill> userSkillRepository,
            IGenericRepository<UserBadge> userBadgeRepository,
            IMapper mapper,
            IMediaService mediaService
            )
        {
            _userRepository = userRepository;
            _userSkillRepository = userSkillRepository;
            _userBadgeRepository = userBadgeRepository;
            _mapper = mapper;
            _mediaService = mediaService;
        }

        public async Task<IEnumerable<UserDto>> GetAllUserDtosAsync()
        {
            // Get all non-deleted, non-admin users + eager load related entities if needed
            var users = await _userRepository.Table
                .Where(u => !u.IsDeleted && u.Role != "Admin")
                .ToListAsync();

            var userDtos = _mapper.Map<List<UserDto>>(users);

            // Populate ProfilePictureUrl for each user
            foreach (var userDto in userDtos)
            {
                var profileMedia = await _mediaService.GetMediaByReferenceAsync("UserProfile", userDto.Id);
                var profileImage = profileMedia
                    .OrderByDescending(m => m.Id)
                    .FirstOrDefault();

                if (profileImage != null)
                {
                    userDto.ProfilePictureUrl = $"/api/media/{profileImage.Id}";
                }
            }

            return userDtos;
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _userRepository.Table
                .Include(u => u.UserSkills.Where(us => !us.IsDeleted))
                    .ThenInclude(us => us.Skill)
                 .Include(u => u.UserLanguages.Where(us => !us.IsDeleted))
                    .ThenInclude(ul => ul.Language)
                    .Include(u => u.MentorProfile)
            .ThenInclude(mp => mp.Availabilities.Where(a => !a.IsDeleted))
            .Include(u=>u.Posts)
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Role != "Admin");

        }


        public async Task<UserDto> GetUserDtoByIdAsync(int id)
        {
            var user = await _userRepository.Table
                 .Include(u => u.UserSkills.Where(us => !us.IsDeleted))
                    .ThenInclude(us => us.Skill)
                 .Include(u => u.UserLanguages.Where(us => !us.IsDeleted))
                    .ThenInclude(ul => ul.Language)
                    .Include(u => u.MentorProfile)
            .ThenInclude(mp => mp.Availabilities.Where(a => !a.IsDeleted))
            .Include(u => u.Posts)
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted && u.Role != "Admin");

            if (user == null)
                return null;

            var userDto = _mapper.Map<UserDto>(user);

            var profileMedia = await _mediaService.GetMediaByReferenceAsync("UserProfile", user.Id);
            var profileImage = profileMedia.OrderByDescending(m => m.Id).FirstOrDefault();

            //var profileImage = profileMedia.FirstOrDefault();

            if (profileImage != null)
            {
                userDto.ProfilePictureUrl = $"/api/media/{profileImage.Id}";
            }
            user.ProfilePictureUrl=userDto.ProfilePictureUrl;
            return userDto;
        }


        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _userRepository.Table
                .Include(u => u.UserSkills.Where(us => !us.IsDeleted))
                    .ThenInclude(us => us.Skill)
                 .Include(u => u.UserLanguages.Where(ul => !ul.IsDeleted))
                    .ThenInclude(ul => ul.Language)
                    .Include(u => u.MentorProfile)
            .ThenInclude(mp => mp.Availabilities.Where(a => !a.IsDeleted))
             .Include(u => u.Posts)
                .Include(u => u.UserBadges)
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted && u.Role != "Admin");
        }
        public async Task<bool> UpdateUserAsync(int userId,UpdateUserDto updatedUser)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted)
                return false;

            // Update allowed fields
            user.Name = updatedUser.Name;
            user.Bio = updatedUser.Bio;
            user.Location = updatedUser.Location;
            user.LastUpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
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
