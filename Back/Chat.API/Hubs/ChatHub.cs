using Chipis.API.DTOs;
using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Chipis.API.Hubs
{
    public interface IChatClient
    {
        public Task ReceiveMessage(MessageResponse outgoing);
        public Task OnIncomingChat(string incom);
    }

    [Authorize]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly IMessagesService _messagesService;

        public ChatHub(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task JoinChat(Guid chatId)
        {
            Console.WriteLine($"[HUB] {Context.ConnectionId} вошёл в чат {chatId}");
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
            await Clients.Group(chatId.ToString())
                .OnIncomingChat($"[HUB] {Context.ConnectionId} вошёл в чат {chatId}");
        }

        public async Task SendMessage(CreateMessageRequest command)
        {
            Guid senderId = Guid.Parse(Context.UserIdentifier);

            Message saved = await _messagesService.SaveNewMessage(
                command.Text, 
                command.ChatId,
                senderId);

            MessageResponse outgoing = new MessageResponse(
                saved.MessageId,
                saved.Sender.UserId,
                saved.Text,
                saved.SentAt,
                false,
                false);

            await Clients.Group(saved.Chat.ChatId.ToString()).ReceiveMessage(outgoing);
        }
    }
}
