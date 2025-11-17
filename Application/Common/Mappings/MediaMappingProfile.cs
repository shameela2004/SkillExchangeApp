//using AutoMapper;
//using MyApp1.Application.DTOs.MediaAsset;
//using MyApp1.Domain.Entities;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

//namespace MyApp1.Application.Common.Mappings
//{
//    public class MediaMappingProfile :Profile
//    {
//        public MediaMappingProfile()
//        {
//            CreateMap<MediaAsset, MediaAssetDto>()
//                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => $"/api/media/{src.Id}"));
//        }
//    }
//}

