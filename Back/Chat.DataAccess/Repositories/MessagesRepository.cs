using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly ChipisDbContext _context;

        public MessagesRepository(ChipisDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Create(Message message)
        {
            MessageEntity messageEntity = new MessageEntity
            {
                MessageEntityId = message.MessageId,
                Text = message.Text,
                SentAt = DateTime.Now,
                ChatEntity = await _context.ChatEntity.FindAsync(message.Chat.ChatId),
                Sender = await _context.UserEntity.FindAsync(message.Sender.UserId)
            };

            await _context.MessageEntity.AddAsync(messageEntity);
            await _context.SaveChangesAsync();

            return messageEntity.MessageEntityId;
        }

        public async Task<Guid> Delete(Guid messageId)
        {
            await _context.MessageEntity
                .Where(m => m.MessageEntityId == messageId)
                .ExecuteDeleteAsync();

            return messageId;
        }

        public async Task<Guid> Change(Guid messageId, string text)
        {
            await _context.MessageEntity
                .ExecuteUpdateAsync(m => m
                .SetProperty(e => e.IsChanged, true)
                .SetProperty(e => e.Text, text));

            return messageId;
        }

        public async Task<List<Message>> GetMessagesByChatId(
            Guid chatId,
            int take,
            Guid? cursorId)
        {
            IQueryable<MessageEntity> query = _context.MessageEntity
                .Include(m => m.ChatEntity)
                .Include(m => m.Sender)
                .Where(m => m.ChatEntity.ChatEntityId == chatId)
                .AsNoTracking();

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

        public async Task<List<Message>> SearchMessages(Guid chatId, string text)
        {
            List<MessageEntity> entities = await _context.MessageEntity
                .Include(m => m.ChatEntity)
                .Include(m => m.Sender)
                .Where(m => 
                    m.Text.Contains(text) && 
                    m.ChatEntity.ChatEntityId == chatId)
                .ToListAsync();

            return entities
                .Select(m => new Message(
                    m.MessageEntityId,
                    m.Text,
                    m.SentAt,
                    new Chat(m.ChatEntity.ChatEntityId, m.ChatEntity.Name),
                    new User(m.Sender.UserEntityId, m.Sender.Nickname, m.Sender.Telephone, m.Sender.HashPassword)))
                .ToList();
        }

        public async Task<List<Message>> SearchMessagesAsSinglWord(Guid chatId, string text)
        {
            List<MessageEntity> entities = await _context.MessageEntity
                .Include(m => m.ChatEntity)
                .Include(m => m.Sender)
                .Where(m =>
                    (EF.Functions.Like(m.Text, $"%[^a-zA-Zа-яА-Я]{text}[^a-zA-Zа-яА-Я]%") ||
                    EF.Functions.Like(m.Text, $"{text}[^a-zA-Zа-яА-Я]%") ||
                    EF.Functions.Like(m.Text, $"%[^a-zA-Zа-яА-Я]{text}") ||
                    m.Text == text) &&
                    m.ChatEntity.ChatEntityId == chatId)
                .ToListAsync();

            return entities
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
