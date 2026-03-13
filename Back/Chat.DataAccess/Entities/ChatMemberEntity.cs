namespace Chipis.DataAccess.Entities
{
    public class ChatMemberEntity
    {
        public Guid ChatMemberEntityId { get; set; }
        public UserEntity UserEntity { get; set; }
        public ChatEntity ChatEntity { get; set; }
    }
}
