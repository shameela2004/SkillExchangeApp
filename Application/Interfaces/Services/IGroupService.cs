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
        Task<IEnumerable<Group>> GetMyGroups(int userId);
        Task<int> CreateGroupAsync(CreateGroupDto dto, int mentorId);
        Task<Group?> GetGroupByIdAsync(int groupId);
        Task<IEnumerable<Group>> GetGroupsByMentorAsync(int mentorId);
        Task<bool> UpdateGroupAsync(int groupId, CreateGroupDto dto, int userId);
        Task<bool> DeleteGroupAsync(int groupId, int userId);

        Task<bool> AddMemberAsync(int groupId, int newMemberUserId, int requesterUserId);
        Task<bool> RemoveMemberAsync(int groupId, int memberUserId, int requesterUserId);

        Task<IEnumerable<GroupMemberDto>> GetGroupMembersAsync(int groupId);
        Task<int> SendMessageAsync(int groupId, int fromUserId, SendGroupMessageDto dto);
        Task<IEnumerable<GroupMessage>> GetMessagesAsync(int groupId);
        Task<bool> IsUserGroupAdmin(int groupId, int userId);
    }

}
