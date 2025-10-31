using AutoMapper;
using MyApp1.Application.DTOs.Group;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class GroupMappingProfile : Profile
    {
        public GroupMappingProfile()
        {
            CreateMap<Group, GroupDto>();
            CreateMap<CreateGroupDto, Group>();

            CreateMap<GroupMember, GroupMemberDto>().ReverseMap();

            CreateMap<GroupMessage, GroupMessageDto>().ReverseMap();
            CreateMap<SendGroupMessageDto, GroupMessage>();
        }
    }

}
