using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Post;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminPostController : ControllerBase
    {
        private readonly IPostService _postService;

        public AdminPostController(IPostService postService)
        {
            _postService = postService;
        }

        // GET /api/AdminPost
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? userId)
        {
            var posts = await _postService.GetAllForAdminAsync(userId);
            return Ok(ApiResponse<IEnumerable<PostDto>>.SuccessResponse(
                posts, StatusCodes.Status200OK, "Posts fetched"));
        }

        // POST /api/AdminPost/{postId}/hide
        [HttpPost("{postId}/hide")]
        public async Task<IActionResult> Hide(int postId)
        {
            var ok = await _postService.HidePostAsync(postId);
            if (!ok)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to hide post"));

            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Post hidden"));
        }

        // POST /api/AdminPost/{postId}/unhide
        [HttpPost("{postId}/unhide")]
        public async Task<IActionResult> Unhide(int postId)
        {
            var ok = await _postService.UnhidePostAsync(postId);
            if (!ok)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to unhide post"));

            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Post unhidden"));
        }
    }

}
