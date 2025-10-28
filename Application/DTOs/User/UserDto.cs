using MyApp1.Application.DTOs.UserSkill;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.User
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? Location { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public ICollection<UserSkillDto>? Skills { get; set; }
    }
}
