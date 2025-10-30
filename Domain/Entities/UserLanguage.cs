using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class UserLanguage : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int LanguageId { get; set; }
        public Language Language { get; set; } = null!;

        public string? Proficiency { get; set; } // Optional, e.g., "Native", "Fluent", "Basic"
    }

}
