using Chipis.Application.DTOs;
using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IMessagesService
    {
        Task<Message> SaveNewMessage(CreateMessageCommand command);
    }
}