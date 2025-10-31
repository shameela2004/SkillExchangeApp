using MyApp1.Application.DTOs.Mentor;
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
        Task<List<MentorAvailabilityDto>> GetAvailabilitiesAsync(int userId);
        Task<bool> AddAvailabilitiesAsync(int userId, IList<MentorAvailabilityDto> availabilities);
        Task<bool> UpdateAvailabilityAsync(int userId, int availabilityId, MentorAvailabilityDto dto);
        Task<bool> DeleteAvailabilityAsync(int userId, int availabilityId);

        Task<IEnumerable<MentorDto>> GetAllMentorsAsync(string? statusFilter = null);
        //Task<IEnumerable<MentorDto>> GetPendingMentorApplicationsAsync();
        Task<bool> ApproveMentorAsync(int userId);
        Task<bool> RejectMentorAsync(int userId);
        Task<bool> IsMentorApprovedAsync(int userId);

    }
}
