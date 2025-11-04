using MyApp1.Application.DTOs.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IRatingService
    {
        Task<bool> SubmitRatingAsync(CreateRatingDto dto, int userId);
        Task<bool> UpdateRatingAsync(UpdateRatingDto dto, int userId);
        Task<IEnumerable<RatingDto>> GetMentorRatingsAsync(int mentorId);
        Task<SkillRatingSummaryDto?> GetSkillRatingSummaryAsync(int mentorId, int skillId);
        Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardAsync(int? skillId);
        Task<IEnumerable<RatingDto>> GetRatingsAsync(int? mentorId = null, int? skillId = null, int? ratingValue = null);
        Task<bool> DeleteRatingAsync(int ratingId);

    }

}
