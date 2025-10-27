using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IOtpService
    {
        Task GenerateAndSendOtpAsync( string email, string purpose);
        Task<bool> VerifyOtpAsync(int userId, string otpCode, string purpose);
        Task ResetPasswordAsync(int userId, string otpCode, string newPassword, string confirmPassword);

    }
}

