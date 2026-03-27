namespace Chipis.DataAccess.Entities
{
    public class MessageEntity
    {
        public Guid MessageEntityId { get; set; }
        public string Text { get; set;  } = string.Empty;
        public DateTime SentAt { get; set; }
        public bool IsChanged { get; set; }

        public ChatEntity ChatEntity { get; set; }
        public UserEntity Sender { get; set; }
    }
}
