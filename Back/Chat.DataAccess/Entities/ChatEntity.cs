namespace Chipis.DataAccess.Entities
{
    public class ChatEntity
    {
        public Guid ChatEntityId { get; set;  }
        public string Name { get; set; } = string.Empty;

        public ICollection<ChatMemberEntity> Members { get; set; }
    }
}
