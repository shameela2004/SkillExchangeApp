using AutoMapper;
using MyApp1.Application.DTOs.GroupSession;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class GroupSessionMappingProfile :Profile
    {
        public GroupSessionMappingProfile()
        {
                CreateMap<GroupSession, GroupSessionDto>()
    .ForMember(d => d.GroupName, opt => opt.MapFrom(s => s.Group.Name));

            CreateMap<CreateGroupSessionDto, GroupSession>();
            CreateMap<UpdateGroupSessionDto, GroupSession>();
        }
    }
}
