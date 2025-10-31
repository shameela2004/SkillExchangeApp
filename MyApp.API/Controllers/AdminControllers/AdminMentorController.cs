using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Mentor;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMentorController : ControllerBase
    {
        private readonly IMentorService _mentorService;

        public AdminMentorController(IMentorService mentorService)
        {
            _mentorService = mentorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMentors([FromQuery] string? status)
        {
            var mentors = await _mentorService.GetAllMentorsAsync(status);
            return Ok(ApiResponse<IEnumerable<MentorDto>>.SuccessResponse(mentors, 200, "Mentors fetched"));
        }
        //[HttpGet("pending")]
        //public async Task<IActionResult> GetPendingMentorApplications()
        //{
        //    var pendingMentors = await _mentorService.GetPendingMentorApplicationsAsync();
        //    return Ok(ApiResponse<IEnumerable<MentorDto>>.SuccessResponse(pendingMentors, 200, "Pending mentor applications fetched"));
        //}

        [HttpPost("{userId}/approve")]
        public async Task<IActionResult> ApproveMentor(int userId)
        {
            var success = await _mentorService.ApproveMentorAsync(userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to approve mentor"));
            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Mentor approved"));
        }

        [HttpPost("{userId}/reject")]
        public async Task<IActionResult> RejectMentor(int userId)
        {
            var success = await _mentorService.RejectMentorAsync(userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to reject mentor"));
            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Mentor rejected"));
        }
    }
}
