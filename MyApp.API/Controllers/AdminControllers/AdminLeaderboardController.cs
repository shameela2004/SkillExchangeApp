using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApp1.Application.Interfaces.Services;

namespace MyApp1.API.Controllers.AdminControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminLeaderboardController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public AdminLeaderboardController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetLeaderboard([FromQuery] int? skillId)
        {
            var leaderboard = await _ratingService.GetLeaderboardAsync(skillId);
            return Ok(leaderboard);
        }
    }
}
