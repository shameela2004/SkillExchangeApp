using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.DTOs.Auth
{
    public class RevokeRequest
    {
        public string RefreshToken { get; set; } = string.Empty;

    }
}
