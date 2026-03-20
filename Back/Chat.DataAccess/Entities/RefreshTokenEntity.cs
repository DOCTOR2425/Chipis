namespace Chipis.DataAccess.Entities
{
    public class RefreshTokenEntity
    {
        public Guid RefreshTokenEntityId {  get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime? RevokedAt { get; set; }

        public UserEntity UserEntity { get; set; }
    }
}
