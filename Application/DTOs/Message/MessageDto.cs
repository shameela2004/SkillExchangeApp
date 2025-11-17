using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Message
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int FromUserId { get; set; }
        public string FromUserName { get; set; } = string.Empty;
        public int ToUserId { get; set; }
        public string Content { get; set; } = string.Empty;
        public string? VoiceFilePath { get; set; }
        public string? FilePath { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
