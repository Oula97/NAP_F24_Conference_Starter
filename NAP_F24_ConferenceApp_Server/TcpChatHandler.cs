using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace NAP_F24_ConferenceApp_Server
{
    public class TcpChatHandler
    {
        private readonly TcpListener listener;
        private readonly RoomManager roomManager;

        public TcpChatHandler(int port, RoomManager manager)
        {
            listener = new TcpListener(IPAddress.Any, port);
            roomManager = manager;
        }

        public async Task StartAsync()
        {
            listener.Start();
            Console.WriteLine($"TCP Chat Server started on port {((IPEndPoint)listener.LocalEndpoint).Port}");

            while (true)
            {
                try
                {
                    TcpClient client = await listener.AcceptTcpClientAsync();
                    Console.WriteLine("🟢 New client connected.");

                    _ = HandleClientAsync(client);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Error accepting client: {ex.Message}");
                }
            }
        }

        public async Task HandleClientAsync(TcpClient client)
        {
            string roomName = "General"; // الغرفة الافتراضية

            // إنشاء ClientInfo مع اسم عشوائي
            var clientInfo = new ClientInfo
            {
                TcpClient = client,
                UserName = "User" + Guid.NewGuid().ToString("N").Substring(0, 5)
            };

            // إضافة العميل إلى الغرفة
            roomManager.AddClientToRoom(roomName, client);

            using NetworkStream stream = client.GetStream();

            // 🔹 إرسال رسالة ترحيب فقط للعميل نفسه
            string welcomeMsg = $"Welcome to room '{roomName}'";
            byte[] welcomeData = Encoding.UTF8.GetBytes(welcomeMsg);
            await stream.WriteAsync(welcomeData, 0, welcomeData.Length);

            // 🔹 إعلام بقية المستخدمين بانضمامه
            string joinMsg = $"🔔 {clientInfo.UserName} joined the room.";
            byte[] joinData = Encoding.UTF8.GetBytes(joinMsg);
            foreach (var otherClient in roomManager.GetClientsInRoom(roomName))
            {
                if (otherClient != clientInfo && otherClient.TcpClient.Connected)
                {
                    var otherStream = otherClient.TcpClient.GetStream();
                    await otherStream.WriteAsync(joinData, 0, joinData.Length);
                }
            }

            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // العميل قطع الاتصال

                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // طباعة على الكونسول للخادم
                    Console.WriteLine($"[{roomName}] {clientInfo.UserName}: {message}");

                    string formattedMessage = $"[{clientInfo.UserName}] {message}";

                    // إرسال الرسالة لكل العملاء ما عدا المرسل
                    foreach (var otherClient in roomManager.GetClientsInRoom(roomName))
                    {
                        if (otherClient.TcpClient != client && otherClient.TcpClient.Connected)
                        {
                            var otherStream = otherClient.TcpClient.GetStream();
                            byte[] data = Encoding.UTF8.GetBytes(formattedMessage);
                            await otherStream.WriteAsync(data, 0, data.Length);
                        }
                    }
                }

            }
            catch (IOException)
            {
                Console.WriteLine("⚠️ Client forcibly disconnected.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ TCP Error: {ex.Message}");
            }
            finally
            {
                roomManager.RemoveClientFromRoom(roomName, client);
                client.Close();

                // إعلام الآخرين أنه غادر
                string leaveMsg = $"❌ {clientInfo.UserName} left the room.";
                byte[] leaveData = Encoding.UTF8.GetBytes(leaveMsg);
                foreach (var otherClient in roomManager.GetClientsInRoom(roomName))
                {
                    if (otherClient.TcpClient.Connected)
                    {
                        var otherStream = otherClient.TcpClient.GetStream();
                        await otherStream.WriteAsync(leaveData, 0, leaveData.Length);
                    }
                }
            }
        }



    }
}
