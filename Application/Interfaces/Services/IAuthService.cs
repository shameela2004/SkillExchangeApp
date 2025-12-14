using MyApp1.Application.DTOs.Auth;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterRequest request);
        Task<JwtTokenResponse> LoginAsync(LoginRequest request);
        Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken);
        Task<(JwtTokenResponse Tokens, User User)> RefreshTokenWithUserAsync(string accessToken, string refreshToken);
        Task RevokeRefreshTokenAsync(string refreshToken);
    }
}
