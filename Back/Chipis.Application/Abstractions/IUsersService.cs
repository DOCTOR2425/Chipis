using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IUsersService
    {
        Task<Guid> CreateUser(User user);
        Task<Guid> DeleteUser(Guid id);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(Guid userId);
        Task<List<User>> SearchUsersByNickname(string nickname);
        Task<Guid> UpdateUser(
            Guid userId,
            string nickname,
            string telephone,
            string hashPassword);
        Task<(string accessToken, string refreshToken)> 
            Login(string telephone, string password);
        Task<(string accessToken, string refreshToken)> 
            RegisterUser(string nickname, string telephone, string password);
    }
}