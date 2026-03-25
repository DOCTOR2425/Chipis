namespace Chipis.API.DTOs
{
    public class UserResponse
    {
        public UserResponse(Guid userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        public Guid UserId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
