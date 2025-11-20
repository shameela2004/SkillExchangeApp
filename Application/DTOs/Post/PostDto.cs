using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Post
{
    public class PostDto
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
        public int LikeCount { get; set; }
        public int CommentCount { get; set; }
        public bool HasLiked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
