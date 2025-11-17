using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.DTOs.MediaAsset;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

namespace MyApp1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MediaController : ControllerBase
    {
        private readonly IMediaService _mediaService;
        public MediaController(IMediaService mediaService)
        {
            _mediaService = mediaService;
        }
        [HttpPost("upload")]
        [Authorize]
        [Consumes("multipart/form-data")]

        public async Task<IActionResult> UploadMedia([FromForm] MediaUploadRequest request)
        {
            if (request.File == null || request.File.Length == 0)
                return BadRequest("No file uploaded");

            using var ms = new MemoryStream();
            await request.File.CopyToAsync(ms);

            var media = new MediaAsset
            {
                FileName = request.File.FileName,
                ContentType = request.File.ContentType,
                Data = ms.ToArray(),
                UploadedByUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                ReferenceType = request.ReferenceType,
                ReferenceId = request.ReferenceId
            };

            var mediaId = await _mediaService.UploadMediaAsync(media);
            return Ok(mediaId);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMedia(int id)
        {
            var media = await _mediaService.GetMediaAsync(id);
            if (media == null) return NotFound();

            return File(media.Data, media.ContentType,media.FileName);
        }
    }
}
