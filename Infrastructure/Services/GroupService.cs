using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Group;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Enums;
using MyApp1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Services
{
    public class GroupService : IGroupService
    {
        private readonly IGenericRepository<Group> _groupRepo;
        private readonly IGenericRepository<GroupMember> _groupMemberRepo;
        private readonly IGenericRepository<GroupMessage> _groupMessageRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;

        public GroupService(IGenericRepository<Group> groupRepo,
                            IGenericRepository<GroupMember> groupMemberRepo,
                            IGenericRepository<GroupMessage> groupMessageRepo,
                            IMapper mapper)
        {
            _groupRepo = groupRepo;
            _groupMemberRepo = groupMemberRepo;
            _groupMessageRepo = groupMessageRepo;
            _mapper = mapper;
        }

        public async Task<int> CreateGroupAsync(CreateGroupDto dto, int mentorId)
        {
            var group = new Group
            {
                Name = dto.Name,
                SkillId = dto.SkillId,
                MentorId = mentorId,
                MaxMembers = dto.MaxMembers
            };

            await _groupRepo.AddAsync(group);
            await _groupRepo.SaveChangesAsync();
            // Add creator as admin member
            var adminMember = new GroupMember
            {
                GroupId = group.Id,
                UserId = mentorId,
                Role = "Admin"
            };

            await _groupMemberRepo.AddAsync(adminMember);
            await _groupMemberRepo.SaveChangesAsync();
            return group.Id;
        }


        public async Task<bool> DeleteGroupAsync(int groupId, int userId)
        {
            //var isAdmin = false;
            //var admin = _userRepo.Table.FirstOrDefault(u => u.Id == userId && u.Role == "Admin");
            //if (admin != null)
            //{
            //     isAdmin = true;
            //}
            if (!await IsUserGroupAdmin(groupId, userId))
                return false;
            var group = await _groupRepo.GetByIdAsync(groupId);
            if (group == null) return false;

            _groupRepo.Remove(group);
            await _groupRepo.SaveChangesAsync();
            return true;
        }

        public async Task<Group?> GetGroupByIdAsync(int groupId)
        {
            return await _groupRepo.GetByIdAsync(groupId);
        }

        public async Task<IEnumerable<Group>> GetGroupsByMentorAsync(int mentorId)
        {
            return await _groupRepo.Table.Where(g => g.MentorId == mentorId).ToListAsync();
        }

        public async Task<bool> UpdateGroupAsync(int groupId, CreateGroupDto dto, int userId)
        {
            if (!await IsUserGroupAdmin(groupId, userId))
                return false; // not authorized to update

            var group = await _groupRepo.GetByIdAsync(groupId);
            if (group == null || group.IsDeleted==true)
                return false;

            group.Name = dto.Name;
            group.SkillId = dto.SkillId;
            group.MaxMembers = dto.MaxMembers;

            await _groupRepo.UpdateAsync(group);
            await _groupRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddMemberAsync(int groupId, int newMemberUserId, int requesterUserId)
        {
            var group = await _groupRepo.GetByIdAsync(groupId);
            if (group == null || group.IsDeleted == true)
                return false;
            if (!await IsUserGroupAdmin(groupId, requesterUserId))
                return false; // not authorized

            var existingMember = await _groupMemberRepo.Table.FirstOrDefaultAsync(m =>
                m.GroupId == groupId && m.UserId == newMemberUserId && !m.IsDeleted);
            if (existingMember != null)
                return false; // already a member

            var member = new GroupMember
            {
                GroupId = groupId,
                UserId = newMemberUserId,
                Role = "Member"
            };

            await _groupMemberRepo.AddAsync(member);
            await _groupMemberRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMemberAsync(int groupId, int memberUserId, int requesterUserId)
        {
            var group = await _groupRepo.GetByIdAsync(groupId);
            if (group == null || group.IsDeleted)
                return false;
            if (!await IsUserGroupAdmin(groupId, requesterUserId))
                return false; // not authorized

            var member = await _groupMemberRepo.Table.FirstOrDefaultAsync(m =>
                m.GroupId == groupId && m.UserId == memberUserId && !m.IsDeleted);
            if (member == null)
                return false;

            _groupMemberRepo.Remove(member);
            await _groupMemberRepo.SaveChangesAsync();
            return true;
        }



        public async Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(int groupId)
        {

            var members = await _groupMemberRepo.Table
                .Include(m => m.User)
                .Where(m => m.GroupId == groupId && !m.IsDeleted)
                .ToListAsync();

            return _mapper.Map<IEnumerable<GroupMemberDto>>(members);
        }


        public async Task<int> SendMessageAsync(int groupId, int fromUserId, SendGroupMessageDto dto)
{
            var group = await _groupRepo.Table.FirstOrDefaultAsync(g => g.Id == groupId && !g.IsDeleted);
            if (group == null)
                throw new InvalidOperationException("Group is deleted or does not exist.");
            var message = new GroupMessage
    {
        GroupId = groupId,
        FromUserId = fromUserId,
        Content = dto.Content,
        FilePath = dto.FilePath
    };

    await _groupMessageRepo.AddAsync(message);
    await _groupMessageRepo.SaveChangesAsync();

    return message.Id;
}


        public async Task<IEnumerable<GroupMessage>> GetMessagesAsync(int groupId)
        {
            return await _groupMessageRepo.Table
                .Include(m => m.FromUser)
                .Include(m => m.Group)
                .Where(m => m.GroupId == groupId).ToListAsync();
        }
        public async Task<bool> IsUserGroupAdmin(int groupId, int userId)
        {
            var member = await _groupMemberRepo.Table
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == userId && m.Role == "Admin");
            return member != null;
        }
    }

}
