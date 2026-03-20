using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IRefreshTokensRepository
    {
        Task<Guid> Create(RefreshToken refreshToken);
        Task<Guid> Delete(Guid tokenId);
        Task<Guid> Revoke(Guid tokenId);
    }
}