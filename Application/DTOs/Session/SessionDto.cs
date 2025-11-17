using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Session
{
    //public class SessionDto
    //{
    //    public int SessionId { get; set; }
    //    public string Title { get; set; } = string.Empty;
    //    public DateTime StartTime { get; set; }
    //    public DateTime EndTime { get; set; }
    //    public string Role { get; set; }     // e.g., "Mentor" / "Learner"
    //    public string Status { get; set; }   // e.g., "Completed", "Upcoming", "Cancelled"
    //}



    public class SessionDto
    {
        public int Id { get; set; }
        public string Mentor { get; set; } = string.Empty;
        public string Skill { get; set; } = string.Empty;
        public DateTime ScheduledAt { get; set; }
        public string Mode { get; set; } = string.Empty;
        public int? MaxParticipants { get; set; }
        public string? Notes { get; set; }
        public decimal? Price { get; set; }
        public bool IsCompleted { get; set; }
    }

}
