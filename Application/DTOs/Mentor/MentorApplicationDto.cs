using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Mentor
{
    public class MentorApplicationDto
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string? AadhaarImageUrl { get; set; }
        public string? SocialProfileUrl { get; set; }
    }
}
