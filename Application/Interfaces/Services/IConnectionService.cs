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
        Task<bool> AcceptConnectionAsync(int connectionId);
        Task<IEnumerable<Connection>> GetUserConnectionsAsync(int userId);

    }
}
