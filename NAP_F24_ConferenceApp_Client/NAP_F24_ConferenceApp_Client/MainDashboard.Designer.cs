namespace NAP_F24_ConferenceApp_Client
{
    partial class MainDashboard
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private List<Room> rooms = new List<Room>();

        private TextBox txtRoomName;
        private ListBox lstRooms;
        private Button btnCreateRoom;
        private Button btnJoinRoom;
        private Button btnDeleteRoom;



        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainDashboard));
            txtRoomName = new TextBox();
            lstRooms = new ListBox();
            btnCreateRoom = new Button();
            btnJoinRoom = new Button();
            btnDeleteRoom = new Button();
            label1 = new Label();
            btnClose = new Button();
            btnBack = new Button();
            SuspendLayout();
            // 
            // txtRoomName
            // 
            txtRoomName.Location = new Point(119, 12);
            txtRoomName.Name = "txtRoomName";
            txtRoomName.Size = new Size(303, 27);
            txtRoomName.TabIndex = 0;
            // 
            // lstRooms
            // 
            lstRooms.Location = new Point(12, 49);
            lstRooms.Name = "lstRooms";
            lstRooms.ScrollAlwaysVisible = true;
            lstRooms.Size = new Size(410, 44);
            lstRooms.TabIndex = 1;
            // 
            // btnCreateRoom
            // 
            btnCreateRoom.FlatStyle = FlatStyle.System;
            btnCreateRoom.Location = new Point(12, 236);
            btnCreateRoom.Name = "btnCreateRoom";
            btnCreateRoom.Size = new Size(129, 30);
            btnCreateRoom.TabIndex = 2;
            btnCreateRoom.Text = "Create Room";
            btnCreateRoom.Click += btnCreateRoom_Click;
            // 
            // btnJoinRoom
            // 
            btnJoinRoom.FlatStyle = FlatStyle.System;
            btnJoinRoom.Location = new Point(163, 236);
            btnJoinRoom.Name = "btnJoinRoom";
            btnJoinRoom.Size = new Size(90, 30);
            btnJoinRoom.TabIndex = 3;
            btnJoinRoom.Text = "Join Room";
            btnJoinRoom.Click += btnJoinRoom_Click;
            // 
            // btnDeleteRoom
            // 
            btnDeleteRoom.FlatStyle = FlatStyle.System;
            btnDeleteRoom.Location = new Point(281, 236);
            btnDeleteRoom.Name = "btnDeleteRoom";
            btnDeleteRoom.Size = new Size(141, 30);
            btnDeleteRoom.TabIndex = 4;
            btnDeleteRoom.Text = "Delete Room";
            btnDeleteRoom.Click += btnDeleteRoom_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.Location = new Point(12, 15);
            label1.Name = "label1";
            label1.Size = new Size(101, 20);
            label1.TabIndex = 5;
            label1.Text = "Room Name:";
            // 
            // btnClose
            // 
            btnClose.FlatStyle = FlatStyle.System;
            btnClose.Location = new Point(220, 272);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(202, 30);
            btnClose.TabIndex = 6;
            btnClose.Text = "Close";
            btnClose.Click += btnClose_Click;
            // 
            // btnBack
            // 
            btnBack.FlatStyle = FlatStyle.System;
            btnBack.Location = new Point(12, 272);
            btnBack.Name = "btnBack";
            btnBack.Size = new Size(202, 30);
            btnBack.TabIndex = 7;
            btnBack.Text = "Back";
            btnBack.Click += btnBack_Click;
            // 
            // MainDashboard
            // 
            BackgroundImage = Properties.Resources.NAP_App;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(434, 321);
            Controls.Add(btnBack);
            Controls.Add(btnClose);
            Controls.Add(label1);
            Controls.Add(txtRoomName);
            Controls.Add(lstRooms);
            Controls.Add(btnCreateRoom);
            Controls.Add(btnJoinRoom);
            Controls.Add(btnDeleteRoom);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainDashboard";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Conference Dashboard";
            Load += MainDashboard_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnClose;
        private Button btnBack;
    }
}