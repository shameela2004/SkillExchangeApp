using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Post : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        public string Content { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
        public int LikeCount { get; set; } = 0;
        public int CommentCount { get; set; } = 0;

        public ICollection<PostComment>? Comments { get; set; }
        public ICollection<PostLike>? Likes { get; set; }
    }
}
