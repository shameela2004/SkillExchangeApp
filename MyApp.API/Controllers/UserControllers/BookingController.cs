using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Booking;
using MyApp1.Application.Interfaces.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public BookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> BookSession([FromBody] BookSessionDto dto)
        {
            var learnerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var bookingId = await _bookingService.BookSessionAsync(dto, learnerId);
            return Ok(ApiResponse<int>.SuccessResponse(bookingId, StatusCodes.Status201Created, "Session booked"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBooking(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Booking not found"));

            var dto = _mapper.Map<BookingDto>(booking);
            return Ok(ApiResponse<BookingDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Booking details"));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetBookingsForUser(int userId)
        {
            var bookings = await _bookingService.GetBookingsByUserIdAsync(userId);
            var dtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(dtos, StatusCodes.Status200OK, "User bookings fetched"));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] string? reason)
        {
            var success = await _bookingService.CancelBookingAsync(id, reason);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Cancellation failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Cancelled", StatusCodes.Status200OK, "Booking cancelled"));
        }
        [Authorize(Roles = "Mentor")]
        [HttpGet("pending")]
        public async Task<IActionResult> GetPendingBookings()
        {
            var mentorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var bookings = await _bookingService.GetPendingBookingsForMentorAsync(mentorId);
            var dtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(dtos, 200, "Pending bookings fetched"));
        }

        [Authorize(Roles = "Mentor")]
        [HttpPost("{bookingId}/approve")]
        public async Task<IActionResult> ApproveBooking(int bookingId)
        {
            var success = await _bookingService.ApproveBookingAsync(bookingId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to approve booking"));
            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Booking approved"));
        }

        [Authorize(Roles = "Mentor")]
        [HttpPost("{bookingId}/reject")]
        public async Task<IActionResult> RejectBooking(int bookingId, [FromBody] string? reason)
        {
            var success = await _bookingService.RejectBookingAsync(bookingId, reason);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to reject booking"));
            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Booking rejected"));
        }

        //  Get meeting link after the booking is approved and also in session creation the meeting link must be generated
    }
}
