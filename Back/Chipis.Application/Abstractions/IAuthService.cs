using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IAuthService
    {
        Task<(string accessToken, string refreshToken, User user)> 
            Login(string telephone, string password);
        Task<(string accessToken, string refreshToken, User user)> 
            RegisterUser(string nickname, string telephone, string password);
        Task<(string accessToken, string refreshToken)>
            RefreshTokens(string refreshToken);
    }
}