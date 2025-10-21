using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Rating : BaseEntity
    {
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
        public int RatedToUserId { get; set; } // Mentor
        public User RatedToUser { get; set; } = null!;
        public int RatedByUserId { get; set; } // Learner
        public User RatedByUser { get; set; } = null!;
        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;

        public int RatingValue { get; set; } // 1-5
        public string? Feedback { get; set; }
    }
}
