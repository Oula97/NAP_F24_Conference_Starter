using System;
using System.Threading;

namespace NAP_F24_ConferenceApp_Server
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var roomManager = new RoomManager();

            var tcpServer = new TcpChatHandler(5000, roomManager);
            var udpServer = new UdpStreamHandler(6000, roomManager);

            _ = tcpServer.StartAsync();
            _ = udpServer.StartAsync();

            Console.WriteLine("Servers running. Press Enter to exit.");
            Console.ReadLine();
        }
    }
}
