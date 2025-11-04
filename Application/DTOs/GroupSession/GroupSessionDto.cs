using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.GroupSession
{
    public class GroupSessionDto
    {
        public int Id { get; set; }
        public int GroupId { get; set; }
        public DateTime ScheduledAt { get; set; }
        public string Mode { get; set; } = string.Empty;
        public string? Notes { get; set; }
        public string? VideoLink { get; set; }
        public decimal? Price { get; set; }
        public bool IsCompleted { get; set; }
    }

}
