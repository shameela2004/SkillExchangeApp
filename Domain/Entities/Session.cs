using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Session : BaseEntity
    {
        public int MentorId { get; set; }
        public User Mentor { get; set; } = null!;

        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;

        public int? GroupId { get; set; }
        public Group? Group { get; set; }

        public string Type { get; set; } = string.Empty; // Individual / Group
        public DateTime ScheduledAt { get; set; }
        public string Mode { get; set; } = string.Empty; // Online / Offline
        public string? Notes { get; set; }
        public decimal Price { get; set; }
        public bool IsPremiumAccessible { get; set; } = false;
        public string? VideoLink { get; set; }

        // Navigation
        public ICollection<Booking>? Bookings { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<Message>? Messages { get; set; }
    }
}
