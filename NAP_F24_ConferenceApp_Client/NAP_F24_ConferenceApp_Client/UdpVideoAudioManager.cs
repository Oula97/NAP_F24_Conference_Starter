using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using OpenCvSharp;

namespace NAP_F24_ConferenceApp_Client
{
    public class UdpVideoAudioManager
    {
        public string ServerAddress { get; private set; }
        public int VideoPort { get; private set; }
        public int AudioPort { get; private set; }
        private UdpClient videoClient;
        private UdpClient audioClient;

        public UdpVideoAudioManager(string serverAddress, int videoPort, int audioPort)
        {
            ServerAddress = serverAddress;
            VideoPort = videoPort;
            AudioPort = audioPort;
            videoClient = new UdpClient();
            audioClient = new UdpClient();
        }

        public void SendVideo(byte[] frameData)
        {
            videoClient.Send(frameData, frameData.Length, ServerAddress, VideoPort);
        }

        public void SendAudio(byte[] audioData)
        {
            audioClient.Send(audioData, audioData.Length, ServerAddress, AudioPort);
        }
    }
}
