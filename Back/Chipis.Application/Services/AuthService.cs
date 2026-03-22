using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using System.Security;

namespace Chipis.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IHashProvider _hashProvider;
        private readonly IJwtProvider _jwtProvider;
        private readonly IRefreshTokensRepository _refreshTokensRepository;

        public AuthService(
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

        public async Task<(string accessToken, string refreshToken)>
            RefreshTokens(string refreshToken)
        {
            RefreshToken storedToken = await _refreshTokensRepository.GetByString(refreshToken);

            if (storedToken is null)
                throw new SecurityException("Invalid refresh token");

            if (storedToken.ExpiresAt < DateTime.UtcNow)
                throw new SecurityException("Refresh token expired");

            if (storedToken.RevokedAt is not null)
                throw new SecurityException("Refresh token revoked");

            User user = storedToken.User;

            var (newRawToken, newTokenHash, newExpiresAt) = _jwtProvider.GenerateRefreshToken(user.UserId);

            var newRefreshToken = new RefreshToken(
                Guid.NewGuid(),
                newTokenHash,
                DateTime.UtcNow,
                newExpiresAt,
                user);

            await _refreshTokensRepository.Create(newRefreshToken);
            await _refreshTokensRepository.Delete(storedToken.RefreshTokenId);

            string newAccessToken = _jwtProvider.GenerateAccessToken(user.UserId);

            return (newAccessToken, newRawToken);
        }
    }
}
