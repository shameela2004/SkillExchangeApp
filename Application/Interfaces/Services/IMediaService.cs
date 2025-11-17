using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IMediaService
    {
        Task<int> UploadMediaAsync(MediaAsset media);
        Task<MediaAsset?> GetMediaAsync(int id);
        Task<IEnumerable<MediaAsset>> GetMediaByReferenceAsync(string referenceType, int referenceId);
    }
}
