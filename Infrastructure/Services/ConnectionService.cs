using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Connection;
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
    public class ConnectionService : IConnectionService
    {
        private readonly IGenericRepository<Connection> _connectionRepository;
        private readonly IMapper _mapper;
        public ConnectionService(IGenericRepository<Connection> connectionRepository, IMapper mapper)
        {
            _connectionRepository = connectionRepository;
            _mapper = mapper;
        }

        public async Task<bool> SendConnectionRequestAsync(int fromUserId, int toUserId)
        {
            if (fromUserId == toUserId) return false; // Cannot connect to self

            var existingConnection = await _connectionRepository.Table
     .FirstOrDefaultAsync(c =>
         ((c.UserId == fromUserId && c.ConnectedUserId == toUserId) ||
          (c.UserId == toUserId && c.ConnectedUserId == fromUserId)) &&
         (c.Status !="Rejected") &&
         !c.IsDeleted);


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

        public async Task<bool> AcceptConnectionAsync(int connectionId, int actingUserId)
        {
            var connection = await _connectionRepository.GetByIdAsync(connectionId);
            if (connection == null) return false;

            // Only the connected user (receiver) can accept
            if (connection.ConnectedUserId != actingUserId) return false;

            if (connection.Status == "Accepted") return false; // already accepted
            if (connection.Status == "Rejected") return false; // cannot accept rejected

            connection.Status = "Accepted";
            await _connectionRepository.UpdateAsync(connection);
            await _connectionRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RejectConnectionAsync(int connectionId, int actingUserId)
        {
            var connection = await _connectionRepository.GetByIdAsync(connectionId);
            if (connection == null) return false;

            // Only the connected user (receiver) can reject
            if (connection.ConnectedUserId != actingUserId) return false;

            if (connection.Status == "Accepted") return false; // cannot reject accepted
            if (connection.Status == "Rejected") return false; // already rejected

            // Reject means remove connection
            _connectionRepository.Remove(connection);
            await _connectionRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteConnectionAsync(int connectionId, int actingUserId)
        {
            var connection = await _connectionRepository.GetByIdAsync(connectionId);
            if (connection == null) return false;

            if (connection.UserId != actingUserId && connection.ConnectedUserId != actingUserId)
                return false;

            connection.IsDeleted = true;
            connection.DeletedAt = DateTime.UtcNow;

            await _connectionRepository.UpdateAsync(connection);
            return true;
        }



        public async Task<IEnumerable<ConnectionDto>> GetUserConnectionsAsync(int userId)
        {
            var connections = await _connectionRepository.Table
                .Include(c => c.User)
                .Include(c => c.ConnectedUser)
                .Where(c => (c.UserId == userId || c.ConnectedUserId == userId)
                            && c.Status == "Accepted"
                            && !c.IsDeleted)
                .ToListAsync();

            var connectionDtos = connections.Select(c => new ConnectionDto
            {
                ConnectionId = c.Id,
                UserId = (c.UserId == userId) ? c.ConnectedUserId : c.UserId,
                UserName = (c.UserId == userId) ? c.ConnectedUser.Name : c.User.Name,
                Status = c.Status,
                CreatedAt = c.CreatedAt
            }).ToList();

            return connectionDtos;
        }
        public async Task<IEnumerable<ConnectionDto>> GetPendingConnectionsAsync(int userId)
        {
            var connections = await _connectionRepository.Table
                .Include(c => c.User)
                .Include(c => c.ConnectedUser)
                .Where(c => c.ConnectedUserId == userId   // pending requests sent *to* this user
                            && c.Status == "Pending"
                            && !c.IsDeleted)
                .ToListAsync();

            var connectionDtos = connections.Select(c => new ConnectionDto
            {
                ConnectionId = c.Id,
                UserId = c.UserId,
                UserName = c.User.Name,
                Status = c.Status,
                CreatedAt = c.CreatedAt
            }).ToList();

            return connectionDtos;
        }

    }

}
