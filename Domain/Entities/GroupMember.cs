using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class GroupMember : BaseEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public int UserId { get; set; }
        public User User { get; set; } = null!;
    }
}
