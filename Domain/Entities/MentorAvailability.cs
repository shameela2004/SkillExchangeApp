using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class MentorAvailability :BaseEntity
    {
        public int Id { get; set; }
        public int MentorProfileId { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public MentorProfile MentorProfile { get; set; }
    }

}
