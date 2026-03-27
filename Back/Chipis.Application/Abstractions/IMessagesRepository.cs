using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IMessagesRepository
    {
        Task<Guid> Create(Message message);
        Task<Guid> Delete(Guid messageId);
        Task<List<Message>> GetMessagesByChatId(
            Guid chatId,
            int take,
            Guid? cursorId);
        Task<List<Message>> SearchMessages(Guid chatId, string text);
        Task<List<Message>> SearchMessagesAsSinglWord(Guid chatId, string text);
    }
}