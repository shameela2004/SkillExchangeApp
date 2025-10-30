using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Common;
using MyApp1.Application.DTOs.Post;
using MyApp1.Application.Exceptions;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly IPostService _postService;
        private readonly IGenericService<PostComment> _commentService;
        private readonly IMapper _mapper;

        public PostController(IPostService postService, IMapper mapper,IGenericService<PostComment> commentService)
        {
            _postService = postService;
            _mapper = mapper;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] string sortBy = "CreatedAt",
    [FromQuery] bool descending = true)
        {
            var posts = await _postService.GetPostsAsync(page, pageSize, sortBy, descending);
            var postsDto = _mapper.Map<IEnumerable<PostDto>>(posts);
            return Ok(ApiResponse<IEnumerable<PostDto>>.SuccessResponse(postsDto, StatusCodes.Status200OK, "Posts fetched successfully"));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDto createDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var result = await _postService.CreatePostAsync(userId, createDto);
            if (!result)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to create post"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status201Created, "Post created successfully"));
        }
        [HttpPut("{postId:int}")]
        public async Task<IActionResult> EditPost(int postId, [FromBody] EditPostDto editDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var post = await _postService.GetPostByIdAsync(postId);
            if (post == null)
                return NotFound(ApiResponse<string>.FailResponse(404, "Post not found"));

            if (post.UserId != userId) // Permission check
                return Forbid();

            var success = await _postService.EditPostAsync(postId, userId, editDto);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to edit post"));

            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Post edited successfully"));
        }
        [HttpDelete("{postId:int}")]
        [Authorize]
        public async Task<IActionResult> DeletePost(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var post = await _postService.GetPostByIdAsync(postId);

            if (post == null)
                return NotFound(ApiResponse<string>.FailResponse(404, "Post not found"));

            if (post.UserId != userId)
                return Forbid();

            var success = await _postService.DeletePostAsync(postId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(400, "Failed to delete post"));

            return Ok(ApiResponse<string>.SuccessResponse(null, 200, "Post deleted successfully"));
        }

        // Post Comments

        [HttpGet("{postId}/comments")]
        public async Task<IActionResult> GetCommentsForPost(
       int postId,
       [FromQuery] int page = 1,
       [FromQuery] int pageSize = 10,
       [FromQuery] string sortBy = "CreatedAt",
       [FromQuery] bool descending = true)
        {
            var comments = await _postService.GetCommentsByPostIdAsync(postId, page, pageSize, sortBy, descending);
            var commentsDto = _mapper.Map<IEnumerable<PostCommentDto>>(comments);
            return Ok(ApiResponse<IEnumerable<PostCommentDto>>.SuccessResponse(commentsDto, StatusCodes.Status200OK, "Comments fetched"));
        }

        [HttpPost("{postId}/comment")]
        public async Task<IActionResult> AddComment(int postId, [FromBody] AddCommentDto commentDto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _postService.AddCommentAsync(postId, userId, commentDto.CommentText);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to add comment"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status201Created, "Comment added successfully"));
        }
        [HttpPut("{id:int}")]
        public async Task<IActionResult> EditComment(int id, [FromBody] EditCommentDto editDto)
        {
            var comment = await _commentService.GetByIdAsync(id);
            if (comment == null)
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, "Comment not found"));

            comment.Comment = editDto.CommentText;

            await _commentService.UpdateAsync(comment);

            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Comment updated successfully"));
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            try
            {
                await _commentService.DeleteAsync(id);
                return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Comment deleted successfully"));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiResponse<string>.FailResponse(StatusCodes.Status404NotFound, ex.Message));
            }
        }

        // Post Likes
        [HttpPost("{postId}/toggle-like")]
        public async Task<IActionResult> ToggleLikePost(int postId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var success = await _postService.ToggleLikePostAsync(postId, userId);
            if (!success)
                return BadRequest(ApiResponse<string>.FailResponse(StatusCodes.Status400BadRequest, "Failed to toggle like"));
            return Ok(ApiResponse<string>.SuccessResponse(null, StatusCodes.Status200OK, "Like toggled successfully"));
        }

    }
}
