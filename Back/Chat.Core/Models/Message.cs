namespace Chipis.Core.Models
{
    public class Message
    {
        public Message(Guid messageId, string text, DateTime sentAt, Chat chat, User user)
        {
            MessageId = messageId;
            Text = text;
            SentAt = sentAt;
            Chat = chat;
            Sender = user;
        }

        public Guid MessageId { get; }
        public string Text { get; } = string.Empty;
        public DateTime SentAt { get; }

        public Chat Chat { get; }
        public User Sender { get; }
    }
}
