using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.MediaAsset
{
    public class MediaUploadRequest
    {
        public IFormFile File { get; set; }
        public string ReferenceType { get; set; }
        public int ReferenceId { get; set; }
    }
}
