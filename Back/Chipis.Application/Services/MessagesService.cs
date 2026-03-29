using Chipis.Application.Abstractions;
using Chipis.Core.Models;

namespace Chipis.Application.Services
{
    public class MessagesService : IMessagesService
    {
        private readonly IMessagesRepository _messagesRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IChatsRepository _chatsRepository;

        public MessagesService(
            IMessagesRepository messagesRepository,
            IUsersRepository usersRepository,
            IChatsRepository chatsRepository)
        {
            _messagesRepository = messagesRepository;
            _usersRepository = usersRepository;
            _chatsRepository = chatsRepository;
        }

        public async Task<Message> SaveNewMessage(string text, Guid chatId, Guid senderId)
        {
            Message message = new Message(
                Guid.NewGuid(),
                text,
                DateTime.Now,
                false,
                false,
                await _chatsRepository.GetById(chatId),
                await _usersRepository.GetById(senderId));

            await _messagesRepository.Create(message);
            return message;
        }

        public async Task<Guid> DeleteMessage(Guid messageId)
        {
            return await _messagesRepository.Delete(messageId);
        }
    }
}
