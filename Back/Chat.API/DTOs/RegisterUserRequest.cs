namespace Chipis.API.DTOs
{
    public class RegisterUserRequest
    {
        public RegisterUserRequest()
        {
        }

        public RegisterUserRequest(string name, string telephone, string password)
        {
            Nickname = name;
            Telephone = telephone;
            Password = password;
        }

        public string Nickname { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
