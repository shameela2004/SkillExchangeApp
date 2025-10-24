using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class UserReport
    {
        public int Id { get; set; }
        public int ReportedUserId { get; set; }
        public User ReportedUser { get; set; }
        public int ReporterUserId { get; set; }
        public User ReporterUser { get; set; }
        public string Reason { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Reviewed, ActionTaken
        public string AdminNote { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReviewedAt { get; set; }
    }
}
