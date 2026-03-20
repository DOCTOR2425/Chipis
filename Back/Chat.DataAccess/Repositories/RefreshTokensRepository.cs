using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace Chipis.DataAccess.Repositories
{
    public class RefreshTokensRepository : IRefreshTokensRepository
    {
        private readonly ChipisDbContext _context;
        private readonly IHashProvider _hashProvider;

        public RefreshTokensRepository(
            ChipisDbContext context,
            IHashProvider hashProvider)
        {
            _context = context;
            _hashProvider = hashProvider;
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

        public async Task<RefreshToken> GetByString(string token)
        {
            RefreshTokenEntity tokenEntity = await _context.RefreshTokenEntity
                .Include(t => t.UserEntity)
                .FirstOrDefaultAsync(t => t.TokenHash == Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(token))));

            RefreshToken refreshToken = new RefreshToken(
                tokenEntity.RefreshTokenEntityId,
                tokenEntity.TokenHash,
                tokenEntity.CreatedAt,
                tokenEntity.ExpiresAt,
                new User(
                    tokenEntity.UserEntity.UserEntityId,
                    tokenEntity.UserEntity.Nickname,
                    tokenEntity.UserEntity.Telephone,
                    tokenEntity.UserEntity.HashPassword));

            return refreshToken;
        }
    }
}
