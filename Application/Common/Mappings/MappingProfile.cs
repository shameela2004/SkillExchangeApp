using AutoMapper;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.DTOs.UserSkill;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class MappingProfile : Profile

    {
        public MappingProfile() {
            CreateMap<User, UserDto>()
           .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.UserSkills));

            CreateMap<UserSkill, UserSkillDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));

            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.UserSkills, opt => opt.Ignore()) // Skills not updated here
                .ForMember(dest => dest.Id, opt => opt.Ignore());          // Prevent Id overwrite
        }
    }
}
