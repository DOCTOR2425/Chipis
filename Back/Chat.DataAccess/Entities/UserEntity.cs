using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess.Entities
{
    [Index(nameof(Telephone), IsUnique = true)]
    public class UserEntity
    {
        public Guid UserEntityId { get; set; }
        public string Nickname { get; set; } = string.Empty;
        public string Telephone { get; set; } = string.Empty;
        public string HashPassword { get; set; } = string.Empty;
    }
}
