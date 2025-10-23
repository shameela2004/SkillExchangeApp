using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Interfaces.Services
{
    public interface ITokenService
    {
        (string Token, string RefreshToken) GenerateTokens(User user);
        Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken);
        Task RevokeRefreshToken(string refreshToken);
    }
}
