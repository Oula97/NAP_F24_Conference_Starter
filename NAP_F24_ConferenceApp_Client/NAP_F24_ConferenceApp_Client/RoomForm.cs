using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using NAudio.Wave;

namespace NAP_F24_ConferenceApp_Client
{
    public partial class RoomForm : Form
    {
        private Room currentRoom;

        private TcpClient tcpClient;
        private NetworkStream tcpStream;

        private UdpClient videoUdpClient;
        private UdpClient audioUdpClient;

        private WaveInEvent waveIn;
        private WaveOutEvent waveOut;
        private BufferedWaveProvider bufferedWaveProvider;

        private FilterInfoCollection videoDevices;
        private VideoCaptureDevice videoSource;

        private const int TcpPort = 5000;
        private const int UdpVideoPort = 6000;
        private const int UdpAudioPort = 6001;
        private const string ServerAddress = "127.0.0.1";

        private bool isStreaming = false;
        private bool isMuted = false;
        private bool isVideoEnabled = true;
        private string localUserName;

        public RoomForm(Room room)
        {
            InitializeComponent();
            currentRoom = room;
            lblRoomTitle.Text = $"Room: {currentRoom.Name}";
            btnMuteAudio.Enabled = false; // غير مفعل حتى يبدأ التسجيل

            // توليد اسم عشوائي للمستخدم الحالي
            localUserName = "User" + Guid.NewGuid().ToString("N").Substring(0, 5);
        }

