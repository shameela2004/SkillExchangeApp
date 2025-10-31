﻿using AutoMapper;
using MyApp1.Application.DTOs.Booking;
using MyApp1.Application.DTOs.Connection;
using MyApp1.Application.DTOs.Group;
using MyApp1.Application.DTOs.GroupSession;
using MyApp1.Application.DTOs.Language;
using MyApp1.Application.DTOs.Notification;
using MyApp1.Application.DTOs.Post;
using MyApp1.Application.DTOs.Session;
using MyApp1.Application.DTOs.Skill;
using MyApp1.Application.DTOs.User;
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
           .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.UserSkills))
            .ForMember(dest => dest.Languages, opt => opt.MapFrom(src => src.UserLanguages))
            .ForMember(dest => dest.Badges, opt => opt.MapFrom(src => src.UserBadges));
            CreateMap<UserSkill, UserSkillDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));


            //CreateMap<UpdateUserDto, User>()
            //    .ForMember(dest => dest.UserSkills, opt => opt.Ignore()) // Skills not updated here
            //    .ForMember(dest => dest.Id, opt => opt.Ignore());          // Prevent Id overwrite
            //CreateMap<UserSkill, UserSkillDto>()
            //    .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));

            CreateMap<UserBadge, UserBadgeDto>()
    .ForMember(dest => dest.BadgeName, opt => opt.MapFrom(src => src.Badge.Name))
    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Badge.Description))
    .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill != null ? src.Skill.Name : null))
    .ForMember(dest => dest.EarnedAt, opt => opt.MapFrom(src => src.CreatedAt));

          

            CreateMap<Notification, NotificationDto>();

            CreateMap<Connection, ConnectionDto>()
    .ForMember(dest => dest.ConnectionId, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.ConnectedUserId))
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.ConnectedUser.Name))
    .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));


            CreateMap<Post, PostDto>()
    .ForMember(dest => dest.PostId, opt => opt.MapFrom(src => src.Id))
    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
    .ForMember(dest => dest.CommentCount, opt => opt.MapFrom(src => src.CommentCount))
    .ForMember(dest => dest.LikeCount, opt => opt.MapFrom(src => src.LikeCount))
    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<PostComment, PostCommentDto>()
                .ForMember(dest => dest.CommentId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Name))
                .ForMember(dest => dest.CommentText, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));

            CreateMap<EditPostDto, Post>()
       .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
       .ForMember(dest => dest.MediaUrl, opt => opt.MapFrom(src => src.MediaUrl));

            // Language Mappings
            CreateMap<Language, LanguageResponseDto>();
            CreateMap<UserLanguage, UserLanguageResponseDto>()
                .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language.Name));
            CreateMap<AddUserLanguageDto, UserLanguage>();
            CreateMap<UserLanguage, UserLanguageDto>()
    .ForMember(dest => dest.LanguageName, opt => opt.MapFrom(src => src.Language.Name));


            // Skill Mappings
            CreateMap<Skill, SkillResponseDto>();
            CreateMap<UserSkill, UserSkillDto>()
    .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));

            CreateMap<UserSkill, UserSkillResponseDto>()
                .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skill.Name));
            CreateMap<AddUserSkillDto, UserSkill>();

            // Session Mappings
            CreateMap<Session, SessionDto>();
            CreateMap<CreateSessionDto, Session>();
            CreateMap<UpdateSessionDto, Session>();

            // Group Session
            CreateMap<CreateGroupSessionDto, GroupSession>();
            CreateMap<GroupSession, GroupSessionDto>();

            // Booking
            CreateMap<BookSessionDto, Booking>();
            CreateMap<Booking, BookingDto>();

          
        }
    }
}
