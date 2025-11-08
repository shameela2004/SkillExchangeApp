using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Group
{
    public class GroupMessageDto
    {
        //public int Id { get; set; }
        //public int GroupId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? FilePath { get; set; }
        public int FromUserId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
