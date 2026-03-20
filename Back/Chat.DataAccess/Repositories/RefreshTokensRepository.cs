using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess.Repositories
{
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private readonly ChipisDbContext _context;

        public RefreshTokensRepository(ChipisDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(RefreshToken refreshToken)
        {
            RefreshTokenEntity refreshTokenEntity = new RefreshTokenEntity
            {
                RefreshTokenEntityId = refreshToken.RefreshTokenId,
                TokenHash = refreshToken.TokenHash,
                CreatedAt = refreshToken.CreatedAt,
                ExpiresAt = refreshToken.ExpiresAt,
                RevokedAt = refreshToken.RevokedAt,
                UserEntity = await _context.UserEntity
                    .FindAsync(refreshToken.User.UserId)
            };

            await _context.RefreshTokenEntity.AddAsync(refreshTokenEntity);
            await _context.SaveChangesAsync();

            return refreshTokenEntity.RefreshTokenEntityId;
        }

        public async Task<Guid> Delete(Guid tokenId)
        {
            await _context.RefreshTokenEntity
                .Where(r => r.RefreshTokenEntityId == tokenId)
                .ExecuteDeleteAsync();

            return tokenId;
        }

        public async Task<Guid> Revoke(Guid tokenId)
        {
            await _context.RefreshTokenEntity
                .Where(t => t.RefreshTokenEntityId == tokenId)
                .ExecuteUpdateAsync(r => r
                    .SetProperty(t => t.RevokedAt, DateTime.Now));

            return tokenId;
        }

        //public async Task<RefreshToken> GetByUser
    }
}
