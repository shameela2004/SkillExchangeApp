using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.OtpDtos
{
    public class OtpVerifyDto
    {
        public int UserId { get; set; }
        public string OtpCode { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty;
    }
}

