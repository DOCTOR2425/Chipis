using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IChatsService
    {
        Task<List<Message>> GetAllMessagesByChatId(Guid chatId);
    }
}