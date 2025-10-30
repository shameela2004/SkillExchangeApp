using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Notification : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Title { get; set; } = "New Notification";
        public string Message { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty; // Chat / Session / System / Group
        public bool IsRead { get; set; } = false;
    }
}
