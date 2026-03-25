using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IMessagesRepository
    {
        Task<Guid> Create(Message message);
        Task<Guid> Delete(Guid messageId);
    }
}