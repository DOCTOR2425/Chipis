namespace Chipis.Core.Models
{
    public class User
    {
        public User(Guid userId, string name, string hashPassword)
        {
            UserId = userId;
            Name = name;
            HashPassword = hashPassword;
        }

        public Guid UserId { get; }
        public string Name { get; } = string.Empty;
        public string HashPassword { get; } = string.Empty;
    }
}
