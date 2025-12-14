using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Dashboard;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminDashboardController : ControllerBase
    {
        private readonly IAdminDashboardService _dashboardService;
        public AdminDashboardController(IAdminDashboardService dashboardService) { 
            _dashboardService = dashboardService;
        }
        [HttpGet("summary")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetSummary()
        {
            var summary = await _dashboardService.GetDashboardSummaryAsync();
            return Ok(ApiResponse<AdminDashboardSummaryDto>.SuccessResponse(
                summary, StatusCodes.Status200OK, "Summary fetched"));
        }
    }
}
