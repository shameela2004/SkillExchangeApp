using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Rating;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Services
{
    public class RatingService : IRatingService
    {
        private readonly IGenericRepository<Rating> _ratingRepo;
        private readonly IGenericRepository<Session> _sessionRepo;
        private readonly IGenericRepository<GroupSession> _groupSessionRepo;
        private readonly IGenericRepository<GroupMember> _groupMemberRepo;
        private readonly IGenericRepository<Booking> _bookingRepo;
        private readonly IMapper _mapper;

        public RatingService(IGenericRepository<Rating> ratingRepo, IMapper mapper, IGenericRepository<Session> sessionRepo, IGenericRepository<GroupSession> groupSessionRepo, IGenericRepository<GroupMember> groupMemberRepo, IGenericRepository<Booking> bookingRepo)
        {
            _ratingRepo = ratingRepo;
            _mapper = mapper;
            _sessionRepo = sessionRepo;
            _groupSessionRepo = groupSessionRepo;
            _groupMemberRepo = groupMemberRepo;
            _bookingRepo = bookingRepo;
        }

        //public async Task<bool> SubmitRatingAsync(CreateRatingDto dto, int ratedByUserId)
        //{
        //    int mentorId;
        //    if (dto.IsGroupSession)
        //    {
        //        var groupSession = await _groupSessionRepo.GetByIdAsync(dto.SessionId);
        //        if (groupSession == null) throw new Exception("Group session not found");
        //        mentorId = groupSession.Group.MentorId;
        //    }
        //    else
        //    {
        //        var session = await _sessionRepo.GetByIdAsync(dto.SessionId);
        //        if (session == null) throw new Exception("Session not found");
        //        mentorId = session.MentorId;
        //    }

        //    var rating = new Rating
        //    {
        //        SessionId = dto.SessionId,
        //        RatedToUserId = mentorId,
        //        RatedByUserId = ratedByUserId,
        //        SkillId = dto.SkillId,
        //        RatingValue = dto.RatingValue,
        //        Feedback = dto.Feedback
        //    };

        //    await _ratingRepo.AddAsync(rating);
        //    await _ratingRepo.SaveChangesAsync();
        //    return true;
        //}
        public async Task<bool> SubmitRatingAsync(CreateRatingDto dto, int userId)
        {
            try
            {
                int mentorId;
                int skillId;
                if (dto.IsGroupSession)
                {
                    var groupSession = await _groupSessionRepo.Table
                        .Include(gs => gs.Group)
                        .ThenInclude(g => g.Mentor) // Ensures Mentor is loaded
                        .FirstOrDefaultAsync(gs => gs.Id == dto.SessionId);

                    if (groupSession == null)
                        throw new Exception("Group session not found");

                    mentorId = groupSession.Group.MentorId;
                    skillId = groupSession.Group.SkillId;

                    var isMember = await _groupMemberRepo.Table.AnyAsync(
                        m => m.GroupId == groupSession.GroupId && m.UserId == userId && !m.IsDeleted);
                    if (!isMember)
                        throw new UnauthorizedAccessException("User did not attend the group session");
                }
                else
                {
                    var session = await _sessionRepo.Table
                        .Include(s => s.Mentor) // Ensures Mentor is loaded
                        .FirstOrDefaultAsync(s => s.Id == dto.SessionId);

                    if (session == null)
                        throw new Exception("Session not found");

                    mentorId = session.MentorId;
                    skillId = session.SkillId;

                    var hasBooking = await _bookingRepo.Table.AnyAsync(b =>
                        b.SessionId == session.Id && b.LearnerId == userId && !b.IsCancelled);
                    if (!hasBooking)
                        throw new UnauthorizedAccessException("User did not attend the session");
                }

                if (userId == mentorId)
                    throw new InvalidOperationException("Mentor cannot rate their own session");

                var rating = new Rating
                {
                    SessionId = dto.SessionId,
                    RatedToUserId = mentorId,
                    RatedByUserId = userId,
                    SkillId = skillId,
                    RatingValue = dto.RatingValue,
                    Feedback = dto.Feedback
                };

                await _ratingRepo.AddAsync(rating);
                await _ratingRepo.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log exception here with your logger as needed
                // Example: _logger.LogError(ex, "Error in SubmitRatingAsync");
                throw new Exception($"Error submitting rating: {ex.Message}", ex);
            }
        }

        public async Task<bool> UpdateRatingAsync(UpdateRatingDto dto, int userId)
        {
            var rating = await _ratingRepo.GetByIdAsync(dto.RatingId);
            if (rating == null || rating.IsDeleted)
                return false;

            // Only the user who created the rating can update it
            if (rating.RatedByUserId != userId)
                throw new UnauthorizedAccessException("User is not authorized to update this rating");

            rating.RatingValue = dto.RatingValue;
            rating.Feedback = dto.Feedback;

            await _ratingRepo.UpdateAsync(rating);
            return true;
        }




        public async Task<IEnumerable<RatingDto>> GetMentorRatingsAsync(int mentorId)
        {
            var ratings = await _ratingRepo.Table
                .Where(r => r.RatedToUserId == mentorId && !r.IsDeleted)
                .ToListAsync();
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<SkillRatingSummaryDto?> GetSkillRatingSummaryAsync(int mentorId, int skillId)
        {
            var ratings = await _ratingRepo.Table
                .Where(r => r.RatedToUserId == mentorId && r.SkillId == skillId && !r.IsDeleted)
                .ToListAsync();

            if (ratings.Count == 0)
                return null;

            var avgRating = ratings.Average(r => r.RatingValue);
            var totalRatings = ratings.Count;
            var points = (int)(avgRating * totalRatings); // Example, modify per your logic
            var badge = GetBadge(points);

            return new SkillRatingSummaryDto
            {
                SkillId = skillId,
                AvgRating = avgRating,
                TotalRatings = totalRatings,
                Points = points,
                Badge = badge
            };
        }

        public async Task<IEnumerable<LeaderboardEntryDto>> GetLeaderboardAsync(int? skillId)
        {
            IQueryable<Rating> query = _ratingRepo.Table.Where(r => !r.IsDeleted);

            if (skillId.HasValue)
                query = query.Where(r => r.SkillId == skillId.Value);

            var grouped = await query
                .GroupBy(r => r.RatedToUserId)
                .Select(g => new
                {
                    MentorId = g.Key,
                    AvgRating = g.Average(r => r.RatingValue),
                    TotalRatings = g.Count()
                })
                .ToListAsync();

            var leaderboard = grouped.Select(g => new LeaderboardEntryDto
            {
                MentorId = g.MentorId,
                Points = (int)(g.AvgRating * g.TotalRatings), // Customize points logic
                Badge = GetBadge((int)(g.AvgRating * g.TotalRatings))
            }).OrderByDescending(l => l.Points).ToList();

            return leaderboard;
        }

        private string GetBadge(int points)
        {
            if (points >= 1000)
                return "Platinum";
            if (points >= 500)
                return "Gold";
            if (points >= 100)
                return "Silver";
            return "Bronze";
        }


        public async Task<IEnumerable<RatingDto>> GetRatingsAsync(int? mentorId = null, int? skillId = null, int? ratingValue = null)
        {
            IQueryable<Rating> query = _ratingRepo.Table.Where(r => !r.IsDeleted);

            if (mentorId.HasValue)
                query = query.Where(r => r.RatedToUserId == mentorId.Value);
            if (skillId.HasValue)
                query = query.Where(r => r.SkillId == skillId.Value);
            if (ratingValue.HasValue)
                query = query.Where(r => r.RatingValue == ratingValue.Value);

            var ratings = await query.ToListAsync();
            return _mapper.Map<IEnumerable<RatingDto>>(ratings);
        }

        public async Task<bool> DeleteRatingAsync(int ratingId)
        {
            var rating = await _ratingRepo.GetByIdAsync(ratingId);
            if (rating == null || rating.IsDeleted)
                return false;

            rating.IsDeleted = true;
            await _ratingRepo.UpdateAsync(rating);
            return true;
        }

    }

}
