namespace Chipis.DataAccess.Entities
{
    public class MessageEntity
    {
        public Guid MessageEntityId { get; set; }
        public string Text { get; set;  } = string.Empty;
        public DateTime Date { get; }

        public ChatEntity ChatEntity { get; }
        public UserEntity Sender { get; }
    }
}
