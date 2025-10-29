using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Post
{
    public class PostCommentDto
    {
        public int CommentId { get; set; }
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string CommentText { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
