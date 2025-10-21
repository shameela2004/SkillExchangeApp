using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class GroupSession : BaseEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public DateTime ScheduledAt { get; set; }
        public string Mode { get; set; } = string.Empty; // Online / Offline
        public string? Notes { get; set; }
    }
}
