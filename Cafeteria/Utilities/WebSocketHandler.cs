using System.Net.WebSockets;

namespace Cafeteria.Utilities
{
    public class WebSocketHandler
    {
        private readonly RequestDelegate _next;

        public WebSocketHandler(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                var socket = await context.WebSockets.AcceptWebSocketAsync();
                await HandleWebSocket(socket);
            }
            else
            {
                await _next(context);
            }
        }

        private async Task HandleWebSocket(WebSocket socket)
        {
            // Lógica para manipular mensagens WebSocket
            // Exemplo: Echo Server
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result;

            do
            {
                result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Text)
                {
                    await socket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
                }

            } while (!result.CloseStatus.HasValue);

            await socket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
}
