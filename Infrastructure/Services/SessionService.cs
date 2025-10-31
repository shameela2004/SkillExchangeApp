using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Session;
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
    //    public class SessionService :ISessionService
    //    {
    //        private readonly IGenericRepository<Session> _sessionRepository;
    //        public SessionService(IGenericRepository<Session> sessionRepository)
    //        {
    //            _sessionRepository = sessionRepository;
    //        }
    //        public async Task<IEnumerable<Session>> GetUserSessionsAsync(int userId, string role)
    //        {
    //            IQueryable<Session> query = _sessionRepository.Table
    //                .Include(s => s.Bookings)
    //                .Include(s => s.Mentor)
    //                .Include(s => s.Skill);

    //            if (role.Equals("Mentor", StringComparison.OrdinalIgnoreCase))
    //            {
    //                query = query.Where(s => s.MentorId == userId);
    //            }
    //            else if (role.Equals("Learner", StringComparison.OrdinalIgnoreCase))
    //            {
    //                query = query.Where(s => s.Bookings.Any(b => b.LearnerId == userId));
    //            }
    //            else
    //            {
    //                // Both roles
    //                query = query.Where(s => s.MentorId == userId || s.Bookings.Any(b => b.LearnerId == userId));
    //            }

    //            return await query.ToListAsync();
    //    }

    //}

    public class SessionService : ISessionService
    {
        private readonly IGenericRepository<Session> _sessionRepository;
        private readonly IGenericRepository<UserSkill> _userSkillRepository;

        public SessionService(IGenericRepository<Session> sessionRepository, IGenericRepository<UserSkill> userSkillRepository)
        {
            _sessionRepository = sessionRepository;
            _userSkillRepository = userSkillRepository;
        }

        public async Task<int> CreateSessionAsync(CreateSessionDto dto, int mentorId)
        {
            var teachingSkills = await _userSkillRepository.Table
                  .Where(us => us.UserId == mentorId && us.Type=="Teaching")
                  .Select(us => us.SkillId)
                  .ToListAsync();

            if (!teachingSkills.Contains(dto.SkillId))
                throw new UnauthorizedAccessException("Skill not allowed for this mentor.");
            var session = new Session
            {
                MentorId = mentorId,
                SkillId = dto.SkillId,
                ScheduledAt = dto.ScheduledAt,
                Mode = dto.Mode,
                Type = dto.Type,
                Notes = dto.Notes,
                Price = dto.Price ?? 0,
            };

            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();

            return session.Id;
        }

        public async Task<bool> UpdateSessionAsync(int sessionId, UpdateSessionDto dto,int userId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null) return false;
            if (session.MentorId != userId) return false;
            if (DateTime.UtcNow > session.ScheduledAt || session.IsCompleted)
                return false; // Cannot update after session time or after completion

            session.ScheduledAt = dto.ScheduledAt;
            session.Notes = dto.Notes;

            await _sessionRepository.UpdateAsync(session);
            return true;
        }

        public async Task<bool> DeleteSessionAsync(int sessionId,int userId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null) return false;
            if (session.MentorId != userId) return false;
            if (DateTime.UtcNow > session.ScheduledAt || session.IsCompleted)
                return false; // Cannot delete after session time or after completion

            _sessionRepository.Remove(session);
            await _sessionRepository.SaveChangesAsync();

            return true;
        }


        public async Task<Session?> GetSessionByIdAsync(int sessionId)
        {
            return await _sessionRepository.GetByIdAsync(sessionId);
        }

        public async Task<IEnumerable<Session>> GetSessionsForUserAsync(int userId, string role)
        {
            IQueryable<Session> query = _sessionRepository.Table;

            if (role.ToLower() == "mentor")
            {
                query = query.Where(s => s.MentorId == userId);
            }
            else if (role.ToLower() == "learner")
            {
                query = query.Where(s => s.Bookings.Any(b => b.LearnerId == userId && !b.IsCancelled));
            }

            return await query.ToListAsync();
        }

        public async Task<bool> MarkSessionCompletedAsync(int sessionId)
        {
            var session = await _sessionRepository.GetByIdAsync(sessionId);
            if (session == null) return false;

            // Assuming you add IsCompleted property on Session entity
            session.IsCompleted = true;

            await _sessionRepository.UpdateAsync(session);

            return true;
        }
    }



}
