using AutoMapper;
using MyApp1.Application.DTOs.User;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class UserProfilePictureResolver : IValueResolver<User, UserDto, string>
    {
        private readonly IUserProfilePictureProvider _provider;
        public UserProfilePictureResolver(IUserProfilePictureProvider provider)
        {
            _provider = provider;
        }
        public string Resolve(User source, UserDto destination, string destMember, ResolutionContext context)
        {
            return _provider.GetProfilePictureUrl(source.Id);
        }
    }
}
