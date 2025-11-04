using AutoMapper;
using MyApp1.Application.DTOs.UserReport;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class UserReportMappingProfile : Profile
    {
        public UserReportMappingProfile()
        {
            CreateMap<UserReport, UserReportDto>()
                .ForMember(dest => dest.ReportedUserName, opt => opt.MapFrom(src => src.ReportedUser.Name))
                .ForMember(dest => dest.ReporterUserName, opt => opt.MapFrom(src => src.ReporterUser.Name));
            CreateMap<CreateUserReportDto, UserReport>();
            CreateMap<UpdateUserReportDto, UserReport>();
        }
    }

}
