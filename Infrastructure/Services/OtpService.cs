using MyApp1.Application.Exceptions;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using MyApp1.Infrastructure.Data;
using MyApp1.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Services
{
    public class OtpService :IOtpService
    {

        private readonly IGenericRepository<OtpVerification> _otpRepo;
        private readonly IGenericRepository<User> _userRepo;
        private readonly IEmailSenderService _emailSender;

        public OtpService(IGenericRepository<OtpVerification> otpRepo,
                          IGenericRepository<User> userRepo,
                          IEmailSenderService emailSender)
        {
            _otpRepo = otpRepo;
            _userRepo = userRepo;
            _emailSender = emailSender;
        }

        private string HashOtp(string otp)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(otp);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        public async Task GenerateAndSendOtpAsync(string email, string purpose)
        {
            var user = (await _userRepo.FindAsync(u => u.Email == email && !u.IsDeleted)).FirstOrDefault();
            if (user == null)
                throw new NotFoundException("User not found.");

            var otpCode = new Random().Next(100000, 999999).ToString();
            var hashedOtp = HashOtp(otpCode);
            var expiryTime = DateTime.UtcNow.AddMinutes(10);

            var otp = new OtpVerification
            {
                UserId = user.Id,
                OtpCode = hashedOtp,
                Purpose = purpose,
                Expiry = expiryTime,
                Used = false
            };

            await _otpRepo.AddAsync(otp);
            await _otpRepo.SaveChangesAsync();

            string emailBody = $"Your OTP code is {otpCode}. It expires at {expiryTime.ToLocalTime():f}.";
            await _emailSender.SendEmailAsync(email, "Your OTP Code", emailBody);
        }

        public async Task<bool> VerifyOtpAsync(int userId, string otpCode, string purpose)
        {
            var hashedInputOtp = HashOtp(otpCode);
            var otps = await _otpRepo.FindAsync(o =>
                o.UserId == userId && o.OtpCode == hashedInputOtp && o.Purpose == purpose && !o.Used);

            var otp = otps.FirstOrDefault();

            if (otp == null || otp.Expiry < DateTime.UtcNow)
                return false;

            otp.Used = true;
            await _otpRepo.UpdateAsync(otp);

            if (purpose == "Signup")
            {
                var user = await _userRepo.GetByIdAsync(userId);
                if (user != null && !user.IsEmailVerified)
                {
                    user.IsEmailVerified = true;
                    await _userRepo.UpdateAsync(user);
                }
            }

            return true;
        }

        public async Task ResetPasswordAsync(int userId, string otpCode, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
                throw new ValidationException("New password and confirm password do not match.");

            var isOtpValid = await VerifyOtpAsync(userId, otpCode, "ResetPassword");
            if (!isOtpValid)
                throw new ValidationException("Invalid or expired OTP.");

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null)
                throw new NotFoundException("User not found.");

            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            await _userRepo.UpdateAsync(user);
        }

        public async Task ChangePasswordAsync(int userId, string currentPassword, string newPassword, string confirmPassword)
        {
            if (newPassword != confirmPassword)
                throw new Exception("New password and confirmation do not match.");

            var user = await _userRepo.GetByIdAsync(userId);
            if (user == null) throw new Exception("User not found.");

            if (!PasswordHasher.VerifyPassword(currentPassword, user.PasswordHash))
                throw new Exception("Current password is incorrect.");

            user.PasswordHash = PasswordHasher.HashPassword(newPassword);
            await _userRepo.UpdateAsync(user);
        }


    }
}
