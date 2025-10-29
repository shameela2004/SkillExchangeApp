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
        //[HttpGet("{id:int}/sessions")]
        //public async Task<IActionResult> GetUserSessions(int id, [FromQuery] string role)
        //{
        //    var sessions = await _sessionService.GetUserSessionsAsync(id, role);
        //    var sessionsDto = _mapper.Map<IEnumerable<SessionDto>>(sessions);
        //    return Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(sessionsDto, StatusCodes.Status200OK, "Sessions fetched successfully"));
        //}
        [HttpGet("{id:int}/sessions")]
        public async Task<IActionResult> GetUserSessions(int id, [FromQuery] string role)
        {
            var sessions = await _sessionService.GetUserSessionsAsync(id, role);

            // Pass userId in AutoMapper context so Role can be resolved properly
            var sessionsDto = _mapper.Map<IEnumerable<SessionDto>>(sessions, opts =>
            {
                opts.Items["UserId"] = id;
            });

            return Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(sessionsDto, StatusCodes.Status200OK, "Sessions fetched successfully"));
        }


    }
}
