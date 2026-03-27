using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IChatsService
    {
        Task<List<Message>> GetAllMessagesByChatId(Guid chatId);
        Task<List<Message>> GetMessagesByChatId(
            Guid chatId,
            int take,
            Guid? cursorId);
        Task<List<Chat>> GetChatsByUser(Guid userId);
        Task<Chat> CreateChat(Guid userId1, Guid userId2);
        Task<List<Message>> SearchMessages(Guid chatId, string text, bool? isSinglWord);
    }
}