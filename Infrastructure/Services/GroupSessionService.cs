using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.GroupSession;
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
    public class GroupSessionService : IGroupSessionService
    {
        private readonly IGenericRepository<GroupSession> _groupSessionRepo;

        public GroupSessionService(IGenericRepository<GroupSession> groupSessionRepo)
        {
            _groupSessionRepo = groupSessionRepo;
        }

        public async Task<int> CreateGroupSessionAsync(CreateGroupSessionDto dto)
        {
            var entity = new GroupSession
            {
                GroupId = dto.GroupId,
                ScheduledAt = dto.ScheduledAt,
                Mode = dto.Mode,
                Notes = dto.Notes
            };

            await _groupSessionRepo.AddAsync(entity);
            await _groupSessionRepo.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> DeleteGroupSessionAsync(int id)
        {
            var entity = await _groupSessionRepo.GetByIdAsync(id);
            if (entity == null) return false;

            _groupSessionRepo.Remove(entity);
            await _groupSessionRepo.SaveChangesAsync();
            return true;
        }

        public async Task<GroupSession?> GetGroupSessionByIdAsync(int id)
        {
            return await _groupSessionRepo.GetByIdAsync(id);
        }

        public async Task<IEnumerable<GroupSession>> GetGroupSessionsByGroupIdAsync(int groupId)
        {
            return await _groupSessionRepo.Table.Where(gs => gs.GroupId == groupId).ToListAsync();
        }
    }

}
