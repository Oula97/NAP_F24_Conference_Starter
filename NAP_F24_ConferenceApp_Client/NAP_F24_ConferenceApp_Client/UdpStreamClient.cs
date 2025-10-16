using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace NAP_F24_ConferenceApp_Client
{
    public class UdpStreamClient
    {
        private string serverIp;
        private int port;
        private UdpClient udpClient;
        private UdpClient udpListener;
        private Thread listenThread;
        private bool listening = false;

        // Video chunk buffer
        private class VideoBuffer
        {
            public int TotalChunks;
            public byte[][] Chunks;
            public int ReceivedCount = 0;
        }
        private Dictionary<string, VideoBuffer> videoBuffers = new Dictionary<string, VideoBuffer>();

        // Event for video frames
        public event Action<string, byte[]> OnVideoFrameReceived;

        public UdpStreamClient(string ip, int port)
        {
            serverIp = ip;
            this.port = port;
            udpClient = new UdpClient();
            udpListener = new UdpClient(0); // random local port
        }

        public void StartReceive()
        {
            listening = true;
            listenThread = new Thread(ListenLoop) { IsBackground = true };
            listenThread.Start();
        }

        private void ListenLoop()
        {
            IPEndPoint ep = new IPEndPoint(System.Net.IPAddress.Any, 0);

            while (listening)
            {
                try
                {
                    byte[] packet = udpListener.Receive(ref ep);
                    if (packet.Length == 0) continue;

                    string headerStr = Encoding.UTF8.GetString(packet, 0, Math.Min(200, packet.Length));
                    if (!headerStr.StartsWith("ROOM:")) continue;

                    // Check if this is a video packet
                    if (headerStr.Contains("|VIDEO|"))
                    {
                        // Format: ROOM:{room}|VIDEO|{username}|{chunkIndex}|{totalChunks}|
                        string[] parts = headerStr.Split('|');
                        if (parts.Length < 6) continue;

                        string username = parts[2];
                        int chunkIndex = int.Parse(parts[3]);
                        int totalChunks = int.Parse(parts[4]);
                        int headerEnd = headerStr.IndexOf('|', headerStr.IndexOf("|VIDEO|") + 1) + 1;
                        byte[] frameData = new byte[packet.Length - headerEnd];
                        Array.Copy(packet, headerEnd, frameData, 0, frameData.Length);

                        string key = username;

                        if (!videoBuffers.ContainsKey(key))
                            videoBuffers[key] = new VideoBuffer { TotalChunks = totalChunks, Chunks = new byte[totalChunks][] };

                        var buffer = videoBuffers[key];
                        if (buffer.Chunks[chunkIndex] == null)
                        {
                            buffer.Chunks[chunkIndex] = frameData;
                            buffer.ReceivedCount++;
                        }

                        if (buffer.ReceivedCount == buffer.TotalChunks)
                        {
                            using (var ms = new System.IO.MemoryStream())
                            {
                                for (int i = 0; i < buffer.TotalChunks; i++)
                                    ms.Write(buffer.Chunks[i], 0, buffer.Chunks[i].Length);

                                OnVideoFrameReceived?.Invoke(username, ms.ToArray());
                            }
                            videoBuffers.Remove(key);
                        }
                    }
                }
                catch { }
            }
        }

        // Send raw UDP payload (video/audio)
        public void SendRoomPayload(string room, byte[] data)
        {
            udpClient.Send(data, data.Length, serverIp, port);
        }

        // Register client to a room
        public void SendRegistration(string room)
        {
            string msg = $"UDPJOIN:{room}";
            byte[] data = Encoding.UTF8.GetBytes(msg);
            udpClient.Send(data, data.Length, serverIp, port);
        }

        // Stop listening
        public void Stop()
        {
            listening = false;
            try { udpListener.Close(); } catch { }
            try { udpClient.Close(); } catch { }
        }

        // Receive raw UDP data for audio
        public byte[] ReceiveUdpData(ref IPEndPoint ep)
        {
            return udpListener.Receive(ref ep);
        }
    }
}
