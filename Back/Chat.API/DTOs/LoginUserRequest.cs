namespace Chipis.API.DTOs
{
    public class LoginUserRequest
    {
        public LoginUserRequest(string telephone, string password)
        {
            Telephone = telephone;
            Password = password;
        }

        public string Telephone { get; set; }= string.Empty;
        public string Password { get; set; }= string.Empty;
    }
}
