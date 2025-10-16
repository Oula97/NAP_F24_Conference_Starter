namespace NAP_F24_ConferenceApp_Client
{
    partial class Start
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
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Start));
            buttonMove = new Button();
            btnClose = new Button();
            SuspendLayout();
            // 
            // buttonMove
            // 
            buttonMove.Location = new Point(12, 202);
            buttonMove.Name = "buttonMove";
            buttonMove.Size = new Size(384, 29);
            buttonMove.TabIndex = 0;
            buttonMove.Text = "Conference  Dashboard";
            buttonMove.UseVisualStyleBackColor = true;
            buttonMove.Click += buttonMove_Click;
            // 
            // btnClose
            // 
            btnClose.FlatStyle = FlatStyle.System;
            btnClose.Location = new Point(12, 237);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(384, 29);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            btnClose.Click += btnClose_Click;
            // 
            // Start
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.NAP_App;
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(408, 276);
            Controls.Add(btnClose);
            Controls.Add(buttonMove);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Start";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Start";
            ResumeLayout(false);
        }

        #endregion

        private Button buttonMove;
        private Button btnClose;
    }
}