using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IUsersRepository
    {
        Task<Guid> Create(User user);
        Task<Guid> Delete(Guid userId);
        Task<List<User>> GetAll();
        Task<Guid> Update(Guid userId, string name, string hasPassword);
        Task<User> GetById(Guid userId);
        Task<List<User>> SearchUsersByName(string userName);
    }
}