using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Message
{
    public class MessageCreateDto
    {
        public int ToUserId { get; set; }
        public string? Content { get; set; } = string.Empty;
        public string? VoiceFilePath { get; set; } // optional voice message path
        public string? FilePath { get; set; }      // optional file attachment path
    }
}
