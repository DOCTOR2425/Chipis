using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IMessagesService
    {
        Task<Message> SaveNewMessage(string text, Guid chatId, Guid senderId);
        Task<Guid> DeleteMessage(Guid messageId);
    }
}