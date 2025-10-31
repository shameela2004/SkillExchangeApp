using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Connection;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Infrastructure.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConnectionController : ControllerBase
    {
        private readonly IConnectionService _connectionService;
        private readonly IMapper _mapper;
        public ConnectionController(IConnectionService connectionService , IMapper mapper)
        {
            _connectionService = connectionService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpPost("connect/{otherUserId}")]
        public async Task<IActionResult> SendConnectionRequest(int otherUserId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _connectionService.SendConnectionRequestAsync(userId, otherUserId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to send connection request"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Connection request sent"));
        }

        [Authorize]
        [HttpPost("connect/accept/{connectionId}")]
        public async Task<IActionResult> AcceptConnection(int connectionId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _connectionService.AcceptConnectionAsync(connectionId, userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to accept connection"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Connection accepted"));
        }

        [Authorize]
        [HttpPost("connect/reject/{connectionId}")]
        public async Task<IActionResult> RejectConnection(int connectionId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _connectionService.RejectConnectionAsync(connectionId, userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to reject connection"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Connection rejected"));
        }

        [Authorize]
        [HttpDelete("connect/{connectionId}")]
        public async Task<IActionResult> DeleteConnection(int connectionId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _connectionService.DeleteConnectionAsync(connectionId, userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to delete connection"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Connection deleted"));
        }

        [Authorize]
        [HttpGet("connections")]
        public async Task<IActionResult> GetConnections()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var connections = await _connectionService.GetUserConnectionsAsync(userId);
            return Ok(ApiResponse<IEnumerable<ConnectionDto>>.SuccessResponse(connections, StatusCodes.Status200OK, "Connections fetched"));
        }

        [Authorize]
        [HttpGet("connections/pending")]
        public async Task<IActionResult> GetPendingConnections()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var pendingConnections = await _connectionService.GetPendingConnectionsAsync(userId);

            return Ok(ApiResponse<IEnumerable<ConnectionDto>>.SuccessResponse(pendingConnections, StatusCodes.Status200OK, "Pending connections fetched"));
        }


    }
}
