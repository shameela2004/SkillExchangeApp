using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MyApp1.Application.DTOs.UserReport;
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
    public class UserReportService : IUserReportService
    {
        private readonly IGenericRepository<UserReport> _userReportRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IMapper _mapper;

        public UserReportService(IGenericRepository<UserReport> userReportRepo, IGenericRepository<User> userRepo, IMapper mapper)
        {
            _userReportRepo = userReportRepo;
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<bool> CreateReportAsync(CreateUserReportDto dto, int reporterUserId)
        {
            var reportedUser = await _userRepo.GetByIdAsync(dto.ReportedUserId);
            var reporterUser = await _userRepo.GetByIdAsync(reporterUserId);

            if (reportedUser == null || reporterUser == null)
                throw new Exception("User not found");

            var report = new UserReport
            {
                ReportedUserId = dto.ReportedUserId,
                ReporterUserId = reporterUserId,
                Reason = dto.Reason,
                Status = "Pending",
                ReviewedAt = null
            };

            await _userReportRepo.AddAsync(report);
            await _userReportRepo.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<UserReportDto>> GetUserReportsAsync(int userId)
        {
            var reports = await _userReportRepo.Table
                 .Where(r => r.ReporterUserId == userId)
                 .Include(r => r.ReportedUser)
                 .Include(r => r.ReporterUser)
                 .ToListAsync();

            return _mapper.Map<IEnumerable<UserReportDto>>(reports);
        }

        public async Task<IEnumerable<UserReportDto>> GetAllReportsAsync()
        {
            var reports = await _userReportRepo.Table
                .Include(r => r.ReportedUser)
                .Include(r => r.ReporterUser)
                .ToListAsync();

            return _mapper.Map<IEnumerable<UserReportDto>>(reports);
        }

        public async Task<bool> UpdateReportStatusAsync(int reportId, UpdateUserReportDto dto)
        {
            var report = await _userReportRepo.GetByIdAsync(reportId);
            if (report == null)
                return false;

            report.Status = dto.Status;
            report.AdminNote = dto.AdminNote;
            report.ReviewedAt = DateTime.UtcNow;

            await _userReportRepo.UpdateAsync(report);
            return true;
        }

        public async Task<UserReportDto?> GetReportByIdAsync(int reportId)
        {
            var report = await _userReportRepo.Table
                .Include(r => r.ReportedUser)
                .Include(r => r.ReporterUser)
                .FirstOrDefaultAsync(r => r.Id == reportId);

            if (report == null)
                return null;

            return _mapper.Map<UserReportDto>(report);
        }
    }

}
