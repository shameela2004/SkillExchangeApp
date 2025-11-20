using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public interface IUserProfilePictureProvider
    {
        string GetProfilePictureUrl(int userId);
    }
}
