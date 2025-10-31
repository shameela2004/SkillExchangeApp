using AutoMapper;
using MyApp1.Application.DTOs.Mentor;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{

    public class MentorMappingProfile : Profile
    {
        public MentorMappingProfile()
        {
            CreateMap<User, MentorDto>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.MentorStatus, opt => opt.MapFrom(src => src.MentorStatus ?? "None"))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.MentorProfile != null ? src.MentorProfile.PhoneNumber : string.Empty))
                .ForMember(dest => dest.AadhaarImageUrl, opt => opt.MapFrom(src => src.MentorProfile != null ? src.MentorProfile.AadhaarImageUrl : string.Empty))
                .ForMember(dest => dest.SocialProfileUrl, opt => opt.MapFrom(src => src.MentorProfile != null ? src.MentorProfile.SocialProfileUrl : string.Empty))
               .ForMember(dest => dest.Availabilities, opt => opt.MapFrom(src => src.MentorProfile != null ? src.MentorProfile.Availabilities : new List<MentorAvailability>())); 

            CreateMap<MentorAvailability, MentorAvailabilityDto>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

            CreateMap<MentorApplicationDto, MentorProfile>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.AadhaarImageUrl, opt => opt.MapFrom(src => src.AadhaarImageUrl))
                .ForMember(dest => dest.SocialProfileUrl, opt => opt.MapFrom(src => src.SocialProfileUrl))
                  .ForMember(dest => dest.Availabilities, opt => opt.Ignore());  // Will be handled separately

            CreateMap<MentorAvailabilityDto, MentorAvailability>()
                .ForMember(dest => dest.DayOfWeek, opt => opt.MapFrom(src => src.DayOfWeek))
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));
        }
    }
}
