using Chipis.Application.DTOs;
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

        public async Task<Message> SaveNewMessage(CreateMessageCommand command)
        {
            Message message = new Message(
                Guid.NewGuid(),
                command.Text,
                DateTime.Now,
                await _chatsRepository.GetById(command.ChatId),
                await _usersRepository.GetById(command.SenderId));

            await _messagesRepository.Create(message);
            return message;
        }

        public async Task<Guid> DeleteMessage(Guid messageId)
        {
            return await _messagesRepository.Delete(messageId);
        }
    }
}
