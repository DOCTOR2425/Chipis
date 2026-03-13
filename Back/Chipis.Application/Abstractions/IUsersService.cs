using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IUsersService
    {
        Task<Guid> CreateUser(User user);
        Task<Guid> DeleteUser(Guid id);
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(Guid userId);
        Task<List<User>> SearchUsersByName(string userName);
        Task<Guid> UpdateUser(Guid userId, string userName, string hashPassword);
    }
}