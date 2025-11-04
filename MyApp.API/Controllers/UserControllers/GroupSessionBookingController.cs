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
    public class GroupSessionBookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly IMapper _mapper;

        public GroupSessionBookingController(IBookingService bookingService, IMapper mapper)
        {
            _bookingService = bookingService;
            _mapper = mapper;
        }
        [HttpPost("group-session")]
        [Authorize(Roles = "Learner")]
        public async Task<IActionResult> BookGroupSession([FromBody] BookSessionDto dto)
        {
            var learnerId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var bookingId = await _bookingService.BookGroupSessionAsync(dto, learnerId);
            return Ok(ApiResponse<int>.SuccessResponse(bookingId, StatusCodes.Status201Created, "Group session booked"));
        }
    }
}
