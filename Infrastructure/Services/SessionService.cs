using Microsoft.EntityFrameworkCore;
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
    public class SessionService :ISessionService
    {
        private readonly IGenericRepository<Session> _sessionRepository;
        public SessionService(IGenericRepository<Session> sessionRepository)
        {
            _sessionRepository = sessionRepository;
        }
        public async Task<IEnumerable<Session>> GetUserSessionsAsync(int userId, string role)
        {
            IQueryable<Session> query = _sessionRepository.Table
                .Include(s => s.Bookings)
                .Include(s => s.Mentor)
                .Include(s => s.Skill);

            if (role.Equals("Mentor", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(s => s.MentorId == userId);
            }
            else if (role.Equals("Learner", StringComparison.OrdinalIgnoreCase))
            {
                query = query.Where(s => s.Bookings.Any(b => b.LearnerId == userId));
            }
            else
            {
                // Both roles
                query = query.Where(s => s.MentorId == userId || s.Bookings.Any(b => b.LearnerId == userId));
            }

            return await query.ToListAsync();
    }

}
}
