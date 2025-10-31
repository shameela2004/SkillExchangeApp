using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Booking;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminBookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IGenericService<Booking> _genericBookingService;
        private readonly IMapper _mapper;


        public AdminBookingController(IBookingService bookingService, IGenericService<Booking> genericBookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _genericBookingService = genericBookingService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _genericBookingService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<BookingDto>>(bookings);
            return Ok(ApiResponse<IEnumerable<BookingDto>>.SuccessResponse(dtos, StatusCodes.Status200OK, "All bookings fetched"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingById(int id)
        {
            var booking = await _bookingService.GetBookingByIdAsync(id);
            if (booking == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Booking not found"));
            var dto = _mapper.Map<BookingDto>(booking);
            return Ok(ApiResponse<BookingDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Booking details fetched"));
        }

        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> CancelBooking(int id, [FromBody] string? reason)
        {
            var success = await _bookingService.CancelBookingAsync(id, reason);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Cancellation failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Cancelled successfully", StatusCodes.Status200OK, "Booking cancelled"));
        }

        // Optional: Update booking status endpoint
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] string status)
        {
            var success = await _bookingService.UpdateBookingStatusAsync(id, status);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Status update failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Updated successfully", StatusCodes.Status200OK, "Booking status updated"));
        }
    }
}
