using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Session
{
    public class CreateSessionDto
    {
        public int SkillId { get; set; }
        public int MentorId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Mode { get; set; } = string.Empty; // Online / Offline
        public string Type { get; set; } = string.Empty; // Individual / Group
        public int? GroupId { get; set; }
        public string? Notes { get; set; }
        public decimal Price { get; set; }
        public bool IsPremiumAccessible { get; set; } = false;
        public string? VideoLink { get; set; }
    }

}
