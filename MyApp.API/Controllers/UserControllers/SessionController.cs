using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Session;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Infrastructure.Services;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
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

        [HttpPost]
        public async Task<IActionResult> CreateSession([FromBody] CreateSessionDto dto)
        {
            var id = await _sessionService.CreateSessionAsync(dto);
            return Ok(ApiResponse<int>.SuccessResponse(id, StatusCodes.Status201Created, "Session created"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSessionById(int id)
        {
            var session = await _sessionService.GetSessionByIdAsync(id);
            if (session == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Session not found"));

            var dto = _mapper.Map<SessionDto>(session);
            return Ok(ApiResponse<SessionDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Session details"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSession(int id, [FromBody] UpdateSessionDto dto)
        {
            var success = await _sessionService.UpdateSessionAsync(id, dto);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Update failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Updated successfully", StatusCodes.Status200OK, "Session updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSession(int id)
        {
            var success = await _sessionService.DeleteSessionAsync(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Delete failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Deleted", StatusCodes.Status200OK, "Session deleted"));
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserSessions(int userId, [FromQuery] string role)
        {
            var sessions = await _sessionService.GetUserSessionsAsync(userId, role);
            var result = _mapper.Map<IEnumerable<SessionDto>>(sessions);
            return Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(result, StatusCodes.Status200OK, "User sessions fetched"));
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> MarkSessionCompleted(int id)
        {
            var success = await _sessionService.MarkSessionCompletedAsync(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Mark complete failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Session marked completed", StatusCodes.Status200OK, "Marked completed"));
        }


    }
}
