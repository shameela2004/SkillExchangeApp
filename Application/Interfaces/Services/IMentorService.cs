using MyApp1.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IMentorService
    {
        Task<bool> ApplyMentorAsync(int userId, MentorApplicationDto dto);
        Task<string> GetMentorApplicationStatusAsync(int userId);
    }
}
