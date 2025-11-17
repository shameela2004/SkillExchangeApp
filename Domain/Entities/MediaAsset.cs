using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class MediaAsset : BaseEntity
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public byte[] Data { get; set; }
        public int UploadedByUserId { get; set; } 
        public User UploadedByUser { get; set; }
        public string ReferenceType { get; set; } // e.g. "UserProfile", "Post", "MentorProfile"
        public int ReferenceId { get; set; } // ID of entity media relates to

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
