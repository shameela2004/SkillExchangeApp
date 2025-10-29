using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Connection
{
    public class ConnectionDto
    {
        public int ConnectionId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; // Pending, Accepted, Rejected
        public DateTime CreatedAt { get; set; }
    }

}
