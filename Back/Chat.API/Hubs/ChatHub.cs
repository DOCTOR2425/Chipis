using Chipis.API.DTOs;
using Chipis.Application.Abstractions;
using Chipis.Application.DTOs;
using Chipis.Core.Models;
using Microsoft.AspNetCore.SignalR;

namespace Chipis.API.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IMessagesService _messagesService;

        public ChatHub(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task JoinChat(Guid chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId.ToString());
        }

        public async Task SendMessage(CreateMessageCommand command)
        {
            Message saved = await _messagesService.SaveNewMessage(command);

            MessageResponse outgoing = new MessageResponse(
                saved.MessageId,
                saved.Sender.UserId,
                saved.Text,
                saved.SentAt,
                false,
                false);

            // Рассылка всем в комнате
            await Clients.Group(saved.Chat.ChatId.ToString())
                .SendAsync("ReceiveMessage", outgoing);
        }
    }
}
