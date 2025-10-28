using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.User
{
    public class UserBadgeDto
    {
        public string BadgeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? SkillName { get; set; }
        public DateTime? EarnedAt { get; set; }
    }

}
