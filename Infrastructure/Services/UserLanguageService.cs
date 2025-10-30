using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.Language;
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
    public class UserLanguageService : IUserLanguageService
    {
        private readonly IGenericRepository<UserLanguage> _userLanguageRepository;
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Language> _languageRepository;

        public UserLanguageService(
            IGenericRepository<UserLanguage> userLanguageRepo,
            IGenericRepository<User> userRepo,
            IGenericRepository<Language> languageRepo)
        {
            _userLanguageRepository = userLanguageRepo;
            _userRepository = userRepo;
            _languageRepository = languageRepo;
        }

        public async Task<IEnumerable<UserLanguage>> GetUserLanguagesAsync(int userId)
        {
            return await _userLanguageRepository.Table
                .Where(ul => ul.UserId == userId && !ul.IsDeleted)
                .Include(ul => ul.Language)
                .ToListAsync();
        }

        public async Task<bool> AddUserLanguageAsync(int userId, AddUserLanguageDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null) return false;

            var language = await _languageRepository.GetByIdAsync(dto.LanguageId);
            if (language == null) return false;

            var exists = await _userLanguageRepository.Table
                .AnyAsync(ul => ul.UserId == userId && ul.LanguageId == dto.LanguageId);

            if (exists) return false;

            var userLang = new UserLanguage
            {
                UserId = userId,
                LanguageId = dto.LanguageId,
                Proficiency = dto.Proficiency
            };

            await _userLanguageRepository.AddAsync(userLang);
            await _userLanguageRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveUserLanguageAsync(int userId, int languageId)
        {
            var userLang = await _userLanguageRepository.Table
                .FirstOrDefaultAsync(ul => ul.UserId == userId && ul.LanguageId == languageId);
            if (userLang == null) return false;

            _userLanguageRepository.Remove(userLang);
            await _userLanguageRepository.SaveChangesAsync();
            return true;
        }
    }

}
