namespace Chipis.Core.Models
{
    public class Message
    {
        public Message(Guid messageId, string text, DateTime sentAt, bool isChanged, bool isReaded, Chat chat, User sender)
        {
            MessageId = messageId;
            Text = text;
            SentAt = sentAt;
            IsChanged = isChanged;
            IsReaded = isReaded;
            Chat = chat;
            Sender = sender;
        }

        public Guid MessageId { get; }
        public string Text { get; } = string.Empty;
        public DateTime SentAt { get; }
        public bool IsChanged { get; set; } = false;
        public bool IsReaded { get; set; }

        public Chat Chat { get; }
        public User Sender { get; }
    }
}
