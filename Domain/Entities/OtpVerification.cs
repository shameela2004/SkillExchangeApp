using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class OtpVerification :BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string OtpCode { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
        public bool Used { get; set; } = false;
        public string Purpose { get; set; } = string.Empty;  // e.g. "Signup" or "ResetPassword"
    }
}
