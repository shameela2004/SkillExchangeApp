using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public int BookingId { get; set; }
        public Booking Booking { get; set; } = null!;

        public int LearnerId { get; set; }
        public User Learner { get; set; } = null!;

        public int MentorId { get; set; }
        public User Mentor { get; set; } = null!;

        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Razorpay / etc

        public string? RazorpayOrderId { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorpaySignature { get; set; }

        public string Status { get; set; } = string.Empty; // Created / Paid / Failed / Refunded
        public decimal CommissionPercent { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal MentorAmount { get; set; }
        public string SettlementStatus { get; set; } = string.Empty; // Pending / Released / OnHold
        public DateTime? SettlementDate { get; set; }
    }
}
