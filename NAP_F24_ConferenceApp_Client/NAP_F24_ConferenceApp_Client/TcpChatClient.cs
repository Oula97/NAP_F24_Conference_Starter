using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace NAP_F24_ConferenceApp_Client
{
    public class TcpChatClient
    {
        private TcpClient _tcpClient;
        private NetworkStream _stream;
        public string UserName { get; private set; }
        public string RoomName { get; private set; }

        public event Action<string> MessageReceived;

        public async Task ConnectAsync(string host, int port, string userName, string roomName)
        {
            _tcpClient = new TcpClient();
            await _tcpClient.ConnectAsync(host, port);
            _stream = _tcpClient.GetStream();
            UserName = userName;
            RoomName = roomName;

            // إرسال اسم المستخدم والغرفة عند الاتصال
            await SendMessageAsync(userName);
            await SendMessageAsync(roomName);

            _ = Task.Run(ListenAsync);
        }

        public async Task SendMessageAsync(string message)
        {
            if (_tcpClient.Connected)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(message);
                await _stream.WriteAsync(buffer, 0, buffer.Length);
            }
        }

        private async Task ListenAsync()
        {
            byte[] buffer = new byte[1024];
            while (_tcpClient.Connected)
            {
                try
                {
                    int bytesRead = await _stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    MessageReceived?.Invoke(message);
                }
                catch
                {
                    break;
                }
            }
        }
    }
}
