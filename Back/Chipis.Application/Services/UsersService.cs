using Chipis.Core.Abstractions;
using Chipis.Core.Models;

namespace Chipis.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;

        public UsersService(IUsersRepository _usersRepository)
        {
            this._usersRepository = _usersRepository;
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _usersRepository.GetAll();
        }

        public async Task<Guid> CreateUser(User user)
        {
            return await _usersRepository.Create(user);
        }

        public async Task<Guid> DeleteUser(Guid id)
        {
            return await _usersRepository.Delete(id);
        }

        public async Task<Guid> UpdateUser(Guid userId, string userName, string hashPassword)
        {
            return await _usersRepository.Update(userId, userName, hashPassword);
        }

        public async Task<List<User>> SearchUsersByName(string userName)
        {
            return await _usersRepository.SearchUsersByName(userName);
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _usersRepository.GetUserById(userId);
        }
    }
}
