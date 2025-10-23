using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyApp1.Application.Interfaces.Services;
using MyApp1.Domain.Entities;
using MyApp1.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Services
{
    public class TokenService :ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly MyApp1DbContext _context;

        public TokenService(IConfiguration configuration, MyApp1DbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public (string Token, string RefreshToken) GenerateTokens(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"]);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Name ?? string.Empty),
                new Claim(ClaimTypes.Email, user.Email ?? string.Empty),
                new Claim(ClaimTypes.Role, user.Role ?? "Learner")
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["JwtSettings:ExpiryMinutes"])),
                Issuer = _configuration["JwtSettings:Issuer"],
                Audience = _configuration["JwtSettings:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            // Generate refresh token
            var refreshToken = CreateRefreshToken();

            // Save refresh token to database
            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expires = DateTime.UtcNow.AddDays(7), // Valid for 7 days
                UserId = user.Id
            };

            _context.RefreshTokens.Add(refreshTokenEntity);
            _context.SaveChanges();

            return (jwtToken, refreshToken);
        }

        public async Task<(string Token, string RefreshToken)> RefreshTokenAsync(string token, string refreshToken)
        {
            var principal = GetPrincipalFromExpiredToken(token);
            if (principal == null)
                throw new SecurityTokenException("Invalid token");

            var userIdClaim = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                throw new SecurityTokenException("Invalid token - no user id claim");

            var userId = int.Parse(userIdClaim.Value);
            var user = _context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null)
                throw new SecurityTokenException("Invalid token");

            var storedRefreshToken = _context.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken && rt.UserId == userId);
            if (storedRefreshToken == null || storedRefreshToken.IsRevoked || storedRefreshToken.Expires <= DateTime.UtcNow)
                throw new SecurityTokenException("Invalid refresh token");

            // Revoke old refresh token
            storedRefreshToken.IsRevoked = true;
            await _context.SaveChangesAsync();

            return GenerateTokens(user);
        }

        public async Task RevokeRefreshToken(string refreshToken)
        {
            var storedRefreshToken = _context.RefreshTokens.SingleOrDefault(rt => rt.Token == refreshToken);
            if (storedRefreshToken != null)
            {
                storedRefreshToken.IsRevoked = true;
                await _context.SaveChangesAsync();
            }
        }

        private string CreateRefreshToken()
        {
            var randomNumber = new byte[64];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:SecretKey"])),
                ValidateLifetime = false,  // We want to extract claims from an expired token
                ValidIssuer = _configuration["JwtSettings:Issuer"],
                ValidAudience = _configuration["JwtSettings:Audience"],
                ClockSkew = TimeSpan.Zero
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
                if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
                    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    throw new SecurityTokenException("Invalid token");

                return principal;
            }
            catch
            {
                return null;
            }
        }
    }
}
