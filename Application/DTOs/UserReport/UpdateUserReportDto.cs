using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.UserReport
{
    public class UpdateUserReportDto
    {
        public string Status { get; set; } = string.Empty; // "Reviewed", "ActionTaken", etc.
        public string AdminNote { get; set; } = string.Empty;
    }
}
