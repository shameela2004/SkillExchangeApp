using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Booking
{
    public class BookingResponseDto
    {
        public int BookingId { get; set; }
        public int PaymentId { get; set; }
        public string RazorpayOrderId { get; set; }
    }

}
