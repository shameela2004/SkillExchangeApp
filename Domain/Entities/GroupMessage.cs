using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class GroupMessage : BaseEntity
    {
        public int GroupId { get; set; }
        public Group Group { get; set; } = null!;
        public int FromUserId { get; set; }
        public User FromUser { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public string? FilePath { get; set; }
    }
}
