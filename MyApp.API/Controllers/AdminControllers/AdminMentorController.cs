//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using MyApp1.Application.Common;
//using MyApp1.Application.DTOs.User;

//namespace MyApp1.API.Controllers.AdminControllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class AdminMentorController : ControllerBase
//    {
//        private readonly IAdminService _adminService;
//        private readonly IMapper _mapper;

//        public AdminMentorController(IAdminService adminService, IMapper mapper)
//        {
//            _adminService = adminService;
//            _mapper = mapper;
//        }

//        [HttpGet("pending")]
//        public async Task<IActionResult> GetPendingMentorApplications()
//        {
//            var mentors = await _adminService.GetPendingMentorApplicationsAsync();
//            var mentorDtos = _mapper.Map<IEnumerable<MentorApplicationDto>>(mentors);
//            return Ok(ApiResponse<IEnumerable<MentorApplicationDto>>.SuccessResponse(mentorDtos, StatusCodes.Status200OK, "Pending mentors fetched"));
//        }

//        [HttpPost("{id}/approve")]
//        public async Task<IActionResult> ApproveMentor(int id)
//        {
//            var success = await _adminService.ApproveMentorAsync(id);
//            if (!success)
//                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to approve mentor"));
//            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Mentor approved"));
//        }

//        //[HttpPost("{id}/reject")]
//        //public async Task<IActionResult> RejectMentor(int id, [FromBody] RejectMentorDto rejectDto)
//        //{
//        //    var success = await _adminService.RejectMentorAsync(id, rejectDto.Reason);
//        //    if (!success)
//        //        return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to reject mentor"));
//        //    return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Mentor rejected"));
//        //}
//    }
//}
