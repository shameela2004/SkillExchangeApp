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
        public int SessionId { get; set; }
        public int LearnerId { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal PaymentAmount { get; set; }
        public bool IsPaid { get; set; }
        public string PaymentStatus { get; set; } = string.Empty;
        public bool IsCancelled { get; set; }
        public string? CancellationReason { get; set; }
    }
}
