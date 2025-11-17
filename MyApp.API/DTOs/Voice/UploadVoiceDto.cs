using Microsoft.AspNetCore.Mvc;

namespace MyApp1.API.DTOs.Voice
{
    public class UploadVoiceDto
    {
        //public int ToUserId { get; set; }

        [FromForm]
        public IFormFile VoiceFile { get; set; }
    }

}
