using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.DTOs.UserReport;
using MyApp1.Application.Interfaces.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/reports")]
    [ApiController]
    public class UserReportController : ControllerBase
    {
        private readonly IUserReportService _userReportService;

        public UserReportController(IUserReportService userReportService)
        {
            _userReportService = userReportService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateReport([FromBody] CreateUserReportDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _userReportService.CreateReportAsync(dto, userId);
            if (!success) return BadRequest("Report creation failed");
            return Ok(new { Success = true });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserReports()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var reports = await _userReportService.GetUserReportsAsync(userId);
            return Ok(reports);
        }

        [HttpGet("{reportId}")]
        public async Task<IActionResult> GetReportDetail(int reportId)
        {
            var report = await _userReportService.GetReportByIdAsync(reportId);
            if (report == null) return NotFound();
            return Ok(report);
        }
    }

}
