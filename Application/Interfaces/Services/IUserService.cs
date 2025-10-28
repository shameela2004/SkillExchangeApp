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
        //Task<User?> GetUserByIdAsync(int id);
        Task<bool> UpdateUserAsync(User user);
        Task<IEnumerable<User>> SearchUsersAsync(string? skill, string? location);
        Task<bool> ApplyMentorAsync(MentorProfile mentorProfile);
        Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId);
    }
}
