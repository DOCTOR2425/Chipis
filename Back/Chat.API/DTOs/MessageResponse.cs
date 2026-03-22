namespace Chipis.API.DTOs
{
    public class MessageResponse
    {
        public MessageResponse(Guid messageResponseId, Guid senderId, string text, DateTime sentAt)
        {
            MessageResponseId = messageResponseId;
            SenderId = senderId;
            Text = text;
            SentAt = sentAt;
        }

        public Guid MessageResponseId { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
    }
}
