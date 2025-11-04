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
            CreateMap<GroupSession, GroupSessionDto>();
            CreateMap<CreateGroupSessionDto, GroupSession>();
            CreateMap<UpdateGroupSessionDto, GroupSession>();
        }
    }
}
