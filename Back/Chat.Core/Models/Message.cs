namespace Chipis.Core.Models
{
    public class Message
    {
        public Message(Guid messageId, string text, DateTime date, Chat chat, User user)
        {
            MessageId = messageId;
            Text = text;
            Date = date;
            Chat = chat;
            Sender = user;
        }

        public Guid MessageId { get; }
        public string Text { get; } = string.Empty;
        public DateTime Date { get; }

        public Chat Chat { get; }
        public User Sender { get; }
    }
}
