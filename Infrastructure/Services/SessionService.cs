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
        private readonly IGenericRepository<Skill> _skillRepository;

        public SessionService(IGenericRepository<Session> sessionRepository, IGenericRepository<Skill> skillRepository)
        {
            _sessionRepository = sessionRepository;
            _skillRepository = skillRepository;
        }

        public async Task<int> CreateSessionAsync(CreateSessionDto dto)
        {
            var skill = await _skillRepository.GetByIdAsync(dto.SkillId);
            if (skill == null) throw new Exception("Skill not found");

            var session = new Session
            {
                SkillId = dto.SkillId,
                MentorId = dto.MentorId,
                ScheduledAt = dto.ScheduledAt,
                Mode = dto.Mode,
                Type = dto.Type,
                GroupId = dto.GroupId,
                Notes = dto.Notes,
                Price = dto.Price,
                IsPremiumAccessible = dto.IsPremiumAccessible,
                VideoLink = dto.VideoLink,
                IsCompleted = false
            };

            await _sessionRepository.AddAsync(session);
            await _sessionRepository.SaveChangesAsync();
            return session.Id;
        }

        public async Task<Session?> GetSessionByIdAsync(int id)
        {
            return await _sessionRepository.GetByIdAsync(id);
        }

        public async Task<bool> UpdateSessionAsync(int id, UpdateSessionDto dto)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return false;

            session.ScheduledAt = dto.ScheduledAt;
            session.Notes = dto.Notes;
            if (dto.Price.HasValue)
                session.Price = dto.Price.Value;

            await _sessionRepository.UpdateAsync(session);
            return true;
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return false;

            _sessionRepository.Remove(session);
            await _sessionRepository.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Session>> GetUserSessionsAsync(int userId, string role)
        {
            IQueryable<Session> query = _sessionRepository.Table;

            if (role.ToLower() == "mentor")
                query = query.Where(s => s.MentorId == userId);
            else
                query = query.Where(s => s.Bookings.Any(b => b.LearnerId == userId && !b.IsCancelled));

            return await query.ToListAsync();
        }

        public async Task<bool> MarkSessionCompletedAsync(int id)
        {
            var session = await _sessionRepository.GetByIdAsync(id);
            if (session == null) return false;

            session.IsCompleted = true;
            await _sessionRepository.UpdateAsync(session);
            return true;
        }

        public async Task<IEnumerable<Session>> GetAllSessionsAsync()
        {
            return await _sessionRepository.GetAllAsync();
        }
    }


}
