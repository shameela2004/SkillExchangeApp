using MyApp1.Application.DTOs.User;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<bool> ApplyMentorAsync(int userId, MentorApplicationDto dto);
        Task<string> GetMentorApplicationStatusAsync(int userId);
        Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId);
        Task<IEnumerable<User>> SearchUsersAsync(SearchUserDto filter);
    }
}
