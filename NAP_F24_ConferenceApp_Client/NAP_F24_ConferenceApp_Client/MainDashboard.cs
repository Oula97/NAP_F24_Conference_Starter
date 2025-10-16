using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave.Compression;

namespace NAP_F24_ConferenceApp_Client
{
    public partial class MainDashboard : Form
    {
        private TcpClient tcpClient;
        private NetworkStream tcpStream;
        private Thread listenThread;
        private const int TcpPort = 5000;
        private const string ServerAddress = "127.0.0.1";

        public MainDashboard()
        {
            InitializeComponent();
            LoadRoomsFromAppSettings();
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            string roomName = txtRoomName.Text.Trim();
            if (!string.IsNullOrEmpty(roomName) && !rooms.Exists(r => r.Name == roomName))
            {
                rooms.Add(new Room { Name = roomName, MemberCount = 0 });
                UpdateRoomList();
                txtRoomName.Clear();
            }
            else
            {
                MessageBox.Show("Invalid or duplicate room name.");
            }
        }

        private void UpdateRoomList()
        {
            lstRooms.DataSource = null;
            lstRooms.DataSource = rooms;
            lstRooms.DisplayMember = "Name";
        }

        private void LoadRoomsFromAppSettings()
        {
            // يمكنك إضافة تحميل الغرف من ملف إعدادات إذا رغبت
        }

        private void btnDeleteRoom_Click(object sender, EventArgs e)
        {
            if (lstRooms.SelectedItem is Room selectedRoom)
            {
                rooms.Remove(selectedRoom);
                UpdateRoomList();
            }
            else
            {
                MessageBox.Show("Please select a room to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnJoinRoom_Click(object sender, EventArgs e)
        {
            if (lstRooms.SelectedItem is Room selectedRoom)
            {
                if (selectedRoom.MemberCount < selectedRoom.MaxMembers)
                {
                    // زيادة عدد الأعضاء
                    selectedRoom.MemberCount++;
                    UpdateRoomList();

                    // الانتقال إلى واجهة RoomForm وتمرير الغرفة
                    RoomForm roomForm = new RoomForm(selectedRoom);

                    // عند غلق RoomForm، إظهار MainDashboard مجددًا
                    roomForm.FormClosed += (s, args) => this.Show();

                    roomForm.Show();
                    this.Hide(); // إخفاء MainDashboard مؤقتًا
                }
                else
                {
                    MessageBox.Show("Room is full.", "Cannot Join Room", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Please select a room to join.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            // إظهار الفورم السابق
            Start start = new Start();
            start.Show();

            // ثم إغلاق الفورم الحالي مباشرة
            this.Dispose(); // Dispose أفضل من Close لضمان تحرير الموارد
        }

        private void MainDashboard_Load(object sender, EventArgs e)
        {
            try
            {
                tcpClient = new TcpClient(ServerAddress, TcpPort);
                tcpStream = tcpClient.GetStream();

                listenThread = new Thread(ListenForServerMessages);
                listenThread.IsBackground = true;
                listenThread.Start();

                // طلب قائمة الغرف فور الاتصال
                byte[] data = Encoding.UTF8.GetBytes("/rooms");
                tcpStream.Write(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Server connection failed: {ex.Message}");
            }
        }
        private void ListenForServerMessages()
        {
            byte[] buffer = new byte[1024];
            try
            {
                while (true)
                {
                    int bytesRead = tcpStream.Read(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    string msg = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    if (msg.StartsWith("📋ROOMS:"))
                    {
                        string[] roomNames = msg.Replace("📋ROOMS:", "").Split(',');
                        Invoke(new Action(() =>
                        {
                            rooms = roomNames.Select(r => new Room { Name = r }).ToList();
                            UpdateRoomList();
                        }));
                    }
                }
            }
            catch { }
        }


    }
}
