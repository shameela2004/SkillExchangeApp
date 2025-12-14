using MyApp1.Application.DTOs.Dashboard;
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
    public class AdminDashboardService :IAdminDashboardService
    {
        private readonly IGenericRepository<User> _userRepository;
        //private readonly IGenericRepository<MentorProfile> _mentorRepository;
        private readonly IGenericRepository<Booking> _bookingRepository;
        public AdminDashboardService(
            IGenericRepository<User> userRepository,
            //IGenericRepository<MentorProfile> mentorRepository,
            IGenericRepository<Booking> bookingRepository
            )
        {
            _userRepository = userRepository;
            //_mentorRepository = mentorRepository;
            _bookingRepository = bookingRepository;
        }
        public async Task<AdminDashboardSummaryDto> GetDashboardSummaryAsync()
        {
            var users = (await _userRepository.GetAllAsync()) ?? new List<User>();
            var bookings = (await _bookingRepository.GetAllAsync()) ?? new List<Booking>();

            var now = DateTime.UtcNow;
            var today = now.Date;
            // Week start = Monday
            int diff = (7 + (today.DayOfWeek - DayOfWeek.Monday)) % 7;
            var weekStart = today.AddDays(-diff);

            var dto = new AdminDashboardSummaryDto
            {
                TotalUsers = users.Count(),
                ActiveUsers = users.Count(u => u.IsActive),
                TotalMentors = users.Count(u => u.Role == "Mentor"),
                PendingMentors = users.Count(u => u.MentorStatus == "Pending"),
                TotalBookings = bookings.Count(),
                ConfirmedBookings = bookings.Count(b => b.Status == "Confirmed"),
                CancelledBookings = bookings.Count(b => b.Status == "Cancelled" || b.IsCancelled),
                TotalRevenue = bookings
                    .Where(b => b.IsPaid && b.PaymentStatus == "Paid")
                    .Sum(b => b.PaymentAmount),

                // Bookings where we actually have a session with ScheduledAt set
                BookingsToday = bookings.Count(b =>
                    b.Session != null &&
                    b.Session.ScheduledAt.Date == today),

                BookingsThisWeek = bookings.Count(b =>
                    b.Session != null &&
                    b.Session.ScheduledAt.Date >= weekStart &&
                    b.Session.ScheduledAt.Date <= today),

                LastMentorApprovedAt = users
                    .Where(u => u.Role == "Mentor" &&
                                u.MentorStatus == "Approved" &&
                                u.LastUpdatedAt != null)
                    .OrderByDescending(u => u.LastUpdatedAt)
                    .Select(u => u.LastUpdatedAt)
                    .FirstOrDefault()
            };

            return dto;
        }


    }
}
