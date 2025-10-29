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
    public class MentorService :IMentorService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<MentorProfile> _mentorProfileRepository;
            public MentorService(
            IGenericRepository<User> userRepository,
            IGenericRepository<MentorProfile> mentorProfileRepository)
        {
            _userRepository = userRepository;
            _mentorProfileRepository = mentorProfileRepository;
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
    }
}
