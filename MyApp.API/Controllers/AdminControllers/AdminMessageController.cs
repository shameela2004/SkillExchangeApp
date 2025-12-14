using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Message;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/admin/messages")]
    [ApiController]
    public class AdminMessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public AdminMessageController(IMessageService messageService, IMapper mapper)
        {
            _messageService = messageService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetConversation([FromQuery] int user1Id, [FromQuery] int user2Id)
        {
            var messages = await _messageService.GetMessagesAsync(user1Id, user2Id);
            var dto = _mapper.Map<IEnumerable<MessageDto>>(messages);
            return Ok(ApiResponse<IEnumerable<MessageDto>>.SuccessResponse(
                dto, StatusCodes.Status200OK, "Conversation fetched"));
        }

    }

}
