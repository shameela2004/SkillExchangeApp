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
        Task<string> UpdateProfilePictureAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUserDtosAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<UserDto> GetUserDtoByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(int userId,UpdateUserDto updatedUser);
        Task<IEnumerable<UserBadge>> GetUserBadgesAsync(int userId);
        Task<IEnumerable<User>> SearchUsersAsync(SearchUserDto filter);
    }
}
