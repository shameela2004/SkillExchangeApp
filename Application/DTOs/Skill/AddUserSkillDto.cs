using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Skill
{
    public class AddUserSkillDto
    {
        public int SkillId { get; set; }
        public string Type { get; set; } = string.Empty; // Learn / Teach
    }

}
