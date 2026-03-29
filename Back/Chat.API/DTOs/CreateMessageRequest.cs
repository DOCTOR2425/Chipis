namespace Chipis.API.DTOs
{
    public class CreateMessageRequest
    {
        public CreateMessageRequest()
        {
        }

        public CreateMessageRequest(Guid chatId, string text)
        {
            ChatId = chatId;
            Text = text;
        }

        public Guid ChatId { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}
