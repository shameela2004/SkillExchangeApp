using AutoMapper;
using MyApp1.Application.Common.Mappings;
using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.AutoMapper
{
    public class UserProfilePictureProvider : IUserProfilePictureProvider
    {
        private readonly MyApp1DbContext _myApp1DbContext;
        private readonly IMediaService _mediaService;
        private readonly MyApp1DbContext _context;
        public UserProfilePictureProvider(MyApp1DbContext context, IMediaService mediaService)
        {
            _context = context;
            _mediaService = mediaService;
        }
        //public string GetProfilePictureUrl(int userId)
        //{
        //    return _context.MediaAssets
        //        .Where(m => m.ReferenceType == "UserProfile" && m.ReferenceId == userId)
        //        .Select(m => m.FileName)
        //        .FirstOrDefault() ?? string.Empty;
        //}
        public string GetProfilePictureUrl(int userId)
        {
            //var profileMedia = _mediaService.GetMediaByReferenceAsync("UserProfile", userId).GetAwaiter().GetResult();

            //var profileImage = profileMedia.OrderByDescending(m => m.Id).FirstOrDefault();
            //if (profileImage != null)
            //{
            //    return $"/api/media/{profileImage.Id}";  
            //}
            //Console.WriteLine("No profile image found for user ID: " + userId);
            //return string.Empty;
            // Use synchronous EF Core calls, e.g.:
            var media = _context.MediaAssets
                .Where(m => m.ReferenceType == "UserProfile" && m.ReferenceId == userId)
                .OrderByDescending(m => m.Id)
                .FirstOrDefault();

            return media != null ? $"/api/media/{media.Id}" : string.Empty;
        }
    }
}
