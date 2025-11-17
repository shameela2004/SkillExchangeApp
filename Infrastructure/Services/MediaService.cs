using Microsoft.EntityFrameworkCore;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.Services
{
    public  class MediaService : IMediaService
    {
        private readonly IGenericRepository<MediaAsset> _mediaAssetRepository;
        public MediaService(IGenericRepository<MediaAsset> mediaAssetRepository)
        {
            _mediaAssetRepository = mediaAssetRepository;
        }
        public async Task<int> UploadMediaAsync(MediaAsset media)
        {
            await _mediaAssetRepository.AddAsync(media);
            await _mediaAssetRepository.SaveChangesAsync();
            return media.Id;
        }

        public async Task<MediaAsset?> GetMediaAsync(int id)
        {
            return await _mediaAssetRepository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<MediaAsset>> GetMediaByReferenceAsync(string referenceType, int referenceId)
        {
            return await _mediaAssetRepository.Table
                .Where(m => m.ReferenceType == referenceType && m.ReferenceId == referenceId)
                .ToListAsync();
        }

    }
}
