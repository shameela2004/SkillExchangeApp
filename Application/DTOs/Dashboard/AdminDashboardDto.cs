using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Dashboard
{
    public class AdminDashboardSummaryDto
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalMentors { get; set; }
        public int PendingMentors { get; set; }
        public int TotalBookings { get; set; }
        public int ConfirmedBookings { get; set; }
        public int CancelledBookings { get; set; }
        public decimal TotalRevenue { get; set; } // sum of paid paymentAmount


        public int BookingsToday { get; set; }
        public int BookingsThisWeek { get; set; }
        public DateTime? LastMentorApprovedAt { get; set; }
    }

}
