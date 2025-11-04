using MyApp1.Application.DTOs.UserReport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IUserReportService
    {
        Task<bool> CreateReportAsync(CreateUserReportDto dto, int reporterUserId);
        Task<IEnumerable<UserReportDto>> GetUserReportsAsync(int userId); // for given user
        Task<IEnumerable<UserReportDto>> GetAllReportsAsync(); // admin view all reports
        Task<bool> UpdateReportStatusAsync(int reportId, UpdateUserReportDto dto);
        Task<UserReportDto?> GetReportByIdAsync(int reportId);
    }

}
