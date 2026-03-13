using Chipis.Core.Models;
using Chipis.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Chipis.DataAccess.Repositories
{
    public class ChatsRepository
    {
        private readonly ChipisDbContext _context;

        public ChatsRepository(ChipisDbContext context)
        {
            _context = context;
        }


        public async Task<List<Chat>> GetChats()
        {
            List<ChatEntity> chatstities = await _context.ChatEntity
                .AsNoTracking()
                .ToListAsync();
            
            List<Chat> chats = chatstities.Select(c=> new Chat(c.ChatEntityId, c.Name))
                .ToList();

            return chats;
        }
    }
}
