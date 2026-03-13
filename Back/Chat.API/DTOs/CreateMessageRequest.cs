namespace Chipis.API.DTOs
{
    public class CreateMessageRequest
    {
        public CreateMessageRequest(Guid chatId, Guid senderId, string text)
        {
            ChatId = chatId;
            SenderId = senderId;
            Text = text;
        }

        public Guid ChatId { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
