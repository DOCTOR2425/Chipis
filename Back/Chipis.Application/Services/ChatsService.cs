using Chipis.Application.Abstractions;
using Chipis.Core.Models;

namespace Chipis.Application.Services
{
    public class ChatsService : IChatsService
    {
        private readonly IChatsRepository _chatsRepository;

        public ChatsService(IChatsRepository chatsRepository)
        {
            this._chatsRepository = chatsRepository;
        }

        public async Task<List<Message>> GetAllMessagesByChatId(Guid chatId)
        {
            return await _chatsRepository.GetAllMessagesByChatId(chatId);
        }

        public async Task<List<Message>> GetMessagesByChatId(
            Guid chatId,
            int take,
            Guid? cursorId)
        {
            return await _chatsRepository.GetMessagesByChatId(chatId, take, cursorId);
        }
    }
}
