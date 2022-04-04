using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ws.client.Managers;

namespace ws.client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            await WsClientManager.Execute();
        }
    }
}
