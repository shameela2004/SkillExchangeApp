using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Post
{
    public class CreatePostDto
    {
        public string Content { get; set; } = string.Empty;
        public string? MediaUrl { get; set; }
    }
}


