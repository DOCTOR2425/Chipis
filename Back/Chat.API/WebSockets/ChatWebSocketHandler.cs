using Chipis.API.DTOs;
using Chipis.Application.DTOs;
using Chipis.Application.Abstractions;
using Chipis.Core.Models;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;

namespace Chipis.API.WebSockets
{
    public class ChatWebSocketHandler
    {
        private readonly Dictionary<Guid, List<WebSocket>> _rooms = new();
        private readonly IMessagesService _messagesService;

        public ChatWebSocketHandler(IMessagesService messagesService)
        {
            _messagesService = messagesService;
        }

        public async Task HandleAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            var buffer = new byte[4 * 1024];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    RemoveSocketFromAllRooms(socket);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    break;
                }

                var json = Encoding.UTF8.GetString(buffer, 0, result.Count);

                var request = JsonSerializer.Deserialize<CreateMessageRequest>(json);
                if (request is null)
                    continue;

                CreateMessageCommand command = new CreateMessageCommand
                (
                    request.ChatId,
                    request.SenderId,
                    request.Text
                );

                Message savedMessage = await _messagesService.SaveNewMessage(command);

                AddClientToRoom(savedMessage.Chat.ChatId, socket);

                var outgoing = new
                {
                    messageId = savedMessage.MessageId,
                    text = savedMessage.Text,
                    date = savedMessage.Date,
                    chatId = savedMessage.Chat.ChatId,
                    senderId = savedMessage.Sender.UserId,
                    senderName = savedMessage.Sender.Nickname
                };

                var outgoingJson = JsonSerializer.Serialize(outgoing);

                await BroadcastToRoomAsync(savedMessage.Chat.ChatId, outgoingJson);
            }
        }

        private void AddClientToRoom(Guid chatId, WebSocket socket)
        {
            if (!_rooms.ContainsKey(chatId))
                _rooms[chatId] = new List<WebSocket>();

            if (!_rooms[chatId].Contains(socket))
                _rooms[chatId].Add(socket);
        }

        private void RemoveSocketFromAllRooms(WebSocket socket)
        {
            foreach (var room in _rooms.Values)
                room.Remove(socket);
        }

        private async Task BroadcastToRoomAsync(Guid chatId, string json)
        {
            if (!_rooms.ContainsKey(chatId))
                return;

            var bytes = Encoding.UTF8.GetBytes(json);

            foreach (var client in _rooms[chatId])
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(
                        bytes,
                        WebSocketMessageType.Text,
                        endOfMessage: true,
                        cancellationToken: CancellationToken.None);
                }
            }
        }
    }
}
