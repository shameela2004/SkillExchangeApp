using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Language
{
    public class UserLanguageResponseDto
    {
        public int LanguageId { get; set; }
        public string LanguageName { get; set; } = string.Empty;
        public string? Proficiency { get; set; }
    }
}
