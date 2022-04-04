using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ws.client.Managers
{
    public class WsClientManager
    {
        public static async Task Execute()
        {
            Console.WriteLine("press enter to continue!");
            Console.ReadKey();
            using (ClientWebSocket client = new ClientWebSocket())
            {
                Uri serviceUri = new Uri("ws://localhost:5000/send");
                var cTs = new CancellationTokenSource();
                cTs.CancelAfter(TimeSpan.FromSeconds(120));
                try
                {
                    await client.ConnectAsync(serviceUri, cTs.Token);
                    while (client.State == WebSocketState.Open)
                    {
                        Console.WriteLine("Enter message to send");
                        var message = Console.ReadLine();
                        if (!string.IsNullOrEmpty(message))
                        {
                            ArraySegment<byte> byteToSend = new ArraySegment<byte>(Encoding.UTF8.GetBytes(message));
                            await client.SendAsync(byteToSend, WebSocketMessageType.Text, true, cTs.Token);

                            while (true)
                            {
                                ArraySegment<byte> byteReceived = new ArraySegment<byte>(ConfigConstants.RESPONSE_BUFFER, ConfigConstants.OFFSET, ConfigConstants.PACKET);
                                WebSocketReceiveResult response = await client.ReceiveAsync(byteReceived, cTs.Token);
                                var responseMessage = Encoding.UTF8.GetString(ConfigConstants.RESPONSE_BUFFER, ConfigConstants.OFFSET, response.Count);
                                Console.WriteLine(responseMessage);
                                if (response.EndOfMessage)
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
                catch (WebSocketException e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            Console.ReadKey();
        } 
    }
}
