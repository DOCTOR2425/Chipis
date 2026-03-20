using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IUsersRepository
    {
        Task<Guid> Create(User user);
        Task<Guid> Delete(Guid userId);
        Task<List<User>> GetAll();
        Task<Guid> Update(
            Guid userId,
            string nickname,
            string telephone,
            string hashPassword);
        Task<User> GetById(Guid userId);
        Task<bool> CheckByTelephone(string telephone);
        Task<User> GetByTelephone(string telephone);
        Task<List<User>> SearchUsersByNickname(string nickname);
    }
}