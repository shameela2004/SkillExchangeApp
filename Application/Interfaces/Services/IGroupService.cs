using MyApp1.Application.DTOs.Group;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IGroupService
    {
        Task<int> CreateGroupAsync(CreateGroupDto dto);
        Task<Group?> GetGroupByIdAsync(int groupId);
        Task<IEnumerable<Group>> GetGroupsByMentorAsync(int mentorId);
        Task<bool> UpdateGroupAsync(int groupId, CreateGroupDto dto, int userId);
        Task<bool> DeleteGroupAsync(int groupId, int userId);

        Task<bool> AddMemberAsync(GroupMemberDto memberDto, int requesterUserId);
        Task<bool> RemoveMemberAsync(GroupMemberDto memberDto, int requesterUserId);
        Task<IEnumerable<GroupMember>> GetGroupMembersAsync(int groupId);

        Task<int> SendMessageAsync(SendGroupMessageDto dto);
        Task<IEnumerable<GroupMessage>> GetMessagesAsync(int groupId);
        Task<bool> IsUserGroupAdmin(int groupId, int userId);
    }

}
