using Chipis.Application.Abstractions;
using Chipis.Core.Models;

namespace Chipis.Application.Services
{
    public class UsersService : IUsersService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHashProvider _hashProvider;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokensRepository _refreshTokensRepository;

        public UsersService(
            IUsersRepository usersRepository,
            IHashProvider hashProvider,
            IJwtProvider jwtProvider,
            IRefreshTokensRepository refreshTokensRepository)
        {
            _usersRepository = usersRepository;
            _hashProvider = hashProvider;
            _jwtProvider = jwtProvider;
            _refreshTokensRepository = refreshTokensRepository;
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

        public async Task<Guid> UpdateUser(
            Guid userId,
            string nickname,
            string telephone,
            string hashPassword)
        {
            return await _usersRepository.Update(userId, nickname, telephone, hashPassword);
        }

        public async Task<List<User>> SearchUsersByNickname(string nickname)
        {
            return await _usersRepository.SearchUsersByNickname(nickname);
        }

        public async Task<User> GetUserById(Guid userId)
        {
            return await _usersRepository.GetById(userId);
        }
    }
}
