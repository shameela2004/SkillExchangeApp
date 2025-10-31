using MyApp1.Application.DTOs.Booking;
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
        Task<int> BookSessionAsync(BookSessionDto dto);
        Task<Booking?> GetBookingByIdAsync(int bookingId);
        Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId);
        Task<bool> CancelBookingAsync(int bookingId, string? reason);
        Task<bool> UpdateBookingStatusAsync(int bookingId, string status);

    }

}
