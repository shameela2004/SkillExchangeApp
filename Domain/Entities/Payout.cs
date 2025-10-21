using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Payout : BaseEntity
    {
        public int PaymentId { get; set; }
        public Payment Payment { get; set; } = null!;

        public int MentorId { get; set; }
        public User Mentor { get; set; } = null!;

        public int AdminId { get; set; } // who initiated
        public User Admin { get; set; } = null!;

        public decimal AmountReleased { get; set; }
        public decimal CommissionAmount { get; set; }
        public string Method { get; set; } = string.Empty; // RazorpayX / Manual / BankTransfer
        public string Status { get; set; } = string.Empty; // Pending / Success / Failed
        public string? TransactionRef { get; set; } // RazorpayX / Bank reference
        public DateTime? ReleasedAt { get; set; }
    }
}
