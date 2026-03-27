using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess.Repositories
{
    public class ChatMembersRepository : IChatMembersRepository
    {
        private readonly ChipisDbContext _context;

        public ChatMembersRepository(ChipisDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(ChatMember chatMember)
        {
            ChatMemberEntity entity = new ChatMemberEntity()
            {
                ChatMemberEntityId = chatMember.ChatMemberId,
                ChatEntity = await _context.ChatEntity
                    .FindAsync(chatMember.Chat.ChatId),
                UserEntity = await _context.UserEntity
                    .FindAsync(chatMember.User.UserId),
            };

            await _context.ChatMemberEntity.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ChatMemberEntityId;
        }

        public async Task AddMembers(List<ChatMember> chatMembers)
        {
            List<ChatMemberEntity> entities = new List<ChatMemberEntity>();
            foreach (ChatMember member in chatMembers)
            {
                entities.Add(new ChatMemberEntity
                {
                    ChatMemberEntityId = member.ChatMemberId,
                    ChatEntity = await _context.ChatEntity
                        .FindAsync(member.Chat.ChatId),
                    UserEntity = await _context.UserEntity
                        .FindAsync(member.User.UserId),
                });
            }

            await _context.ChatMemberEntity.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ChatMember>> GetByChat(Guid chatId)
        {
            List<ChatMemberEntity> entities = await _context.ChatMemberEntity
                .Include(m => m.UserEntity)
                .Include(m => m.ChatEntity)
                .Where(m => m.ChatEntity.ChatEntityId == chatId)
                .AsNoTracking()
                .ToListAsync();

            return entities
                .Select(e => new ChatMember(
                    e.ChatMemberEntityId,
                    new User(
                        e.UserEntity.UserEntityId,
                        e.UserEntity.Nickname,
                        e.UserEntity.Telephone,
                        e.UserEntity.HashPassword),
                    new Chat(
                        e.ChatEntity.ChatEntityId,
                        e.ChatEntity.Name)))
                .ToList();
        }
    }
}
