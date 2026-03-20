namespace Chipis.Core.Models
{
    public class RefreshToken
    {
        public RefreshToken(Guid refreshTokenId, string tokenHash, DateTime createdAt, DateTime expiresAt, User user)
        {
            RefreshTokenId = refreshTokenId;
            TokenHash = tokenHash;
            CreatedAt = createdAt;
            ExpiresAt = expiresAt;
            User = user;
        }

        public Guid RefreshTokenId { get; }
        public string TokenHash { get; } = string.Empty;
        public DateTime CreatedAt { get; }
        public DateTime ExpiresAt { get; }
        public DateTime? RevokedAt { get; }

        public User User { get; }
    }
}
