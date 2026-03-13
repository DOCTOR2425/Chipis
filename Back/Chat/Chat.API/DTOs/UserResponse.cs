namespace Chipis.API.DTOs
{
    public class UserResponse
    {
        public UserResponse(Guid userId, string name, string hashPassword)
        {
            UserId = userId;
            Name = name;
            HashPassword = hashPassword;
        }

        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
    }
}
