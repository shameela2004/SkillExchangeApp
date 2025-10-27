using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.OtpDtos
{
    public class OtpRequestDto
    {
        public string Email { get; set; } = string.Empty;
        public string Purpose { get; set; } = string.Empty; //
    }
}
