using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.GroupSession;
using MyApp1.Application.Interfaces.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupSessionController : ControllerBase
    {
        private readonly IGroupSessionService _groupSessionService;
        private readonly IMapper _mapper;

        public GroupSessionController(IGroupSessionService groupSessionService, IMapper mapper)
        {
            _groupSessionService = groupSessionService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGroupSession([FromBody] CreateGroupSessionDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var id = await _groupSessionService.CreateGroupSessionAsync(dto, userId);

            // Optionally generate video link on creation
            await _groupSessionService.GenerateVideoLinkAsync(id);

            return Ok(ApiResponse<int>.SuccessResponse(id, StatusCodes.Status201Created, "Group session created"));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateGroupSession( int id,[FromBody] UpdateGroupSessionDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _groupSessionService.UpdateGroupSessionAsync(id, dto,userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Update failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Updated", StatusCodes.Status200OK, "Group session updated"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupSession(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _groupSessionService.DeleteGroupSessionAsync(id, userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Delete failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Deleted", StatusCodes.Status200OK, "Group session deleted"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupSession(int id)
        {
            var entity = await _groupSessionService.GetGroupSessionByIdAsync(id);
            if (entity == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Session not found"));

            var dto = _mapper.Map<GroupSessionDto>(entity);
            return Ok(ApiResponse<GroupSessionDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Session details"));
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetSessionsByGroup(int groupId)
        {
            var entities = await _groupSessionService.GetGroupSessionsByGroupIdAsync(groupId);
            var dtos = _mapper.Map<IEnumerable<GroupSessionDto>>(entities);
            return Ok(ApiResponse<IEnumerable<GroupSessionDto>>.SuccessResponse(dtos, StatusCodes.Status200OK, "Sessions fetched"));
        }

        [HttpPost("{id}/complete")]
        public async Task<IActionResult> MarkSessionCompleted(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _groupSessionService.MarkSessionCompletedAsync(id,userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Mark completed failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Completed", StatusCodes.Status200OK, "Group session marked completed"));
        }

    }
}
