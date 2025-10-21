using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace MyApp1.Domain.Entities
{
    public class Skill : BaseEntity
    {
        public string Name { get; set; } = string.Empty;

        // Navigation
        public ICollection<UserSkill>? UserSkills { get; set; }
        public ICollection<Session>? Sessions { get; set; }
        public ICollection<Group>? Groups { get; set; }
        public ICollection<Rating>? Ratings { get; set; }
        public ICollection<UserBadge>? UserBadges { get; set; }
    }
}
