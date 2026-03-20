namespace Chipis.Core.Models
{
    public class User
    {
        public User(Guid userId, string nickname, string telephone, string hashPassword)
        {
            UserId = userId;
            Nickname = nickname;
            Telephone = telephone;
            HashPassword = hashPassword;
        }

        public Guid UserId { get; }
        public string Nickname { get; } = string.Empty;
        public string Telephone { get; } = string.Empty;
        public string HashPassword { get; } = string.Empty;
    }
}
