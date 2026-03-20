using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly ChipisDbContext _context;

        public UsersRepository(ChipisDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAll()
        {
            List<UserEntity> userEntities = await _context.UserEntity
                .AsNoTracking()
                .ToListAsync();

            List<User> users = userEntities
                .Select(u => new User(u.UserEntityId, u.Nickname, u.Telephone, u.HashPassword))
                .ToList();

            return users;
        }

        public async Task<Guid> Create(User user)
        {
            UserEntity userEntity = new UserEntity
            {
                UserEntityId = user.UserId,
                Nickname = user.Nickname,
                Telephone = user.Telephone,
                HashPassword = user.HashPassword,
            };

            await _context.UserEntity.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.UserEntityId;
        }

        public async Task<User> GetById(Guid userId)
        {
            UserEntity userEntity = await _context.UserEntity
                .FindAsync(userId);

            return new User(
                userId,
                userEntity.Nickname,
                userEntity.Telephone,
                userEntity.HashPassword);
        }

        public async Task<bool> CheckByTelephone(string telephone)
        {
            UserEntity? userEntity = await _context.UserEntity
                .FirstOrDefaultAsync(u => u.Telephone == telephone);

            if(userEntity == null)
                return false;
            return true;
        }

        public async Task<User> GetByTelephone(string telephone)
        {
            UserEntity userEntity = await _context.UserEntity
                .FirstOrDefaultAsync(u => u.Telephone == telephone);

            return new User(
                userEntity.UserEntityId, 
                userEntity.Nickname, 
                telephone, 
                userEntity.HashPassword);
        }

        public async Task<List<User>> SearchUsersByNickname(string nickname)
        {
            List<UserEntity> usersEntity = await _context.UserEntity
                .OrderBy(u => u.Nickname)
                .Where(u => u.Nickname.Contains(nickname))
                .ToListAsync();

            return usersEntity
                .Select(u => new User(u.UserEntityId, u.Nickname, u.Telephone, u.HashPassword))
                .ToList();
        }

        public async Task<Guid> Update(
            Guid userId, 
            string nickname, 
            string telephone, 
            string hashPassword)
        {
            await _context.UserEntity
                .Where(u => u.UserEntityId == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Nickname, nickname)
                    .SetProperty(u => u.Telephone, telephone)
                    .SetProperty(u => u.HashPassword, hashPassword));

            return userId;
        }

        public async Task<Guid> Delete(Guid userId)
        {
            await _context.UserEntity
                .Where(u => u.UserEntityId == userId)
                .ExecuteDeleteAsync();

            return userId;
        }
    }
}
