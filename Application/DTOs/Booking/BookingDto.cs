using MyApp1.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Booking
{
    public class BookingDto
    {
        public int Id { get; set; }
        public int? SessionId { get; set; }
        public int? GroupSessionId { get; set; }
        public string LearnerName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public DateTime? SessionDate { get; set; }
        public string Skill { get; set; } = string.Empty;
        public string Mode { get; set; } = string.Empty;
        //public UserDto Mentor { get; set; } = new UserDto();
        public int  MentorId { get; set; }
        public string MentorProfilePictureUrl { get; set; }
        public string MentorName { get; set; }
        public decimal PaymentAmount { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
        public string? CancellationReason { get; set; }
    }
}
