using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Notification;
using MyApp1.Application.Interfaces.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        private readonly IMapper _mapper;
        public NotificationController(INotificationService notificationService, IMapper mapper)
        {
            _notificationService = notificationService;
            _mapper = mapper;
        }
        [Authorize]
        [HttpGet("notifications")]
        public async Task<IActionResult> GetNotifications()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var notifications = await _notificationService.GetNotificationsAsync(userId);
            var notificationsDto = _mapper.Map<IEnumerable<NotificationDto>>(notifications);
            return Ok(ApiResponse<IEnumerable<NotificationDto>>.SuccessResponse(notificationsDto, StatusCodes.Status200OK, "Notifications fetched"));
        }

    }
}
