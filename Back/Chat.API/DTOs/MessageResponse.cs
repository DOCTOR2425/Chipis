namespace Chipis.API.DTOs
{
    public class MessageResponse
    {
        public MessageResponse(Guid messageId, Guid senderId, string text, DateTime sentAt, bool isChanged, bool isReaded)
        {
            MessageId = messageId;
            SenderId = senderId;
            Text = text;
            SentAt = sentAt;
            IsChanged = isChanged;
            Status = isReaded;
        }

        public Guid MessageId { get; set; }
        public Guid SenderId { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsChanged { get; set; }
        public bool Status { get; set; }
    }
}
