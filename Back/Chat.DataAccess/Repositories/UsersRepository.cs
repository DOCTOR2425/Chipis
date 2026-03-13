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
                .Select(u => new User(u.UserEntityId, u.Name, u.HashPassword))
                .ToList();

            return users;
        }

        public async Task<Guid> Create(User user)
        {
            UserEntity userEntity = new UserEntity
            {
                UserEntityId = user.UserId,
                Name = user.Name,
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
                userEntity.Name, 
                userEntity.HashPassword);
        }

        public async Task<List<User>> SearchUsersByName(string userName)
        {
            List<UserEntity> usersEntity = await _context.UserEntity
                .OrderBy(u => u.Name)
                .Where(u => u.Name.Contains(userName))
                .ToListAsync();

            return usersEntity
                .Select(u => new User(u.UserEntityId, u.Name, u.HashPassword))
                .ToList();
        }

        public async Task<Guid> Update(Guid userId, string name, string hasPassword)
        {
            await _context.UserEntity
                .Where(u => u.UserEntityId == userId)
                .ExecuteUpdateAsync(s => s
                    .SetProperty(u => u.Name, name)
                    .SetProperty(u => u.HashPassword, hasPassword));

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
