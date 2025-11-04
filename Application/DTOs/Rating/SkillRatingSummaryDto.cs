using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Rating
{
    public class SkillRatingSummaryDto
    {
        public int SkillId { get; set; }
        public double AvgRating { get; set; }
        public int TotalRatings { get; set; }
        public int Points { get; set; }
        public string Badge { get; set; } = string.Empty;
    }
}
