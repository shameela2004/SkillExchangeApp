using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Payment
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public int LearnerId { get; set; }
        public int MentorId { get; set; }
        public decimal Amount { get; set; }
        public string PaymentMethod { get; set; } = string.Empty;
        public string? RazorpayOrderId { get; set; }
        public string? RazorpayPaymentId { get; set; }
        public string? RazorpaySignature { get; set; }
        public string Status { get; set; } = string.Empty;
        public decimal CommissionPercent { get; set; }
        public decimal CommissionAmount { get; set; }
        public decimal MentorAmount { get; set; }
        public string SettlementStatus { get; set; } = string.Empty;
        public DateTime? SettlementDate { get; set; }
    }
}
