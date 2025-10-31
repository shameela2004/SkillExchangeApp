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

        public GroupService(IGenericRepository<Group> groupRepo,
                            IGenericRepository<GroupMember> groupMemberRepo,
                            IGenericRepository<GroupMessage> groupMessageRepo)
        {
            _groupRepo = groupRepo;
            _groupMemberRepo = groupMemberRepo;
            _groupMessageRepo = groupMessageRepo;
        }

        public async Task<int> CreateGroupAsync(CreateGroupDto dto)
        {
            var group = new Group
            {
                Name = dto.Name,
                SkillId = dto.SkillId,
                MentorId = dto.MentorId,
                MaxMembers = dto.MaxMembers
            };

            await _groupRepo.AddAsync(group);
            await _groupRepo.SaveChangesAsync();
            return group.Id;
        }

        public async Task<bool> DeleteGroupAsync(int groupId, int userId)
        {
            var isAdmin = false;
            var admin = _userRepo.Table.FirstOrDefault(u => u.Id == userId && u.Role == "Admin");
            if (admin != null)
            {
                 isAdmin = true;
            }
            if (!await IsUserGroupAdmin(groupId, userId)|| isAdmin)
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
            if (group == null)
                return false;

            group.Name = dto.Name;
            group.SkillId = dto.SkillId;
            group.MaxMembers = dto.MaxMembers;

            await _groupRepo.UpdateAsync(group);
            await _groupRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddMemberAsync(GroupMemberDto memberDto, int requesterUserId)
        {
            if (!await IsUserGroupAdmin(memberDto.GroupId, requesterUserId))
                return false; // not authorized to add member

            var existing = await _groupMemberRepo.Table.FirstOrDefaultAsync(m =>
                m.GroupId == memberDto.GroupId && m.UserId == memberDto.UserId);
            if (existing != null)
                return false; // already a member

            var member = new GroupMember
            {
                GroupId = memberDto.GroupId,
                UserId = memberDto.UserId,
                Role = GroupRole.Member
            };

            await _groupMemberRepo.AddAsync(member);
            await _groupMemberRepo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveMemberAsync(GroupMemberDto memberDto, int requesterUserId)
        {
            if (!await IsUserGroupAdmin(memberDto.GroupId, requesterUserId))
                return false; // not authorized to remove member

            var member = await _groupMemberRepo.Table.FirstOrDefaultAsync(m =>
                m.GroupId == memberDto.GroupId && m.UserId == memberDto.UserId);
            if (member == null)
                return false;

            _groupMemberRepo.Remove(member);
            await _groupMemberRepo.SaveChangesAsync();
            return true;
        }


        public async Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int groupId)
        {
            return await _groupMemberRepo.Table.Where(m => m.GroupId == groupId).ToListAsync();
        }

        public async Task<int> SendMessageAsync(SendGroupMessageDto dto)
        {
            var message = new GroupMessage
            {
                GroupId = dto.GroupId,
                FromUserId = dto.FromUserId,
                Content = dto.Content,
                FilePath = dto.FilePath
            };

            await _groupMessageRepo.AddAsync(message);
            await _groupMessageRepo.SaveChangesAsync();
            return message.Id;
        }

        public async Task<IEnumerable<GroupMessage>> GetMessagesAsync(int groupId)
        {
            return await _groupMessageRepo.Table.Where(m => m.GroupId == groupId).ToListAsync();
        }
        public async Task<bool> IsUserGroupAdmin(int groupId, int userId)
        {
            var member = await _groupMemberRepo.Table
                .FirstOrDefaultAsync(m => m.GroupId == groupId && m.UserId == userId && m.Role == GroupRole.Admin);
            return member != null;
        }
    }

}
