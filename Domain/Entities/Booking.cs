using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Booking : BaseEntity
    {
        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;

        public int LearnerId { get; set; }
        public User Learner { get; set; } = null!;

        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public string Status { get; set; } = string.Empty; // Pending / Confirmed / Completed / Cancelled
        public decimal PaymentAmount { get; set; }
        public bool IsPaid { get; set; } = false;
        public string PaymentStatus { get; set; } = string.Empty; // Paid / Free / Failed
        public bool IsCancelled { get; set; } = false;
        public string? CancellationReason { get; set; }
        public DateTime? CancelledAt { get; set; }
    }
}
