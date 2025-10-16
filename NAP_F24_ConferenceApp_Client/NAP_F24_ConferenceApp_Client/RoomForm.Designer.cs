using System.Drawing;
using System.Windows.Forms;

namespace NAP_F24_ConferenceApp_Client
{
    partial class RoomForm
    {
        private System.ComponentModel.IContainer components = null;
        private PictureBox pictureBoxLocal;
        private PictureBox pictureBoxRemote;
        private ListBox listBoxMessages;
        private TextBox txtMessage;
        private Button btnSend;
        private Button btnMuteAudio;
        private Button btnLeaveRoom;
        private Label lblRoomTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && components != null)
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RoomForm));
            pictureBoxLocal = new PictureBox();
            pictureBoxRemote = new PictureBox();
            listBoxMessages = new ListBox();
            txtMessage = new TextBox();
            btnSend = new Button();
            btnMuteAudio = new Button();
            btnLeaveRoom = new Button();
            lblRoomTitle = new Label();
            btnStopVideo = new Button();
            ((System.ComponentModel.ISupportInitialize)pictureBoxLocal).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRemote).BeginInit();
            SuspendLayout();
            // 
            // pictureBoxLocal
            // 
            pictureBoxLocal.BackColor = Color.DarkGray;
            pictureBoxLocal.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxLocal.Location = new Point(206, 373);
            pictureBoxLocal.Name = "pictureBoxLocal";
            pictureBoxLocal.Size = new Size(268, 121);
            pictureBoxLocal.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxLocal.TabIndex = 2;
            pictureBoxLocal.TabStop = false;
            // 
            // pictureBoxRemote
            // 
            pictureBoxRemote.BackColor = Color.Black;
            pictureBoxRemote.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxRemote.Location = new Point(20, 60);
            pictureBoxRemote.Name = "pictureBoxRemote";
            pictureBoxRemote.Size = new Size(640, 307);
            pictureBoxRemote.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBoxRemote.TabIndex = 1;
            pictureBoxRemote.TabStop = false;
            // 
            // listBoxMessages
            // 
            listBoxMessages.AllowDrop = true;
            listBoxMessages.FormattingEnabled = true;
            listBoxMessages.ItemHeight = 23;
            listBoxMessages.Location = new Point(680, 41);
            listBoxMessages.Name = "listBoxMessages";
            listBoxMessages.ScrollAlwaysVisible = true;
            listBoxMessages.Size = new Size(340, 326);
            listBoxMessages.TabIndex = 3;
            // 
            // txtMessage
            // 
            txtMessage.Location = new Point(680, 382);
            txtMessage.Name = "txtMessage";
            txtMessage.Size = new Size(340, 30);
            txtMessage.TabIndex = 4;
            // 
            // btnSend
            // 
            btnSend.Location = new Point(680, 418);
            btnSend.Name = "btnSend";
            btnSend.Size = new Size(340, 30);
            btnSend.TabIndex = 5;
            btnSend.Text = "Send";
            btnSend.Click += btnSend_Click;
            // 
            // btnMuteAudio
            // 
            btnMuteAudio.Location = new Point(20, 373);
            btnMuteAudio.Name = "btnMuteAudio";
            btnMuteAudio.Size = new Size(180, 121);
            btnMuteAudio.TabIndex = 6;
            btnMuteAudio.Text = "Mute Mic";
            btnMuteAudio.Click += btnMuteAudio_Click;
            // 
            // btnLeaveRoom
            // 
            btnLeaveRoom.BackColor = Color.SteelBlue;
            btnLeaveRoom.ForeColor = Color.White;
            btnLeaveRoom.Location = new Point(680, 454);
            btnLeaveRoom.Name = "btnLeaveRoom";
            btnLeaveRoom.Size = new Size(340, 40);
            btnLeaveRoom.TabIndex = 7;
            btnLeaveRoom.Text = "Leave Room";
            btnLeaveRoom.UseVisualStyleBackColor = false;
            btnLeaveRoom.Click += btnLeaveRoom_Click;
            // 
            // lblRoomTitle
            // 
            lblRoomTitle.AutoSize = true;
            lblRoomTitle.BackColor = Color.Transparent;
            lblRoomTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblRoomTitle.Location = new Point(20, 15);
            lblRoomTitle.Name = "lblRoomTitle";
            lblRoomTitle.Size = new Size(116, 32);
            lblRoomTitle.TabIndex = 0;
            lblRoomTitle.Text = "Room: ...";
            // 
            // btnStopVideo
            // 
            btnStopVideo.Location = new Point(480, 373);
            btnStopVideo.Name = "btnStopVideo";
            btnStopVideo.Size = new Size(180, 121);
            btnStopVideo.TabIndex = 8;
            btnStopVideo.Text = "Stop Video";
            btnStopVideo.Click += btnStopVideo_Click;
            // 
            // RoomForm
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(245, 247, 250);
            BackgroundImage = Properties.Resources.NAP_App;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(1046, 528);
            Controls.Add(btnStopVideo);
            Controls.Add(lblRoomTitle);
            Controls.Add(pictureBoxRemote);
            Controls.Add(pictureBoxLocal);
            Controls.Add(listBoxMessages);
            Controls.Add(txtMessage);
            Controls.Add(btnSend);
            Controls.Add(btnMuteAudio);
            Controls.Add(btnLeaveRoom);
            Font = new Font("Segoe UI", 10F);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "RoomForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Conference Room";
            FormClosing += RoomForm_FormClosing;
            Load += RoomForm_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBoxLocal).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBoxRemote).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
        private Button btnStopVideo;
    }
}
