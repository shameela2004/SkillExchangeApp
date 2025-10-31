using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Booking;
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
    public class BookingService : IBookingService
    {
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IGenericRepository<Session> _sessionRepo;

        public BookingService(IGenericRepository<Booking> bookingRepo, IGenericRepository<Session> sessionRepo)
        {
            _bookingRepo = bookingRepo;
            _sessionRepo = sessionRepo;
        }

        public async Task<int> BookSessionAsync(BookSessionDto dto)
        {
            var session = await _sessionRepo.GetByIdAsync(dto.SessionId);
            if (session == null) throw new Exception("Session not found");

            var booking = new Booking
            {
                SessionId = dto.SessionId,
                LearnerId = dto.LearnerId,
                PaymentAmount = dto.PaymentAmount ?? 0,
                Status = "Pending",
                IsPaid = false,
                PaymentStatus = "Free",
                IsCancelled = false
            };

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();
            return booking.Id;
        }

        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepo.GetByIdAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepo.Table.Where(b => b.LearnerId == userId && !b.IsCancelled).ToListAsync();
        }

        public async Task<bool> CancelBookingAsync(int bookingId, string? reason)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);
            if (booking == null) return false;

            booking.IsCancelled = true;
            booking.CancellationReason = reason;
            booking.CancelledAt = DateTime.UtcNow;
            booking.Status = "Cancelled";

            await _bookingRepo.UpdateAsync(booking);
            return true;
        }
        public async Task<bool> UpdateBookingStatusAsync(int bookingId, string status)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);
            if (booking == null) return false;

            booking.Status = status;
           await  _bookingRepo.UpdateAsync(booking);
            return true;
        }
    }

}
