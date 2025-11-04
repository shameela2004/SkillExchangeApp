using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.UserReport
{
    public class UserReportDto
    {
        public int Id { get; set; }
        public int ReportedUserId { get; set; }
        public string ReportedUserName { get; set; } = string.Empty;
        public int ReporterUserId { get; set; }
        public string ReporterUserName { get; set; } = string.Empty;
        public string Reason { get; set; }
        public string Status { get; set; } = string.Empty;
        public string AdminNote { get; set; } = string.Empty;
        public DateTime? ReviewedAt { get; set; }
    }
}
