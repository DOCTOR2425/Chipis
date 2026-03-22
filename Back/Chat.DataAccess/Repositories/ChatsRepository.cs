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
                .ToListAsync();

            return messageEntities
                .Select(m => new Message(
                    m.MessageEntityId,
                    m.Text,
                    m.SentAt,
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

        public async Task<List<Message>> GetMessagesByChatId(
            Guid chatId,
            int take,
            Guid? cursorId)
        {
            IQueryable<MessageEntity> query = _context.MessageEntity
                .Include(m => m.ChatEntity)
                .Include(m => m.Sender)
                .Where(m => m.ChatEntity.ChatEntityId == chatId);

            if (cursorId.HasValue)
            {
                MessageEntity? cursorMessage = await _context.MessageEntity
                    .FirstOrDefaultAsync(m => m.MessageEntityId == cursorId);

                if (cursorMessage != null)
                {
                    query = query.Where(m => m.SentAt < cursorMessage.SentAt);
                }
            }

            List<MessageEntity> messageEntities = await query
                .OrderByDescending(m => m.SentAt)
                .Take(take)
                .ToListAsync();

            return messageEntities
                .Select(m => new Message(
                    m.MessageEntityId,
                    m.Text,
                    m.SentAt,
                    new Chat(m.ChatEntity.ChatEntityId, m.ChatEntity.Name),
                    new User(m.Sender.UserEntityId, m.Sender.Nickname, m.Sender.Telephone, m.Sender.HashPassword)))
                .ToList();
        }
    }
}
