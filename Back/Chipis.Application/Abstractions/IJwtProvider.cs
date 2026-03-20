using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IJwtProvider
    {
        string GenerateAccessToken(Guid userId);
        (string token, string tokenHash, DateTime expiresAt) GenerateRefreshToken(Guid userId);
    }
}