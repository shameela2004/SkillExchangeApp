using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Rating
{
    public class CreateRatingDto
    {
        public int SessionId { get; set; }  // Can be Session or GroupSession ID
        public int RatingValue { get; set; } // 1-5
        public string? Feedback { get; set; }
        public bool IsGroupSession { get; set; } // True if group session rating
    }
}
