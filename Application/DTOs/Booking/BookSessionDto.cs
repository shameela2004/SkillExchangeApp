using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Booking
{
    public class BookSessionDto
    {
        public int SessionId { get; set; }
        public int LearnerId { get; set; }
        public decimal? PaymentAmount { get; set; }
    }
}
