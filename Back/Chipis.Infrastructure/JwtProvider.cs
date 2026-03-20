using Chipis.Application.Abstractions;
using Chipis.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chipis.Infrastructure
{
    public class JwtProvider : IJwtProvider
    {
        private readonly IHashProvider _hashProvider;
        private readonly JwtOptions _options;

        public JwtProvider(IHashProvider hashProvider, IOptions<JwtOptions> options)
        {
            _hashProvider = hashProvider;
            _options = options.Value;
        }

        public string GenerateAccessToken(Guid userId)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                    SecurityAlgorithms.HmacSha256);
            Claim[] claims = [new Claim(ClaimTypes.NameIdentifier, userId.ToString())];

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddMinutes(_options.AccessTokenLifetimeMinutes));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string _GenerateRefreshToken(Guid userId)
        {
            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Key)),
                    SecurityAlgorithms.HmacSha256);
            Claim[] claims = [new Claim(ClaimTypes.NameIdentifier, userId.ToString())];

            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddDays(_options.RefreshTokenLifetimeDays));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public (string token, string tokenHash, DateTime expiresAt) 
            GenerateRefreshToken(Guid userId)
        {
            string token = _GenerateRefreshToken(userId);
            var tokenHash = _hashProvider.Generate(token);
            var expiresAt = DateTime.UtcNow.AddDays(_options.RefreshTokenLifetimeDays);

            return (token, tokenHash, expiresAt);
        }
    }
}
