namespace Chipis.API.DTOs
{
    public class MessageResponse
    {
        public MessageResponse(Guid messageId, Guid senderId, string text, DateTime sentAt)
        {
            MessageId = messageId;
            SenderId = senderId;
            Text = text;
            SentAt = sentAt;
        }

        public Guid MessageId { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
