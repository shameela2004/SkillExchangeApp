using AutoMapper;
using MyApp1.Application.DTOs.Message;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class MessageMappingProfile : Profile
    {
        public MessageMappingProfile()
        {
            CreateMap<MessageCreateDto, Message>()
                .ForMember(dest => dest.FromUserId, opt => opt.Ignore())  // set in service/controller
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // set in service/entity

        CreateMap<Message, MessageDto>()
            .ForMember(dest => dest.FromUserName, opt => opt.MapFrom(src => src.FromUser.Name));
        }
    }
}
