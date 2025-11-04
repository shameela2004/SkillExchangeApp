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
        private readonly IGenericRepository<Group> _groupRepo;
        private IGenericRepository<User> _userRepo;

        public GroupSessionService(IGenericRepository<GroupSession> groupSessionRepo, IGenericRepository<Group> groupRepo, IGenericRepository<User> userRepo)
        {
            _groupSessionRepo = groupSessionRepo;
            _groupRepo = groupRepo;
            _userRepo = userRepo;
        }

        public async Task<int> CreateGroupSessionAsync(CreateGroupSessionDto dto,int userId)
        {
            if (!await IsUserMentorOfGroupAsync(dto.GroupId, userId))
                throw new UnauthorizedAccessException("User is not mentor of the group");
            var entity = new GroupSession
            {
                GroupId = dto.GroupId,
                ScheduledAt = dto.ScheduledAt,
                Mode = dto.Mode,
                Notes = dto.Notes,
                Price = dto.Price,
                VideoLink = null,
                IsCompleted = false
            };

            await _groupSessionRepo.AddAsync(entity);
            await _groupSessionRepo.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<bool> UpdateGroupSessionAsync(int id, UpdateGroupSessionDto dto,int userId)
        {
            var entity = await _groupSessionRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                return false;
            if (!await IsUserMentorOfGroupAsync(entity.GroupId, userId))
                throw new UnauthorizedAccessException("User is not mentor of the group");
            entity.ScheduledAt = dto.ScheduledAt;
            entity.Mode = dto.Mode;
            entity.Notes = dto.Notes;
            entity.Price = dto.Price;

            await _groupSessionRepo.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteGroupSessionAsync(int id,int userId)
        {
            var entity = await _groupSessionRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted || entity.IsCompleted)
                return false;
            if (!await IsUserMentorOfGroupAsync(entity.GroupId, userId))
                throw new UnauthorizedAccessException("User is not mentor of the group");
            _groupSessionRepo.Remove(entity);
            await _groupSessionRepo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteSessionByAdminAsync(int id, int userId)
        {
            var adminUser = await _userRepo.GetByIdAsync(userId);
            if (adminUser == null || adminUser.IsDeleted || adminUser.Role != "Admin")
                throw new UnauthorizedAccessException("User is not an admin");
            var entity = await _groupSessionRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted || entity.IsCompleted)
                return false;

            _groupSessionRepo.Remove(entity);
            await _groupSessionRepo.SaveChangesAsync();
            return true;
        }

        public async Task<GroupSession?> GetGroupSessionByIdAsync(int id)
        {
            var entity = await _groupSessionRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                return null;
            return entity;
        }

        public async Task<IEnumerable<GroupSession>> GetGroupSessionsByGroupIdAsync(int groupId)
        {
            return await _groupSessionRepo.Table
                .Where(gs => gs.GroupId == groupId && !gs.IsDeleted)
                .ToListAsync();
        }

        public async Task<bool> MarkSessionCompletedAsync(int id,int userId)
        {
            var entity = await _groupSessionRepo.GetByIdAsync(id);
            if (entity == null || entity.IsDeleted)
                return false;
            if (!await IsUserMentorOfGroupAsync(entity.GroupId, userId))
                throw new UnauthorizedAccessException("User is not mentor of the group");
            entity.IsCompleted = true;
            await _groupSessionRepo.UpdateAsync(entity);
            return true;
        }

        public async Task<string?> GenerateVideoLinkAsync(int sessionId)
        {
            // Example fake implementation, replace by actual call
            var link = $"https://video-provider.com/meeting/{sessionId}-{Guid.NewGuid()}";

            var session = await _groupSessionRepo.GetByIdAsync(sessionId);
            if (session == null || session.IsDeleted)
                return null;

            session.VideoLink = link;
            await _groupSessionRepo.UpdateAsync(session);
            return link;
        }
        private async Task<bool> IsUserMentorOfGroupAsync(int groupId, int userId)
        {
            var group = await _groupRepo.GetByIdAsync(groupId);
            if (group == null || group.IsDeleted)
                return false;
            return group.MentorId == userId;
        }
       
        //    public async Task<bool> CanUserAccessSessionAsync(int sessionId, int userId)
        //{
        //    var session = await _groupSessionRepo.GetByIdAsync(sessionId);
        //    if (session == null || session.IsDeleted) return false;

        //    if (session.Price == null || session.Price == 0)
        //    {
        //        // Free session: verify group membership
        //        return await _groupMemberRepo.Table.AnyAsync(m => m.GroupId == session.GroupId && m.UserId == userId && !m.IsDeleted);
        //    }
        //    else
        //    {
        //        // Paid session: verify booking and payment
        //        return await _bookingRepo.Table.AnyAsync(b => b.SessionId == sessionId && b.LearnerId == userId && b.IsPaid && !b.IsCancelled);
        //    }
        //}


    }


}
