using MyApp1.Application.DTOs.Booking;
using MyApp1.Application.DTOs.Payment;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IBookingService
    {
        Task<int> BookSessionAsync(BookSessionDto dto, int learnerId);
        Task<BookingResponseDto> BookGroupSessionAsync(BookSessionDto dto, int learnerId);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<bool> CancelBookingAsync(int bookingId, string? reason);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);
        Task<IEnumerable<BookingDto>> GetMyPastBookingsAsync(int userId);
        Task<IEnumerable<BookingDto>> GetUpcomingBookingsForUserAsync(int userId);
        Task<IEnumerable<Booking>> GetPendingBookingsForMentorAsync(int mentorId);
        Task<bool> ApproveBookingAsync(int bookingId);
        Task<bool> RejectBookingAsync(int bookingId, string? reason);
        Task<bool> VerifyPaymentAsync(VerifyPaymentDto dto);

    }

}
