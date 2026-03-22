namespace Chipis.API.DTOs
{
    public class ChatResponse
    {
        public ChatResponse()
        {
        }

        public ChatResponse(Guid chatId, string name)
        {
            ChatId = chatId;
            Name = name;
        }

        public Guid ChatId { get; }
        public string Name { get; } = string.Empty;
    }
}
