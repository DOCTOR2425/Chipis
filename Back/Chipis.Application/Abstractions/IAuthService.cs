namespace Chipis.Application.Abstractions
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken)> 
            Login(string telephone, string password);
        Task<(string accessToken, string refreshToken)> 
            RegisterUser(string nickname, string telephone, string password);
        Task<(string accessToken, string refreshToken)>
            RefreshTokens(string refreshToken);
    }
}