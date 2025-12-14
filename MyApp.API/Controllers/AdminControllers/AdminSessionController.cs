using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Session;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/admin/sessions")]
    [ApiController]
    public class AdminSessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IMapper _mapper;

        public AdminSessionController(ISessionService sessionService, IMapper mapper)
        {
            _sessionService = sessionService;
            _mapper = mapper;
        }

        // GET: api/admin/sessions
        [HttpGet]
        public async Task<IActionResult> GetAllSessions(
            [FromQuery] int? mentorId,
            [FromQuery] bool? isCompleted)
        {
            // For admin, reuse GetSessionsForUserAsync with special "all" role, or create a new service method
            var sessions = await _sessionService.GetAllSessionsForAdminAsync(mentorId, isCompleted);
            var dto = _mapper.Map<IEnumerable<SessionDto>>(sessions);

            return Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(
                dto, StatusCodes.Status200OK, "All sessions fetched"));
        }

        // GET: api/admin/sessions/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            var session = await _sessionService.GetSessionByIdAsync(id);
            if (session == null)
                return NotFound(ApiResponse<string>.FailResponse(
                    StatusCodes.Status404NotFound, "Session not found"));

            var dto = _mapper.Map<SessionDto>(session);
            return Ok(ApiResponse<SessionDto>.SuccessResponse(
                dto, StatusCodes.Status200OK, "Session details fetched"));
        }

        // POST: api/admin/sessions/{id}/complete
        [HttpPost("{id}/complete")]
        public async Task<IActionResult> MarkCompleted(int id)
        {
            var ok = await _sessionService.MarkSessionCompletedAsync(id);
            if (!ok)
                return BadRequest(ApiResponse<string>.FailResponse(
                    StatusCodes.Status400BadRequest, "Failed to mark session completed"));

            return Ok(ApiResponse<string>.SuccessResponse(
                "Completed", StatusCodes.Status200OK, "Session marked completed"));
        }

        // DELETE: api/admin/sessions/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByAdmin(int id)
        {
            var ok = await _sessionService.DeleteSessionByAdminAsync(id);
            if (!ok)
                return BadRequest(ApiResponse<string>.FailResponse(
                    StatusCodes.Status400BadRequest, "Delete failed"));

            return Ok(ApiResponse<string>.SuccessResponse(
                "Deleted", StatusCodes.Status200OK, "Session deleted by admin"));
        }
    }

}
