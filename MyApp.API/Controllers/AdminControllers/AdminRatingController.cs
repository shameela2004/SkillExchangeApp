using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public AdminRatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRatings([FromQuery] int? mentorId, [FromQuery] int? skillId, [FromQuery] int? ratingValue)
        {
            var ratings = await _ratingService.GetRatingsAsync(mentorId, skillId, ratingValue);
            return Ok(ratings);
        }

        [HttpDelete("{ratingId}")]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            var success = await _ratingService.DeleteRatingAsync(ratingId);
            if (!success) return NotFound("Rating not found or already deleted");
            return Ok(new { Success = true, Message = "Rating deleted successfully" });
        }
    }
}
