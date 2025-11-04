using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.DTOs.Rating;
using MyApp1.Application.Interfaces.Services;
using System.Security.Claims;

namespace MyApp1.API.Controllers.UserControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RatingContrtoller : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingContrtoller(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitRating([FromBody] CreateRatingDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");

            var success = await _ratingService.SubmitRatingAsync(dto, userId);
            if (!success)
                return BadRequest("Rating submission failed");

            return Ok(new { Success = true });
        }
        [HttpPut("{ratingId}")]
        public async Task<IActionResult> UpdateRating(int ratingId, [FromBody] UpdateRatingDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            // Ensure dto.RatingId matches ratingId for consistency
            if (dto.RatingId != ratingId)
                return BadRequest("Mismatched rating ID");

            var success = await _ratingService.UpdateRatingAsync(dto, userId);
            if (!success)
                return NotFound("Rating not found or deleted");

            return Ok(new { Success = true, Message = "Rating updated successfully" });
        }

        [HttpGet("mentor/{mentorId}")]
        public async Task<IActionResult> GetMentorRatings(int mentorId)
        {
            var ratings = await _ratingService.GetMentorRatingsAsync(mentorId);
            return Ok(ratings);
        }

        [HttpGet("skill/{mentorId}/{skillId}")]
        public async Task<IActionResult> GetSkillRatingSummary(int mentorId, int skillId)
        {
            var summary = await _ratingService.GetSkillRatingSummaryAsync(mentorId, skillId);
            if (summary == null)
                return NotFound();

            return Ok(summary);
        }
    }
}
