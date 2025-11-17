using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.API.DTOs.Voice;
using MyApp1.Application.DTOs.Message;
using MyApp1.Application.Interfaces.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public MessageController(IMessageService messageService, IMapper mapper, IWebHostEnvironment env)
        {
            _messageService = messageService;
            _mapper = mapper;
            _env = env;
        }

        [HttpGet("{user2Id}")]
        public async Task<IActionResult> GetMessages(int user2Id)
        {
            var user1Id = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var messages = await _messageService.GetMessagesAsync(user1Id, user2Id);
            return Ok(messages); // Map to dto if you want
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] MessageCreateDto dto)
        {
            var fromUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var result = await _messageService.CreateMessageAsync(fromUserId, dto);
            if (!result)
                return BadRequest("Failed to save message");
            return Ok();
        }
        [HttpPost("upload-voice")]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> UploadVoice([FromForm] UploadVoiceDto dto)
        {
            if (dto.VoiceFile == null)
                return BadRequest("Voice file missing");

            var fileName = $"{Guid.NewGuid()}.webm";
            var savePath = Path.Combine(_env.WebRootPath, "voices", fileName);

            using (var stream = new FileStream(savePath, FileMode.Create))
            {
                await dto.VoiceFile.CopyToAsync(stream);
            }

            return Ok(new { filePath = $"/voices/{fileName}" });
        }

    }

}
