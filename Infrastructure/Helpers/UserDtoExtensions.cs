using MyApp1.Application.DTOs.User;
using MyApp1.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Helpers
{
    public static class UserDtoExtensions
    {
        public static async Task AssignProfilePictureAsync(this UserDto userDto, IMediaService mediaService)
        {
            if (userDto == null) return;

            var profileMedia = await mediaService.GetMediaByReferenceAsync("UserProfile", userDto.Id);
            var profileImage = profileMedia.OrderByDescending(m => m.Id).FirstOrDefault();

            if (profileImage != null)
            {
                userDto.ProfilePictureUrl = $"/api/media/{profileImage.Id}";
            }
        }

        //public static async Task AssignProfilePicturesAsync(this IEnumerable<UserDto> users, IMediaService mediaService)
        //{
        //    var userIds = users.Select(u => u.Id).ToList();

        //    var mediaByUserId = (await mediaService.GetMediaByReferenceBatchAsync("UserProfile", userIds))
        //                         .GroupBy(m => m.ReferenceId)
        //                         .ToDictionary(g => g.Key, g => g.OrderByDescending(m => m.Id).FirstOrDefault());

        //    foreach (var user in users)
        //    {
        //        if (mediaByUserId.TryGetValue(user.Id, out var media))
        //        {
        //            user.ProfilePictureUrl = $"/api/media/{media.Id}";
        //        }
        //    }
        //}

    }
}
