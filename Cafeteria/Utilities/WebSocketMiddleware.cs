using Cafeteria.Models;
using Cafeteria.Services.Implementations;
using Cafeteria.Services.Interfaces;
using System.Net.WebSockets;
using System.Text;

namespace Cafeteria.Utilities
{
    public class WebSocketMiddleware
    {
        private readonly RequestDelegate _next;

        public WebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();

                string path = context.Request.Path;

                switch (path)
                {
                    case "/ws":
                        WebSocketManager.AdicionarWs(webSocket);
                        await ProcessarSolicitacaoWs(webSocket);
                        break;
                    default:
                        context.Response.StatusCode = 400;
                        await webSocket.CloseAsync(WebSocketCloseStatus.PolicyViolation, "Caminho de solicitação inválido", CancellationToken.None);
                        break;
                }
            }
            else
            {
                await _next(context);
            }
        }

        public async Task ProcessarSolicitacaoWs(WebSocket webSocket)
        {
            try
            {
                while (webSocket.State == WebSocketState.Open)
                {
                    var buffer = new byte[1024 * 4];
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                    // Lógica para processar mensagens recebidas do cliente
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string mensagem = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await EnviarMensagemParaWs(mensagem);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        break;
                    }
                }
            }
            finally
            {
                WebSocketManager.RemoverWs(webSocket);
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Conexão fechada!", CancellationToken.None);
            }
        }
        
        private async Task EnviarMensagemParaWs(string mensagem)
        {
            foreach (var clienteWebSocket in WebSocketManager.ListWs)
            {
                await EnviarMensagem(clienteWebSocket, mensagem);
            }
        }

        private async Task EnviarMensagem(WebSocket webSocket, string mensagem)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(mensagem);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
