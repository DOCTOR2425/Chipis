using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess
{
    public class ChipisDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@$"Server=DESKTOP-CA83TSD;Database=ChipisDB;
				Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;");
            //.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }

        public DbSet<ChatEntity> ChatEntity { get; set; }
        public DbSet<ChatMemberEntity> ChatMemberEntity { get; set; }
        public DbSet<MessageEntity> MessageEntity { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokenEntity { get; set; }
        public DbSet<UserEntity> UserEntity { get; set; }
    }
}
