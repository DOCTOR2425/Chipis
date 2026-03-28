using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess.Repositories
{
    public class ChatsRepository : IChatsRepository
    {
        private readonly ChipisDbContext _context;

        public ChatsRepository(ChipisDbContext context)
        {
            _context = context;
        }

        public async Task<List<Chat>> GetChats()
        {
            List<ChatEntity> chatEntities = await _context.ChatEntity
                .AsNoTracking()
                .ToListAsync();

            List<Chat> chats = chatEntities.Select(c => new Chat(c.ChatEntityId, c.Name))
                .ToList();

            return chats;
        }

        public async Task<List<Message>> GetAllMessagesByChatId(Guid chatId)
        {
            List<MessageEntity> messageEntities = await _context.MessageEntity
                .Include(m => m.ChatEntity)
                .Include(m => m.Sender)
                .OrderBy(m => m.SentAt)
                .Where(m => m.ChatEntity.ChatEntityId == chatId)
                .AsNoTracking()
                .ToListAsync();

            return messageEntities
                .Select(m => new Message(
                    m.MessageEntityId,
                    m.Text,
                    m.SentAt,
                    m.IsChanged,
                    m.IsReaded,
                    new Chat(m.ChatEntity.ChatEntityId, m.ChatEntity.Name),
                    new User(m.Sender.UserEntityId, m.Sender.Nickname, m.Sender.Telephone, m.Sender.HashPassword)))
                .ToList();
        }

        public async Task<Chat> GetById(Guid Id)
        {
            ChatEntity chatEntity = await _context.ChatEntity.FindAsync(Id);

            return new Chat(Id, chatEntity.Name);
        }

        public async Task<Chat> GetByName(string name)
        {
            ChatEntity chatEntity = await _context.ChatEntity
                .FirstOrDefaultAsync(c => c.Name == name);

            return new Chat(chatEntity.ChatEntityId, name);
        }

        public async Task<List<Chat>> GetChatsByUser(Guid userId)
        {
            List<ChatEntity> chatEntities = await _context.ChatMemberEntity
                .Include(cm => cm.UserEntity)
                .Include(cm => cm.ChatEntity)
                .ThenInclude(c => c.Members)
                .ThenInclude(c => c.UserEntity)
                .Where(cm => cm.UserEntity.UserEntityId == userId)
                .Select(cm => cm.ChatEntity)
                .AsNoTracking()
                .ToListAsync();

            return chatEntities.Select(ce => MapToDomain(ce, userId)).ToList();
        }

        private Chat MapToDomain(ChatEntity entity, Guid currentUserId)
        {
            string name;

            if (entity.Members.Count == 2)
            {
                var other = entity.Members.First(m => m.UserEntity.UserEntityId != currentUserId);
                name = other.UserEntity.Nickname;
            }
            else
            {
                name = entity.Name;
            }

            return new Chat(entity.ChatEntityId, name);
        }

        public async Task<Guid> Create(Chat chat)
        {
            ChatEntity chatEntity = new ChatEntity()
            {
                ChatEntityId = chat.ChatId,
                Name = chat.Name,
            };

            await _context.ChatEntity.AddAsync(chatEntity);
            await _context.SaveChangesAsync();

            return chatEntity.ChatEntityId;
        }
    }
}
