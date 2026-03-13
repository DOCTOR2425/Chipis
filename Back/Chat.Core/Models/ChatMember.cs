
namespace Chipis.Core.Models
{
    public class ChatMember
    {
        public ChatMember(Guid chatMemberId, User user, Chat chat)
        {
            ChatMemberId = chatMemberId;
            User = user;
            Chat = chat;
        }

        public Guid ChatMemberId { get; }
        public User User { get; }
        public Chat Chat { get; }
    }
}
