using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Payment;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public PaymentController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpPost("verify")]
        public async Task<IActionResult> VerifyPayment([FromBody] VerifyPaymentDto dto)
        {
            if (dto == null)
                return BadRequest(ApiResponse<string>.FailResponse(400,"Invalid request"));

            var isVerified = await _bookingService.VerifyPaymentAsync(dto);

            if (!isVerified)
                return BadRequest(ApiResponse<string>.FailResponse(400,"Payment verification failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Payment verified successfully", 200));
        }
    }
}
