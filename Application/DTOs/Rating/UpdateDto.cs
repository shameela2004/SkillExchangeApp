using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Rating
{
    public class UpdateRatingDto
    {
        public int RatingId { get; set; }
        public int RatingValue { get; set; } // 1-5
        public string? Feedback { get; set; }
    }

}
