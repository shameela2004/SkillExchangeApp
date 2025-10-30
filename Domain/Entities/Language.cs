using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Language : BaseEntity
    {
        public string Code { get; set; } = string.Empty; // e.g., "en", "fr"
        public string Name { get; set; } = string.Empty; // e.g., "English", "French"
        public ICollection<UserLanguage>? UserLanguages { get; set; }
    }

}
