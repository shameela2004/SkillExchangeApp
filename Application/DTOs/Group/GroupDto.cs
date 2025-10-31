using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Group
{
    public class GroupDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int SkillId { get; set; }
        public int MentorId { get; set; }
        public int MaxMembers { get; set; }
    }
}
