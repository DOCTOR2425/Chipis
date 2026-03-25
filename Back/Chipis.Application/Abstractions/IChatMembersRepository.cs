using Chipis.Core.Models;

namespace Chipis.Application.Abstractions
{
    public interface IChatMembersRepository
    {
        Task<Guid> Create(ChatMember chatMember);
        Task<List<ChatMember>> GetByChat(Guid chatId);
        Task AddMembers(List<ChatMember> chatMembers);
    }
}