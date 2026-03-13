namespace Chipis.DataAccess.Entities
{
    public class UserEntity
    {
        public Guid UserEntityId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
    }
}
