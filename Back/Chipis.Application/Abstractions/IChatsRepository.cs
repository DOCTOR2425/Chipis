using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IChatsRepository
    {
        Task<Chat> GetById(Guid Id);
        Task<Chat> GetByName(string name);
        Task<List<Chat>> GetChats();
    }
}