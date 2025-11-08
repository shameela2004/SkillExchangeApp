using MyApp1.Application.DTOs.Language;
using MyApp1.Application.DTOs.Mentor;
using MyApp1.Application.DTOs.Post;
using MyApp1.Application.DTOs.Skill;
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
        public string Role { get; set; } = string.Empty;
        public string MentorStatus { get; set; } = string.Empty;
        public ICollection<PostDto> ? Posts { get; set; }
        public ICollection<UserSkillDto>? Skills { get; set; }
        public ICollection<UserLanguageDto>? Languages { get; set; }
        public ICollection<UserBadgeDto>? Badges { get; set; }

        public List<MentorAvailabilityDto>? MentorAvailabilities { get; set; }
    }
}
