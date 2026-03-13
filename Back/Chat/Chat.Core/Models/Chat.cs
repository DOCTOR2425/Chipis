namespace Chipis.Core.Models
{
    public class Chat
    {
        public Chat(Guid chatId, string name)
        {
            ChatId = chatId;
            Name = name;
        }

        public Guid ChatId { get; }
        public string Name { get; } = string.Empty;
    }
}
