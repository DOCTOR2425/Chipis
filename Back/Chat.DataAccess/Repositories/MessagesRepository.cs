using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Chipis.DataAccess.Entities;

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
                Date = DateTime.Now,
                ChatEntity = await _context.ChatEntity.FindAsync(message.Chat.ChatId),
                Sender = await _context.UserEntity.FindAsync(message.Sender.UserId)
            };

            await _context.MessageEntity.AddAsync(messageEntity);
            await _context.SaveChangesAsync();

            return messageEntity.MessageEntityId;
        }
    }
}
