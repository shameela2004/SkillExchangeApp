using MyApp1.Application.DTOs.Language;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IUserLanguageService
    {
        Task<IEnumerable<UserLanguage>> GetUserLanguagesAsync(int userId);
        Task<bool> AddUserLanguageAsync(int userId, AddUserLanguageDto dto);
        Task<bool> RemoveUserLanguageAsync(int userId, int languageId);
    }

}
