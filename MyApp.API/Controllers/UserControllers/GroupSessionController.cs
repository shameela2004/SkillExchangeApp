using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.GroupSession;
using MyApp1.Application.Interfaces.Services;

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
            var id = await _groupSessionService.CreateGroupSessionAsync(dto);
            return Ok(ApiResponse<int>.SuccessResponse(id, StatusCodes.Status201Created, "Group session created"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupSession(int id)
        {
            var entity = await _groupSessionService.GetGroupSessionByIdAsync(id);
            if (entity == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Group session not found"));

            var dto = _mapper.Map<GroupSessionDto>(entity);
            return Ok(ApiResponse<GroupSessionDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Group session details"));
        }

        [HttpGet("group/{groupId}")]
        public async Task<IActionResult> GetSessionsByGroup(int groupId)
        {
            var entities = await _groupSessionService.GetGroupSessionsByGroupIdAsync(groupId);
            var dtos = _mapper.Map<IEnumerable<GroupSessionDto>>(entities);
            return Ok(ApiResponse<IEnumerable<GroupSessionDto>>.SuccessResponse(dtos, StatusCodes.Status200OK, "Group sessions fetched"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupSession(int id)
        {
            var success = await _groupSessionService.DeleteGroupSessionAsync(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Deletion failed"));

            return Ok(ApiResponse<string>.SuccessResponse("Deleted", StatusCodes.Status200OK, "Group session deleted"));
        }
    }
}
