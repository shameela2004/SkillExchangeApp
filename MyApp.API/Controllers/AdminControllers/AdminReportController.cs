using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.DTOs.UserReport;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/admin/reports")]
    [ApiController]
    public class AdminReportController : ControllerBase
    {
        private readonly IUserReportService _userReportService;

        public AdminReportController(IUserReportService userReportService)
        {
            _userReportService = userReportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReports()
        {
            var reports = await _userReportService.GetAllReportsAsync();
            return Ok(reports);
        }

        [HttpPut("{reportId}")]
        public async Task<IActionResult> UpdateReportStatus(int reportId, [FromBody] UpdateUserReportDto dto)
        {
            var success = await _userReportService.UpdateReportStatusAsync(reportId, dto);
            if (!success) return NotFound();
            return Ok(new { Success = true, Message = "Report updated successfully" });
        }
    }

}
