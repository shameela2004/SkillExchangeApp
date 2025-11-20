using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Mentor
{
    public class MentorDto
    {
        public int UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string MentorProfilePictureUrl { get; set; }
        public string MentorStatus { get; set; } = string.Empty; // Pending, Approved, Rejected
        public string PhoneNumber { get; set; } = string.Empty;
        public string AadhaarImageUrl { get; set; } = string.Empty;
        public string SocialProfileUrl { get; set; } = string.Empty;
        public List<MentorAvailabilityDto> Availabilities { get; set; } = new List<MentorAvailabilityDto>();
    }
}
