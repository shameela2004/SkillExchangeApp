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

        [HttpGet]
        public async Task<IActionResult> GetAllSessions()
        {
            var sessions = await _sessionService.GetSessionsForUserAsync(0, "all"); 
            var dto = _mapper.Map<IEnumerable<SessionDto>>(sessions);
            return Ok(ApiResponse<IEnumerable<SessionDto>>.SuccessResponse(dto, StatusCodes.Status200OK, "All sessions fetched"));
        }
    }

}
