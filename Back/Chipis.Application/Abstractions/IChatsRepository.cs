using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IChatsRepository
    {
        Task<Chat> GetById(Guid Id);
        Task<Chat> GetByName(string name);
        Task<List<Chat>> GetChats();
        Task<List<Message>> GetAllMessagesByChatId(Guid chatId);
        Task<List<Message>> GetMessagesByChatId(
            Guid chatId,
            int take,
            Guid? cursorId);
        Task<List<Chat>> GetChatsByUser(Guid userId);
        Task<Guid> Create(Chat chat);
    }
}