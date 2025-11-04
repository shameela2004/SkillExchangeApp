using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class GroupSessionBooking : BaseEntity
    {
        public int GroupSessionId { get; set; }
        public GroupSession GroupSession { get; set; } = null!;

        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int? PaymentId { get; set; }
        public Payment? Payment { get; set; }

        public bool IsPaid { get; set; } = false;
        public bool IsCancelled { get; set; } = false;
    }

}
