using MyApp1.Application.DTOs.Connection;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IConnectionService
    {
        Task<bool> SendConnectionRequestAsync(int fromUserId, int toUserId);
        Task<bool> AcceptConnectionAsync(int connectionId, int actingUserId);
        Task<bool> RejectConnectionAsync(int connectionId, int actingUserId);
        Task<bool> DeleteConnectionAsync(int connectionId, int actingUserId);
        Task<IEnumerable<ConnectionDto>> GetUserConnectionsAsync(int userId);
        Task<IEnumerable<ConnectionDto>> GetPendingConnectionsAsync(int userId);

    }
}
