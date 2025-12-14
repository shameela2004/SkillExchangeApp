using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.User
{
    public class PagedUsersResult
    {
        public IEnumerable<UserDto> Users { get; set; } = Enumerable.Empty<UserDto>();
        public int TotalCount { get; set; }
        public int ActiveCount { get; set; }
        public int InactiveCount { get; set; }
        public int MentorCount { get; set; }
        public int PendingMentorCount { get; set; }
    }
}
