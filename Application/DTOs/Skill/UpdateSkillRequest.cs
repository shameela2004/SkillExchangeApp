using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Skill
{
    public class UpdateSkillRequest
    {
        public int Id { get; set; }  // Required to identify record
        public string Name { get; set; } = string.Empty;
    }
}
