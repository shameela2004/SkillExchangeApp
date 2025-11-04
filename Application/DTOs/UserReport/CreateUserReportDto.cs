using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.UserReport
{
    public class CreateUserReportDto
    {
        public int ReportedUserId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}
