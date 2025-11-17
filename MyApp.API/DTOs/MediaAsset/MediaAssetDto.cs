using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.MediaAsset
{
    public class MediaAssetDto
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        // Optional: if you want to send base64 image directly
        public string Base64Image { get; set; }
    }



}
