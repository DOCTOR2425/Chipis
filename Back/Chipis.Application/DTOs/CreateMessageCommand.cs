namespace Chipis.Application.DTOs
{
    public class CreateMessageCommand
    {
        public CreateMessageCommand(Guid chatId, Guid senderId, string text)
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
