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
    public class ConnectionService :IConnectionService
    {
        private readonly IGenericRepository<Connection> _connectionRepository;
        public ConnectionService(IGenericRepository<Connection> connectionRepository)
        {
            _connectionRepository = connectionRepository;
        }
        public async Task<bool> SendConnectionRequestAsync(int fromUserId, int toUserId)
        {
            if (fromUserId == toUserId) return false; // Cannot connect to self

            // Check if request or connection already exists
            var existingConnection = await _connectionRepository.Table
                .FirstOrDefaultAsync(c =>
                    (c.UserId == fromUserId && c.ConnectedUserId == toUserId) ||
                    (c.UserId == toUserId && c.ConnectedUserId == fromUserId));

            if (existingConnection != null)
                return false;

            var connection = new Connection
            {
                UserId = fromUserId,
                ConnectedUserId = toUserId,
                Status = "Pending"
            };

            await _connectionRepository.AddAsync(connection);
            await _connectionRepository.SaveChangesAsync();
            return true;
        }
         
        public async Task<bool> AcceptConnectionAsync(int connectionId)
        {
            var connection = await _connectionRepository.GetByIdAsync(connectionId);
            if (connection == null || connection.Status == "Accepted")
                return false;

            connection.Status = "Accepted";
            await _connectionRepository.UpdateAsync(connection);

            return true;
        }
        public async Task<IEnumerable<Connection>> GetUserConnectionsAsync(int userId)
        {
            return await _connectionRepository.Table
                .Include(c => c.ConnectedUser)
                .Where(c => c.UserId == userId && c.Status == "Accepted")
                .ToListAsync();
        }


    }
}
