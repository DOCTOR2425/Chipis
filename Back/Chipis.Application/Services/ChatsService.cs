using Chipis.Application.Abstractions;
using Chipis.Core.Models;

namespace Chipis.Application.Services
{
    public class ChatsService : IChatsService
    {
        private readonly IChatsRepository _chatsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IChatMembersRepository _chatMembersRepository;
        private readonly IMessagesRepository _messagesRepository;

        public ChatsService(
            IChatsRepository chatsRepository,
            IUsersRepository usersRepository,
            IChatMembersRepository chatMembersRepository,
            IMessagesRepository messagesRepository)
        {
            _chatsRepository = chatsRepository;
            _usersRepository = usersRepository;
            _chatMembersRepository = chatMembersRepository;
            _messagesRepository = messagesRepository;
        }

        public async Task<List<Message>> GetAllMessagesByChatId(Guid chatId)
        {
            return await _chatsRepository.GetAllMessagesByChatId(chatId);
        }

        public async Task<List<Message>> GetMessagesByChatId(
            Guid chatId,
            int take,
            Guid? cursorId)
        {
            return await _messagesRepository.GetMessagesByChatId(chatId, take, cursorId);
        }

        public async Task<List<Chat>> GetChatsByUser(Guid userId)
        {
            return await _chatsRepository.GetChatsByUser(userId);
        }

        public async Task<Chat> CreateChat(Guid userId1, Guid userId2)
        {
            User user1 = await _usersRepository.GetById(userId1);
            User user2 = await _usersRepository.GetById(userId2);

            Chat chat = new Chat(Guid.NewGuid(), string.Empty);
            ChatMember chatMember1 = new ChatMember(Guid.NewGuid(), user1, chat);
            ChatMember chatMember2 = new ChatMember(Guid.NewGuid(), user2, chat);

            await _chatsRepository.Create(chat);
            await _chatMembersRepository.AddMembers(new List<ChatMember>()
            {
                chatMember1,
                chatMember2
            });

            return chat;
        }

        public async Task<List<Message>> SearchMessages(Guid chatId, string text, bool? isSinglWord)
        {
            if(isSinglWord.HasValue && isSinglWord == true)
                return await _messagesRepository.SearchMessagesAsSinglWord(chatId, text);
            return await _messagesRepository.SearchMessages(chatId, text);
        }
    }
}
