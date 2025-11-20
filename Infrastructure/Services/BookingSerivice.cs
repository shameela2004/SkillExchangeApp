using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Booking;
using MyApp1.Application.DTOs.Payment;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using MyApp1.Infrastructure.Helpers;
using MyApp1.Infrastructure.RazorPay;
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
        private readonly IGenericRepository<GroupSession> _groupSessionRepo;
        private readonly IGenericRepository<GroupMember> _groupMemberRepo;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaService;
        private readonly IGenericRepository<Payment> _paymentRepo;
        private readonly RazorpayService _razorpayService;

        public BookingService(IGenericRepository<Booking> bookingRepo, IGenericRepository<Session> sessionRepo, IGenericRepository<GroupSession> groupSessionRepo, IGenericRepository<GroupMember> groupMemberRepo, RazorpayService razorpayService, IGenericRepository<Payment> paymentRepo,IMapper mapper,IMediaService mediaService)
        {
            _bookingRepo = bookingRepo;
            _sessionRepo = sessionRepo;
            _groupSessionRepo = groupSessionRepo;
            _groupMemberRepo = groupMemberRepo;
            _razorpayService = razorpayService;
            _mapper = mapper;
            _mediaService = mediaService;
            _paymentRepo = paymentRepo;
        }
        public async Task<int> BookSessionAsync(BookSessionDto dto, int learnerId)
        {
            var session = await _sessionRepo.GetByIdAsync(dto.SessionId);
            if (session == null)
                throw new Exception("Session not found");

            var existingBooking = await _bookingRepo.Table
                .FirstOrDefaultAsync(b => b.LearnerId == learnerId && b.SessionId == dto.SessionId && !b.IsCancelled);

            if (existingBooking != null)
                throw new Exception("Already booked");

            var booking = new Booking
            {
                SessionId = dto.SessionId,
                GroupSessionId = null,
                LearnerId = learnerId,
                PaymentAmount = session.Price,
                Status = "Pending",
                IsPaid = false,
                PaymentStatus = session.Price > 0 ? "Pending" : "Free",
                IsCancelled = false
            };

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            if (session.Price > 0)
            {
                var razorpayOrder = _razorpayService.CreateOrder((int)session.Price);
                var payment = new Payment
                {
                    BookingId = booking.Id,
                    LearnerId = learnerId,
                    MentorId = session.MentorId,
                    Amount = session.Price,
                    PaymentMethod = "Razorpay",
                    Status = "Created",
                    RazorpayOrderId = razorpayOrder["id"].ToString(),
                    CommissionPercent = 0, // assign as needed later
                    CommissionAmount = 0,
                    MentorAmount = 0,
                    SettlementStatus = "Pending"
                };

                await _paymentRepo.AddAsync(payment);
                await _paymentRepo.SaveChangesAsync();

                booking.PaymentId = payment.Id;
                await _bookingRepo.UpdateAsync(booking);
            }
            else
            {
                booking.Status = "Pending";
                booking.IsPaid = true;
                booking.PaymentStatus = "Free";
                await _bookingRepo.UpdateAsync(booking);
            }

            return booking.Id;
        }

        public async Task<BookingResponseDto> BookGroupSessionAsync(BookSessionDto dto, int learnerId)
        {
            //var groupSession = await _groupSessionRepo.GetByIdAsync(dto.SessionId);
            var groupSession = await _groupSessionRepo.Table
     .Include(gs => gs.Group) // Eagerly load Group navigation property
     .FirstOrDefaultAsync(gs => gs.Id == dto.SessionId);
            if (groupSession == null)
                throw new Exception("Group session not found");

            var isMember = await _groupMemberRepo.Table.AnyAsync(m =>
                m.GroupId == groupSession.GroupId && m.UserId == learnerId && !m.IsDeleted);
            if (!isMember)
                throw new Exception("User is not a member of the group");

            var existingBooking = await _bookingRepo.Table
                .FirstOrDefaultAsync(b => b.LearnerId == learnerId && b.GroupSessionId == dto.SessionId && !b.IsCancelled);

            if (existingBooking != null)
                throw new Exception("Already booked");

            var booking = new Booking
            {
                SessionId = null,
                GroupSessionId = dto.SessionId,
                LearnerId = learnerId,
                PaymentAmount = groupSession.Price ?? 0,
                Status = groupSession.Price > 0 ? "Pending" : "Free",
                IsPaid = groupSession.Price == 0,
                PaymentStatus = groupSession.Price == 0 ? "Free" : "Pending",
                IsCancelled = false
            };

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            if (groupSession.Price > 0)
            {
                var razorpayOrder = _razorpayService.CreateOrder((int)groupSession.Price);
                var payment = new Payment
                {
                    BookingId = booking.Id,
                    LearnerId = learnerId,
                    MentorId = groupSession.Group.MentorId,
                    Amount = groupSession.Price ?? 0,
                    PaymentMethod = "Razorpay",
                    Status = "Created",
                    RazorpayOrderId = razorpayOrder["id"].ToString(),
                    CommissionPercent = 0,
                    CommissionAmount = 0,
                    MentorAmount = 0,
                    SettlementStatus = "Pending"
                };

                await _paymentRepo.AddAsync(payment);
                await _paymentRepo.SaveChangesAsync();

                booking.PaymentId = payment.Id;
                await _bookingRepo.UpdateAsync(booking);
            }
            else
            {
                booking.Status = "Confirmed";
                booking.IsPaid = true;
                booking.PaymentStatus = "Free";
                await _bookingRepo.UpdateAsync(booking);
            }
            var getPayment = await _paymentRepo.GetByIdAsync(booking.PaymentId.GetValueOrDefault());

            var response = new BookingResponseDto
            {
                BookingId = booking.Id,
                PaymentId = getPayment?.Id ?? 0,
                RazorpayOrderId = getPayment?.RazorpayOrderId ?? string.Empty
            };

            return response;
        }


        public async Task<Booking?> GetBookingByIdAsync(int bookingId)
        {
            return await _bookingRepo.GetByIdAsync(bookingId);
        }

        public async Task<IEnumerable<Booking>> GetBookingsByUserIdAsync(int userId)
        {
            return await _bookingRepo.Table.Where(b => b.LearnerId == userId ).ToListAsync();
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

        public async Task<IEnumerable<BookingDto>> GetMyPastBookingsAsync(int userId)
        {
            var bookings = await _bookingRepo.Table
                .Include(b => b.Learner)
                 .Include(b => b.Session)
        .ThenInclude(s => s.Skill)
    .Include(b => b.Session)
        .ThenInclude(s => s.Mentor)
                .Where(b => b.LearnerId == userId && (b.Session.ScheduledAt < DateTime.Now || b.IsCancelled))
                .ToListAsync();
            var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);

            foreach (var bookingDto in bookingDtos)
            {
                var profileMedia = await _mediaService.GetMediaByReferenceAsync("UserProfile", bookingDto.MentorId);

                var profileImage = profileMedia.OrderByDescending(m => m.Id).FirstOrDefault();

                if (profileImage != null)
                {
                    bookingDto.MentorProfilePictureUrl = $"/api/media/{profileImage.Id}";
                }
            }

            return bookingDtos;



        }

        public async Task<IEnumerable<BookingDto>> GetUpcomingBookingsForUserAsync(int userId)
        {
            var bookings = await _bookingRepo.Table
                .Include(b => b.Learner)
                .Include(b => b.Session)
                .ThenInclude(s => s.Skill)
                .Where(b => b.LearnerId == userId && b.Session.ScheduledAt>DateTime.Now && !b.IsCancelled)
                .ToListAsync();
            var bookingDtos = _mapper.Map<List<BookingDto>>(bookings);

            foreach (var bookingDto in bookingDtos)
            {
                var profileMedia = await _mediaService.GetMediaByReferenceAsync("UserProfile", bookingDto.MentorId);

                var profileImage = profileMedia.OrderByDescending(m => m.Id).FirstOrDefault();

                if (profileImage != null)
                {
                    bookingDto.MentorProfilePictureUrl = $"/api/media/{profileImage.Id}";
                }
            }
            return bookingDtos;
        }

        public async Task<IEnumerable<Booking>> GetPendingBookingsForMentorAsync(int mentorId)
        {
            return await _bookingRepo.Table
                .Include(b=>b.Learner)
                .Include(b => b.Session)
                .ThenInclude(s => s.Skill)
                .Where(b => b.Session.MentorId == mentorId && b.Status == "Pending" && !b.IsCancelled)
                .ToListAsync();
        }

        public async Task<bool> ApproveBookingAsync(int bookingId)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);
            if (booking == null || booking.IsCancelled || booking.Status != "Pending")
                return false;

            booking.Status = "Confirmed";
            booking.IsPaid = booking.PaymentAmount == 0 ? true : booking.IsPaid;
            booking.PaymentStatus = booking.PaymentAmount == 0 ? "Free" : booking.PaymentStatus;

            await _bookingRepo.UpdateAsync(booking);
            return true;
        }

        public async Task<bool> RejectBookingAsync(int bookingId, string? reason)
        {
            var booking = await _bookingRepo.GetByIdAsync(bookingId);
            if (booking == null || booking.IsCancelled || booking.Status != "Pending")
                return false;

            booking.Status = "Rejected";
            booking.IsCancelled = true;
            booking.CancellationReason = reason;
            booking.CancelledAt = DateTime.UtcNow;

            await _bookingRepo.UpdateAsync(booking);
            return true;
        }

        public async Task<bool> VerifyPaymentAsync(VerifyPaymentDto dto)
        {
            var isValid = _razorpayService.VerifyPayment(dto);
            if (!isValid) return false;

            var payment = await _paymentRepo.Table.FirstOrDefaultAsync(p => p.RazorpayOrderId == dto.RazorpayOrderId);
            if (payment == null) return false;

            payment.Status = "Paid";
            payment.RazorpayPaymentId = dto.RazorpayPaymentId;
            payment.RazorpaySignature = dto.RazorpaySignature;
            await _paymentRepo.UpdateAsync(payment);

            var booking = await _bookingRepo.GetByIdAsync(payment.BookingId);
            if (booking != null)
            {
                booking.Status = "Confirmed";
                booking.IsPaid = true;
                booking.PaymentStatus = "Paid";
                await _bookingRepo.UpdateAsync(booking);
            }

            return true;
        }

    }

}
