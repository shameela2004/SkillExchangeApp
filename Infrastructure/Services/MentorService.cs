using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Mentor;
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
        private readonly INotificationService _notificationService;
        private readonly INotificationSender _notificationSender;
        private readonly IMapper _mapper;
            public MentorService(
            IGenericRepository<User> userRepository,
            IGenericRepository<MentorProfile> mentorProfileRepository,
            INotificationService notificationService,
            INotificationSender notificationSender,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _mentorProfileRepository = mentorProfileRepository;
            _notificationService = notificationService;
            _notificationSender = notificationSender;
            _mapper = mapper;
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




        //   Availabilities Management
        public async Task<List<MentorAvailabilityDto>> GetAvailabilitiesAsync(int userId)
        {
            var mentorProfile = await _mentorProfileRepository.Table
                .Include(mp => mp.Availabilities)
                .FirstOrDefaultAsync(mp => mp.UserId == userId);

            if (mentorProfile == null)
                return new List<MentorAvailabilityDto>();

            var availabilities = mentorProfile.Availabilities
                .Where(a => !a.IsDeleted)
                .Select(a => new MentorAvailabilityDto
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList();

            return availabilities;
        }

        public async Task<bool> AddAvailabilitiesAsync(int userId, IList<MentorAvailabilityDto> availabilities)
        {
            var mentorProfile = await _mentorProfileRepository.Table
                .Include(m => m.Availabilities)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (mentorProfile == null)
                return false;

            foreach (var a in availabilities)
            {
                mentorProfile.Availabilities.Add(new MentorAvailability
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                });
            }

            await _mentorProfileRepository.UpdateAsync(mentorProfile);
            return true;
        }

        public async Task<bool> UpdateAvailabilityAsync(int userId, int availabilityId, MentorAvailabilityDto dto)
        {
            var mentorProfile = await _mentorProfileRepository.Table
                .Include(m => m.Availabilities)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (mentorProfile == null)
                return false;

            var availability = mentorProfile.Availabilities.FirstOrDefault(a => a.Id == availabilityId);

            if (availability == null)
                return false;

            availability.DayOfWeek = dto.DayOfWeek;
            availability.StartTime = dto.StartTime;
            availability.EndTime = dto.EndTime;

            await _mentorProfileRepository.UpdateAsync(mentorProfile);
            return true;
        }

        public async Task<bool> DeleteAvailabilityAsync(int userId, int availabilityId)
        {
            var mentorProfile = await _mentorProfileRepository.Table
                .Include(m => m.Availabilities)
                .FirstOrDefaultAsync(m => m.UserId == userId);

            if (mentorProfile == null)
                return false;

            var availability = mentorProfile.Availabilities.FirstOrDefault(a => a.Id == availabilityId);

            if (availability == null)
                return false;

            // Soft delete by setting flags
            availability.IsDeleted = true;
            availability.DeletedAt = DateTime.UtcNow;

            await _mentorProfileRepository.UpdateAsync(mentorProfile);
            return true;
        }





        public async Task<IEnumerable<MentorDto>> GetAllMentorsAsync(string? statusFilter = null)
        {
            var query = _userRepository.Table
                .Include(u => u.MentorProfile)
                .ThenInclude(mp => mp.Availabilities)
                .AsQueryable();

            if (!string.IsNullOrEmpty(statusFilter))
                query = query.Where(u => u.MentorStatus == statusFilter);

            var mentors = await query.ToListAsync();

            return mentors.Select(u => new MentorDto
            {
                UserId = u.Id,
                Name = u.Name,
                Email = u.Email,
                MentorStatus = u.MentorStatus ?? "None",
                PhoneNumber = u.MentorProfile?.PhoneNumber ?? string.Empty,
                AadhaarImageUrl = u.MentorProfile?.AadhaarImageUrl ?? string.Empty,
                SocialProfileUrl = u.MentorProfile?.SocialProfileUrl ?? string.Empty,
                Availabilities = u.MentorProfile?.Availabilities.Select(a => new MentorAvailabilityDto
                {
                    DayOfWeek = a.DayOfWeek,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime
                }).ToList() ?? new List<MentorAvailabilityDto>()
            }).ToList();
        }

        //public async Task<IEnumerable<MentorDto>> GetPendingMentorApplicationsAsync()
        //{
        //    var pendingUsers = await _userRepository.Table
        //        .Include(u => u.MentorProfile)
        //        .ThenInclude(mp => mp.Availabilities)
        //        .Where(u => u.MentorStatus == "Pending" && !u.IsDeleted)
        //        .OrderByDescending(u => u.CreatedAt)
        //        .ToListAsync();

        //    return _mapper.Map<IEnumerable<MentorDto>>(pendingUsers);
        //}


        public async Task<bool> ApproveMentorAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted || user.MentorStatus == "Approved")
                return false;

            user.MentorStatus = "Approved";
            user.Role = "Mentor"; // Change role to Mentor
            await _userRepository.UpdateAsync(user);
            // Create notification
            var notificationId = await _notificationService.CreateNotificationAsync(
                userId,
                "Mentor Status Changed",
                "Congratulations! Your mentor status has been approved.",
                "Mentor");

            // Push real-time notification with SignalR Hub
            // Push real-time notification using INotificationSender abstraction
            await _notificationSender.SendNotification(userId.ToString(), "Congratulations! Your mentor status has been approved.");
            return true;
        }


        public async Task<bool> RejectMentorAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null || user.IsDeleted || user.MentorStatus == "Rejected")
                return false;

            user.MentorStatus = "Rejected";
            user.Role = "Learner"; // Revert role to Learner
            await _userRepository.UpdateAsync(user);
            // Create notification
            await _notificationService.CreateNotificationAsync(
                userId,
                "Mentor Status Changed",
                "Your mentor request has been rejected.",
                "Mentor");

            // Send real-time notification
            await _notificationSender.SendNotification(userId.ToString(), "Your mentor request has been rejected.");
            return true;
        }



        public async Task<bool> IsMentorApprovedAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.MentorStatus == "Approved";
        }


    }
}
