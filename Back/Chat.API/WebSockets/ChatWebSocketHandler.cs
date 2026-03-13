using System.Net.WebSockets;
using System.Text;

namespace Chipis.API.WebSockets
{
    public class ChatWebSocketHandler
    {
        private readonly List<WebSocket> _clients = new List<WebSocket>();

        public async Task HandleAsync(HttpContext context)
        {
            if (!context.WebSockets.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                return;
            }

            var socket = await context.WebSockets.AcceptWebSocketAsync();
            _clients.Add(socket);

            var buffer = new byte[1024 * 4];

            while (socket.State == WebSocketState.Open)
            {
                var result = await socket.ReceiveAsync(buffer, CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _clients.Remove(socket);
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closed", CancellationToken.None);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                await BroadcastMessageAsync(message);
            }
        }

        private async Task BroadcastMessageAsync(string message)
        {
            var bytes = Encoding.UTF8.GetBytes(message);

            foreach (var client in _clients)
            {
                if (client.State == WebSocketState.Open)
                {
                    await client.SendAsync(
                        bytes,
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None
                    );
                }
            }
        }
    }
}