        //================ LOAD =================
        private void RoomForm_Load(object sender, EventArgs e)
        {
            try
            {
                ConnectTcp();
                SetupUdp();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to connect to server: {ex.Message}", "Connection Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //================ TCP CHAT =================
        private void ConnectTcp()
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(ServerAddress, TcpPort);
            tcpStream = tcpClient.GetStream();

            isStreaming = true;

            // إرسال اسم المستخدم إلى الخادم (اختياري حسب تصميم الخادم)
            byte[] nameData = Encoding.UTF8.GetBytes(localUserName);
            tcpStream.Write(nameData, 0, nameData.Length);

            Thread listenThread = new Thread(ListenTcp) { IsBackground = true };
            listenThread.Start();
        }

        private void ListenTcp()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (isStreaming)
                {
                    int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead > 0)
                    {
                        string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                        if (IsHandleCreated && !IsDisposed)
                        {
                            Invoke(new Action(() =>
                            {
                                // إذا كانت الرسالة مرسلة من نفس المستخدم، نظهرها مرة واحدة فقط مع [You]
                                if (msg.Contains(localUserName))
                                    msg = msg.Replace(localUserName, "You");

                                listBoxMessages.Items.Add(msg);
                                listBoxMessages.TopIndex = listBoxMessages.Items.Count - 1;
                            }));
                        }
                    }
                }
            }
            catch (ObjectDisposedException) { } // النموذج مغلق
            catch
            {
            }

        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(message)) return;

            try
            {
                string formattedMsg = $"[{localUserName}] {message}";
                byte[] data = Encoding.UTF8.GetBytes(formattedMsg);
                tcpStream.Write(data, 0, data.Length);

                // عرض رسالة المستخدم فوراً مرة واحدة
                if (IsHandleCreated && !IsDisposed)
                {
                    Invoke(new Action(() =>
                    {
                        listBoxMessages.Items.Add($"[You] {message}");
                        listBoxMessages.TopIndex = listBoxMessages.Items.Count - 1;
                        txtMessage.Clear();
                    }));
                }
            }
            catch { }
        }

        //================ AUDIO =================
        private void SetupUdp()
        {
            try
            {
                videoUdpClient = new UdpClient();
                audioUdpClient = new UdpClient();

                bufferedWaveProvider = new BufferedWaveProvider(new WaveFormat(44100, 1));
                waveOut = new WaveOutEvent();
                waveOut.Init(bufferedWaveProvider);
                waveOut.Play();

                // تشغيل استقبال الصوت
                Thread audioThread = new Thread(ReceiveAudio) { IsBackground = true };
                audioThread.Start();

                // تشغيل استقبال الفيديو
                Thread videoThread = new Thread(ReceiveVideo) { IsBackground = true };
                videoThread.Start();

                StartVideoCapture();
                StartAudioCapture();
            }
            catch { }
        }

        private void ReceiveAudio()
        {
            try
            {
                UdpClient receiveClient = new UdpClient();
                receiveClient.ExclusiveAddressUse = false;
                receiveClient.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                receiveClient.Client.Bind(new IPEndPoint(IPAddress.Any, UdpAudioPort));

                IPEndPoint ep = new IPEndPoint(IPAddress.Any, UdpAudioPort);

                while (isStreaming)
                {
                    byte[] data = receiveClient.Receive(ref ep);
                    bufferedWaveProvider.AddSamples(data, 0, data.Length);
                }
            }
            catch { }
        }

        private void StartAudioCapture()
        {
            waveIn = new WaveInEvent { WaveFormat = new WaveFormat(44100, 1) };
            waveIn.DataAvailable += (s, e) =>
            {
                if (!isMuted)
                    audioUdpClient.Send(e.Buffer, e.BytesRecorded, ServerAddress, UdpAudioPort);
            };
            waveIn.StartRecording();

            btnMuteAudio.Enabled = true;
        }

        private void btnMuteAudio_Click(object sender, EventArgs e)
        {
            isMuted = !isMuted;
            btnMuteAudio.Text = isMuted ? "Unmute Mic" : "Mute Mic";
        }

        //================ VIDEO =================
        private void StartVideoCapture()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            if (videoDevices.Count == 0) return;

            videoSource = new VideoCaptureDevice(videoDevices[0].MonikerString);
            videoSource.NewFrame += VideoSource_NewFrame;
            videoSource.Start();
        }

        private void VideoSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                Bitmap frame = new Bitmap(eventArgs.Frame, new Size(320, 240)); // الحجم المصغر
                Invoke(new Action(() =>
                {
                    if (pictureBoxLocal != null)
                        pictureBoxLocal.Image = (Bitmap)frame.Clone();
                }));

                using (MemoryStream ms = new MemoryStream())
                {
                    frame.Save(ms, ImageFormat.Jpeg);
                    byte[] buffer = ms.ToArray();
                    videoUdpClient.Send(buffer, buffer.Length, ServerAddress, UdpVideoPort);
                }
            }
            catch { }
        }

        private void ReceiveVideo()
        {
            try
            {
                UdpClient videoReceiver = new UdpClient();
                videoReceiver.ExclusiveAddressUse = false;
                videoReceiver.Client.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                videoReceiver.Client.Bind(new IPEndPoint(IPAddress.Any, UdpVideoPort));

                IPEndPoint ep = new IPEndPoint(IPAddress.Any, UdpVideoPort);

                while (isStreaming)
                {
                    try
                    {
                        byte[] data = videoReceiver.Receive(ref ep);
                        using (MemoryStream ms = new MemoryStream(data))
                        {
                            Image img = Image.FromStream(ms);
                            Invoke(new Action(() =>
                            {
                                if (pictureBoxRemote != null)
                                    pictureBoxRemote.Image = (Bitmap)img.Clone();
                            }));
                        }
                    }
                    catch { }
                }
            }
            catch { }
        }

        //================ LEAVE & CLEANUP =================
        private void btnLeaveRoom_Click(object sender, EventArgs e)
        {
            // إخفاء الفورم فوراً لتجنب شعور التأخير
            this.Hide();

            // تنظيف الموارد في Thread منفصل
            Thread cleanupThread = new Thread(() =>
            {
                StopStreaming();
            });
            cleanupThread.IsBackground = true;
            cleanupThread.Start();

            // إظهار MainDashboard
            Invoke(new Action(() =>
            {
                MainDashboard mainDashboard = new MainDashboard();
                mainDashboard.Show();
                this.Close(); // اغلق الفورم الحالي بعد إخفائه
            }));
        }


        private void RoomForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopStreaming();
        }

        private void StopStreaming()
        {
            isStreaming = false;

            try
            {
                waveIn?.StopRecording();
                waveIn?.Dispose();
                waveIn = null;

                waveOut?.Stop();
                waveOut?.Dispose();
                waveOut = null;

                if (videoSource != null)
                {
                    if (videoSource.IsRunning)
                    {
                        videoSource.SignalToStop();
                        videoSource.WaitForStop(); // هنا تستغرق وقت قليل
                    }
                    videoSource = null;
                }

                videoUdpClient?.Close();
                videoUdpClient = null;

                audioUdpClient?.Close();
                audioUdpClient = null;

                tcpStream?.Close();
                tcpStream = null;

                tcpClient?.Close();
                tcpClient = null;
            }
            catch { }
        }


        private void btnStopVideo_Click(object sender, EventArgs e)
        {
            if (isVideoEnabled)
            {
                // إيقاف الفيديو
                if (videoSource != null && videoSource.IsRunning)
                {
                    videoSource.SignalToStop();
                    videoSource.WaitForStop();
                    pictureBoxLocal.Image = null;
                }
                btnStopVideo.Text = "Start Video";
                isVideoEnabled = false;
            }
            else
            {
                // إعادة تشغيل الفيديو
                StartVideoCapture();
                btnStopVideo.Text = "Stop Video";
                isVideoEnabled = true;
            }
        }
    }
}
