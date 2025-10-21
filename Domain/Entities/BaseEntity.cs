using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public int? LastUpdatedBy { get; set; }
        public DateTime? LastUpdatedAt { get; set; }

        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Mostly for Users
        public DateTime? LastLogout { get; set; }
    }
}
