using MyApp1.Application.DTOs.Session;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface ISessionService
    {
        Task<int> CreateSessionAsync(CreateSessionDto dto, int mentorId);
        Task<bool> UpdateSessionAsync(int sessionId, UpdateSessionDto dto,int userId);
        Task<bool> DeleteSessionAsync(int sessionId, int userId);
        Task<Session?> GetSessionByIdAsync(int sessionId);
        Task<IEnumerable<Session>> GetSessionsForUserAsync(int userId, string role);
        Task<IEnumerable<Session>> GetSessionsForMentorAsync(int userId);
        Task<bool> MarkSessionCompletedAsync(int sessionId);
    }


}
