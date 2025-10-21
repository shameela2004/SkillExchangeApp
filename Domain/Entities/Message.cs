using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Message : BaseEntity
    {
        public int FromUserId { get; set; }
        public User FromUser { get; set; } = null!;
        public int ToUserId { get; set; }
        public User ToUser { get; set; } = null!;
        public int? SessionId { get; set; }
        public Session? Session { get; set; }

        public string Content { get; set; } = string.Empty;
        public string? VoiceFilePath { get; set; }
        public string? FilePath { get; set; }
    }
}
