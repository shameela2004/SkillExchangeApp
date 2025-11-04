using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Rating
{
    public class RatingDto
    {
        public int SkillId { get; set; }
        public int RatingValue { get; set; }
        public string? Feedback { get; set; }
    }
}
