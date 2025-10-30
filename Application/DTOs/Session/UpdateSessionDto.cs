using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Session
{
    public class UpdateSessionDto
    {
        public DateTime ScheduledAt { get; set; }
        public string? Notes { get; set; }
        public decimal? Price { get; set; }
    }

}
