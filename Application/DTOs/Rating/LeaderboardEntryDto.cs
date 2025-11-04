using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Rating
{
    public class LeaderboardEntryDto
    {
        public int MentorId { get; set; }
        public int Points { get; set; }
        public string Badge { get; set; } = string.Empty;
    }
}
