using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NAP_F24_ConferenceApp_Server
{
    public class UdpStreamHandler
    {
        private readonly UdpClient udpServer;
        private readonly RoomManager roomManager;

        public UdpStreamHandler(int port, RoomManager manager)
        {
            udpServer = new UdpClient(port);
            roomManager = manager;
        }

        public async Task StartAsync()
        {
            Console.WriteLine("UDP Streaming Server started.");

            while (true)
            {
                try
                {
                    var result = await udpServer.ReceiveAsync();
                    byte[] mediaData = result.Buffer;
                    IPEndPoint senderEndpoint = result.RemoteEndPoint;

                    // تحديد غرفة العميل المرسل
                    string senderRoom = roomManager.FindRoomByClient(senderEndpoint);

                    // إذا العميل جديد ولم يتم تسجيله بعد
                    if (senderRoom == null)
                    {
                        // البحث في الغرف لتعيين الـUdpEndPoint لأول مرة
                        foreach (var roomName in roomManager.GetAllRooms())
                        {
                            foreach (var client in roomManager.GetClientsInRoom(roomName))
                            {
                                if (client.UdpEndPoint == null)
                                {
                                    client.UdpEndPoint = senderEndpoint;
                                    senderRoom = roomName;
                                    break;
                                }
                            }
                            if (senderRoom != null) break;
                        }
                    }

                    // إذا لم يتم تحديد الغرفة، تجاهل البيانات
                    if (senderRoom == null)
                        continue;

                    // إعادة البث لكل العملاء الآخرين في نفس الغرفة
                    foreach (var client in roomManager.GetClientsInRoom(senderRoom))
                    {
                        if (client.UdpEndPoint != null && !client.UdpEndPoint.Equals(senderEndpoint))
                        {
                            await udpServer.SendAsync(mediaData, mediaData.Length, client.UdpEndPoint);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UDP error: {ex.Message}");
                }
            }
        }
    }
}

