using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace NAP_F24_ConferenceApp_Server
{
    // ====================== ClientInfo ======================
    public class ClientInfo
    {
        public TcpClient TcpClient { get; set; }
        public string UserName { get; set; }
        public IPEndPoint UdpEndPoint { get; set; }
    }

    // ====================== RoomManager ======================
    public class RoomManager
    {
        private readonly ConcurrentDictionary<string, ConcurrentBag<ClientInfo>> rooms = new();

        // إنشاء غرفة جديدة
        public bool CreateRoom(string roomName)
        {
            return rooms.TryAdd(roomName, new ConcurrentBag<ClientInfo>());
        }

        // حذف غرفة
        public bool DeleteRoom(string roomName)
        {
            return rooms.TryRemove(roomName, out _);
        }

        // إضافة عميل إلى الغرفة
        public void AddClientToRoom(string roomName, TcpClient tcpClient, string userName = "Guest")
        {
            if (!rooms.ContainsKey(roomName))
                CreateRoom(roomName);

            rooms[roomName].Add(new ClientInfo
            {
                TcpClient = tcpClient,
                UserName = userName
            });
        }

        // حذف عميل من الغرفة
        public void RemoveClientFromRoom(string roomName, TcpClient tcpClient)
        {
            if (rooms.TryGetValue(roomName, out var clients))
            {
                var newBag = new ConcurrentBag<ClientInfo>(
                    clients.Where(c => c.TcpClient != tcpClient)
                );

                rooms[roomName] = newBag;
            }
        }


        // الحصول على جميع العملاء في الغرفة
        public IEnumerable<ClientInfo> GetClientsInRoom(string roomName)
        {
            if (rooms.TryGetValue(roomName, out var clients))
                return clients;
            return Array.Empty<ClientInfo>();
        }

        // الحصول على جميع أسماء الغرف
        public IEnumerable<string> GetAllRooms() => rooms.Keys;

        // 🔍 دالة جديدة: تحديد اسم الغرفة التي ينتمي إليها العميل (للـ UDP)
        public string FindRoomByClient(IPEndPoint endpoint)
        {
            foreach (var kvp in rooms)
            {
                foreach (var client in kvp.Value)
                {
                    if (client.UdpEndPoint != null && client.UdpEndPoint.Equals(endpoint))
                        return kvp.Key;
                }
            }
            return null;
        }
    }
}
