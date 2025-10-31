using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Session;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Infrastructure.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/sessions")]
    [ApiController]
    public class SessionController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IMapper _mapper;

        public SessionController(ISessionService sessionService, IMapper mapper)
        {
            _sessionService = sessionService;
            _mapper = mapper;
        }
        [Authorize(Roles = "Mentor")]
        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionDto dto)
        {
            int mentorId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var sessionId = await _sessionService.CreateSessionAsync(dto, mentorId);
            return Ok(ApiResponse<int>.SuccessResponse(sessionId, StatusCodes.Status201Created, "Session created"));
        }
        [Authorize(Roles = "Mentor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] UpdateSessionDto dto)
        {
            var userId= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _sessionService.UpdateSessionAsync(id, dto,userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to update session"));

            return Ok(ApiResponse<string>.SuccessResponse("Session updated successfully", StatusCodes.Status200OK, "Updated"));
        }
        [Authorize(Roles = "Mentor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var userId= int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _sessionService.DeleteSessionAsync(id, userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to delete session"));

            return Ok(ApiResponse<string>.SuccessResponse("Session deleted successfully", StatusCodes.Status200OK, "Deleted"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSession(int id)
        {
            var session = await _sessionService.GetSessionByIdAsync(id);
            if (session == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Session not found"));

            var dto = _mapper.Map<SessionDto>(session);
            return Ok(ApiResponse<SessionDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Session info retrieved"));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserSessions(int userId, [FromQuery] string role)
        {
            var sessions = await _sessionService.GetSessionsForUserAsync(userId, role);
            var dto = _mapper.Map<IEnumerable<SessionDto>>(sessions);
            return Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(dto, StatusCodes.Status200OK, "User sessions fetched"));
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMySessions()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            var sessions = await _sessionService.GetSessionsForUserAsync(userId, role);
            var dto = _mapper.Map<IEnumerable<SessionDto>>(sessions);

            return Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(dto, StatusCodes.Status200OK, "Sessions fetched"));
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> CompleteSession(int id)
        {
            var success = await _sessionService.MarkSessionCompletedAsync(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to mark session completed"));

            return Ok(ApiResponse<string>.SuccessResponse("Session marked completed", StatusCodes.Status200OK, "Completed"));
        }
    }

}
