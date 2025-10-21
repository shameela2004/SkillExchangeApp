using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Connection : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public int ConnectedUserId { get; set; }
        public User ConnectedUser { get; set; } = null!;
        public string Status { get; set; } = string.Empty; // Pending / Accepted
    }
}
