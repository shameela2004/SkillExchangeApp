using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.User
{
    public class SearchUserDto
    {
        public string? Skill { get; set; }
        public string? Location { get; set; }
        public string? Role { get; set; }  // Learner / Mentor / Both or empty to search all
        //public int? MinSkillLevel { get; set; }  // optional filter for skill proficiency if available
    }

}
