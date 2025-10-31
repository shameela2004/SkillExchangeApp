using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Booking;
using MyApp1.Application.Interfaces.Services;

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
        public async Task<IActionResult> BookSession([FromBody] BookSessionDto dto)
        {
            var bookingId = await _bookingService.BookSessionAsync(dto);
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
    }
}
