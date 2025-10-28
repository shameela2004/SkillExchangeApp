using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class MentorProfile :BaseEntity
    {
        public int Id { get; set; }
        public int UserId { get; set; } // FK to User

        public string? AadhaarImageUrl { get; set; }
        public string? SocialProfileUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public List<MentorAvailability> Availabilities { get; set; }

        public User User { get; set; }
    }
}
