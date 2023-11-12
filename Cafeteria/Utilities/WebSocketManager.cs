using System.Net.WebSockets;

namespace Cafeteria.Utilities
{
    public static class WebSocketManager
    {
        // Listas para armazenar as conexões WebSocket
        public static List<WebSocket> ListWs { get; } = new List<WebSocket>();

        // Métodos para adicionar, remover ou realizar outras operações nas listas
        public static void AdicionarWs(WebSocket webSocket)
        {
            ListWs.Add(webSocket);
        }

        public static void RemoverWs(WebSocket webSocket)
        {
            ListWs.Remove(webSocket);
        }
    }
}
