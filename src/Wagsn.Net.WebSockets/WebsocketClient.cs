using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Wagsn.Net.WebSockets
{
    public class WebsocketClient
    {
        public WebSocket WebSocket { get; set; }

        public string Id { get; set; }

        public Task SendMessageAsync(WebsocketMessage message)
        {
            var msg = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
            return WebSocket.SendAsync(new ArraySegment<byte>(msg, 0, msg.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}
