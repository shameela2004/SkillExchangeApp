using AutoMapper;
using MyApp1.Application.DTOs.Rating;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class RatingMappingProfile : Profile
    {
        public RatingMappingProfile()
        {
            CreateMap<Rating, RatingDto>();
            CreateMap<CreateRatingDto, Rating>();
            CreateMap<Rating, SkillRatingSummaryDto>()
                .ForMember(dest => dest.AvgRating, opt => opt.Ignore()) // Will be computed separately
                .ForMember(dest => dest.TotalRatings, opt => opt.Ignore())
                .ForMember(dest => dest.Points, opt => opt.Ignore())
                .ForMember(dest => dest.Badge, opt => opt.Ignore());
            CreateMap<Rating, LeaderboardEntryDto>()
                .ForMember(dest => dest.MentorId, opt => opt.MapFrom(src => src.RatedToUserId))
                .ForMember(dest => dest.Points, opt => opt.Ignore())
                .ForMember(dest => dest.Badge, opt => opt.Ignore());
            CreateMap<UpdateRatingDto, Rating>()
           .ForMember(dest => dest.Session, opt => opt.Ignore())
           .ForMember(dest => dest.RatedByUser, opt => opt.Ignore())
           .ForMember(dest => dest.RatedToUser, opt => opt.Ignore())
           .ForMember(dest => dest.Skill, opt => opt.Ignore())
           .ForMember(dest => dest.Id, opt => opt.Ignore())
           .ForMember(dest => dest.SessionId, opt => opt.Ignore())
           .ForMember(dest => dest.RatedByUserId, opt => opt.Ignore())
           .ForMember(dest => dest.RatedToUserId, opt => opt.Ignore())
           .ForMember(dest => dest.SkillId, opt => opt.Ignore())
           .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
           .ForMember(dest => dest.IsDeleted, opt => opt.Ignore());
        }
    }

}
