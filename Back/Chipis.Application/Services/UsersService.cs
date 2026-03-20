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

        public async Task<(string accessToken, string refreshToken)> 
            RegisterUser(string nickname, string telephone, string password)
        {
            if (await _usersRepository.CheckByTelephone(telephone))
                throw new InvalidOperationException();

            Guid userId = Guid.NewGuid();

            var (refreshToken, tokenHash, expiresAt) = _jwtProvider.GenerateRefreshToken(userId);
            string accessToken = _jwtProvider.GenerateAccessToken(userId);

            User user = new User(
                userId,
                nickname,
                telephone,
                _hashProvider.Generate(password));
            await _usersRepository.Create(user);

            RefreshToken token = new RefreshToken(
                Guid.NewGuid(),
                tokenHash,
                DateTime.Now,
                expiresAt,
                user);
            await _refreshTokensRepository.Create(token);

            return (accessToken, refreshToken);
        }

        public async Task<(string accessToken, string refreshToken)> 
            Login(string telephone, string password)
        {
            if (!await _usersRepository.CheckByTelephone(telephone))
                throw new UnauthorizedAccessException();

            User user = await _usersRepository.GetByTelephone(telephone);

            if (!_hashProvider.Verify(password, user.HashPassword))
                throw new UnauthorizedAccessException();

            var (refreshToken, tokenHash, expiresAt) = _jwtProvider.GenerateRefreshToken(user.UserId);
            string accessToken = _jwtProvider.GenerateAccessToken(user.UserId);
            RefreshToken token = new RefreshToken(
                Guid.NewGuid(),
                tokenHash,
                DateTime.Now,
                expiresAt,
                user);
            await _refreshTokensRepository.Create(token);

            return (accessToken, refreshToken);
        }
    }
}
