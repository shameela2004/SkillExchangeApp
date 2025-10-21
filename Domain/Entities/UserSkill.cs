using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class UserSkill : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;

        public string Type { get; set; } = string.Empty; // Learn / Teach
    }
}
