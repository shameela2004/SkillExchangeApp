using MyApp1.Application.DTOs.GroupSession;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IGroupSessionService
    {
        Task<int> CreateGroupSessionAsync(CreateGroupSessionDto dto,int userId);
        Task<bool> UpdateGroupSessionAsync(int id, UpdateGroupSessionDto dto, int userId);
        Task<bool> DeleteGroupSessionAsync(int id, int userId);
        Task<bool> DeleteSessionByAdminAsync(int id, int userId);
        Task<GroupSession?> GetGroupSessionByIdAsync(int id);
        Task<IEnumerable<GroupSession>> GetGroupSessionsByGroupIdAsync(int groupId);
        Task<bool> MarkSessionCompletedAsync(int id, int userId);
        Task<string?> GenerateVideoLinkAsync(int sessionId);
    }

}
