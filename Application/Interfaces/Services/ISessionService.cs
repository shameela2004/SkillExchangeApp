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
        Task<int> CreateSessionAsync(CreateSessionDto dto);
        Task<Session?> GetSessionByIdAsync(int id);
        Task<bool> UpdateSessionAsync(int id, UpdateSessionDto dto);
        Task<bool> DeleteSessionAsync(int id);
        Task<IEnumerable<Session>> GetUserSessionsAsync(int userId, string role);
        Task<bool> MarkSessionCompletedAsync(int id);
        Task<IEnumerable<Session>> GetAllSessionsAsync();
    }

}
