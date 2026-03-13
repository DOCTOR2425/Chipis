namespace Chipis.API.DTOs
{
    public class UserRequest
    {
        public UserRequest(string name, string hashPassword)
        {
            Name = name;
            HashPassword = hashPassword;
        }

        public string Name { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
    }
}
