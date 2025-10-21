using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Subscription : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public string Plan { get; set; } = string.Empty; // Monthly / Yearly
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
