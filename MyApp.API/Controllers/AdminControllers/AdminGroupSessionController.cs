using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.GroupSession;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminGroupSessionController : ControllerBase
    {
        private readonly IGroupSessionService _groupSessionService;
        private readonly IGenericService<GroupSession> _genericGroupSessionService;
        private readonly IMapper _mapper;

        public AdminGroupSessionController(IGroupSessionService groupSessionService, IGenericService<GroupSession> genericGroupSessionService, IMapper mapper)
        {
            _groupSessionService = groupSessionService;
            _genericGroupSessionService = genericGroupSessionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllGroupSessions()
        {
            var sessions = await _genericGroupSessionService.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<GroupSessionDto>>(sessions);
            return Ok(ApiResponse<IEnumerable<GroupSessionDto>>.SuccessResponse(dtos, StatusCodes.Status200OK, "Group sessions fetched"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetGroupSessionById(int id)
        {
            var session = await _groupSessionService.GetGroupSessionByIdAsync(id);
            if (session == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Group session not found"));
            var dto = _mapper.Map<GroupSessionDto>(session);
            return Ok(ApiResponse<GroupSessionDto>.SuccessResponse(dto, StatusCodes.Status200OK, "Group session details fetched"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroupSession(int id)
        {
            var success = await _groupSessionService.DeleteGroupSessionAsync(id);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Delete failed"));
            return Ok(ApiResponse<string>.SuccessResponse("Deleted successfully", StatusCodes.Status200OK, "Group session deleted"));
        }
    }
}
