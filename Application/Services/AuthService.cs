using Microsoft.Extensions.Configuration;
using MyApp1.Application.DTOs.Auth;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Domain.Interfaces;
using MyApp1.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthService(IGenericRepository<User> userRepository, IConfiguration configuration,ITokenService tokenService)
        {
            _userRepository = userRepository;
            _configuration = configuration;
            _tokenService = tokenService;
        }

        public async Task RegisterAsync(RegisterRequest request)
        {
            var existingUser = (await _userRepository.FindAsync(u => u.Email == request.Email)).FirstOrDefault();
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                Email = request.Email,
                Name = request.Username,
                PasswordHash = PasswordHasher.HashPassword(request.Password)
            };

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();
        }

        public async Task<JwtTokenResponse> LoginAsync(LoginRequest request)
        {
            var user = (await _userRepository.FindAsync(u => u.Email == request.Email)).FirstOrDefault();

            if (user == null || !PasswordHasher.VerifyPassword(request.Password, user.PasswordHash))
            {
                throw new Exception("Invalid email or password.");
            }

            var (token, refreshToken) = _tokenService.GenerateTokens(user);
            var expiresIn = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"]));

            return new JwtTokenResponse
            {
                Token = token,
                RefreshToken = refreshToken,
                Expiration = expiresIn
            };
        }

        public async Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken)
        {
            var tokens = await _tokenService.RefreshTokenAsync(token, refreshToken);
            return tokens;
        }

        public async Task RevokeRefreshTokenAsync(string refreshToken)
        {
            await _tokenService.RevokeRefreshToken(refreshToken);
        }
    }
}