using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Domain.Entities
{
    public class Group : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public int SkillId { get; set; }
        public Skill Skill { get; set; } = null!;
        public int MentorId { get; set; }
        public User Mentor { get; set; } = null!;
        public int MaxMembers { get; set; }

        // Navigation
        public ICollection<GroupMember>? Members { get; set; }
        public ICollection<GroupSession>? GroupSessions { get; set; }
        public ICollection<GroupMessage>? GroupMessages { get; set; }
    }
}
