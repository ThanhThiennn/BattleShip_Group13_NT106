namespace BattleShip
{
    partial class Lobby
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Lobby));
            this.PanelLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.btnLogOut = new Guna.UI2.WinForms.Guna2Button();
            this.btnFriend = new Guna.UI2.WinForms.Guna2Button();
            this.btnNews = new Guna.UI2.WinForms.Guna2Button();
            this.btnSetting = new Guna.UI2.WinForms.Guna2Button();
            this.btnSpeaker = new Guna.UI2.WinForms.Guna2Button();
            this.PanelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.txtPlayerName = new System.Windows.Forms.TextBox();
            this.btnJoinByID = new Guna.UI2.WinForms.Guna2Button();
            this.btnBrowseMatch = new Guna.UI2.WinForms.Guna2Button();
            this.btnCreateMatch = new Guna.UI2.WinForms.Guna2Button();
            this.btnPlayRandom = new Guna.UI2.WinForms.Guna2Button();
            this.btnPlayBot = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PanelLeft.SuspendLayout();
            this.PanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelLeft
            // 
            this.PanelLeft.Controls.Add(this.btnLogOut);
            this.PanelLeft.Controls.Add(this.btnFriend);
            this.PanelLeft.Controls.Add(this.btnNews);
            this.PanelLeft.Controls.Add(this.btnSetting);
            this.PanelLeft.Controls.Add(this.btnSpeaker);
            this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelLeft.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.PanelLeft.Location = new System.Drawing.Point(0, 0);
            this.PanelLeft.Name = "PanelLeft";
            this.PanelLeft.Size = new System.Drawing.Size(352, 673);
            this.PanelLeft.TabIndex = 0;
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.Transparent;
            this.btnLogOut.BorderRadius = 20;
            this.btnLogOut.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLogOut.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLogOut.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLogOut.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLogOut.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLogOut.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnLogOut.ForeColor = System.Drawing.Color.White;
            this.btnLogOut.ImageSize = new System.Drawing.Size(30, 30);
            this.btnLogOut.Location = new System.Drawing.Point(53, 567);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(233, 45);
            this.btnLogOut.TabIndex = 1;
            this.btnLogOut.Text = "Log Out";
            this.btnLogOut.UseTransparentBackground = true;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // btnFriend
            // 
            this.btnFriend.BorderThickness = 1;
            this.btnFriend.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFriend.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFriend.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFriend.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFriend.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.btnFriend.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFriend.ForeColor = System.Drawing.Color.White;
            this.btnFriend.Image = ((System.Drawing.Image)(resources.GetObject("btnFriend.Image")));
            this.btnFriend.ImageSize = new System.Drawing.Size(30, 30);
            this.btnFriend.Location = new System.Drawing.Point(176, 628);
            this.btnFriend.Name = "btnFriend";
            this.btnFriend.Size = new System.Drawing.Size(55, 45);
            this.btnFriend.TabIndex = 0;
            // 
            // btnNews
            // 
            this.btnNews.BorderThickness = 1;
            this.btnNews.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnNews.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnNews.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnNews.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnNews.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.btnNews.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnNews.ForeColor = System.Drawing.Color.White;
            this.btnNews.Image = ((System.Drawing.Image)(resources.GetObject("btnNews.Image")));
            this.btnNews.ImageSize = new System.Drawing.Size(35, 35);
            this.btnNews.Location = new System.Drawing.Point(121, 628);
            this.btnNews.Name = "btnNews";
            this.btnNews.Size = new System.Drawing.Size(55, 45);
            this.btnNews.TabIndex = 0;
            // 
            // btnSetting
            // 
            this.btnSetting.BorderThickness = 1;
            this.btnSetting.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSetting.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSetting.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSetting.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSetting.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.btnSetting.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSetting.ForeColor = System.Drawing.Color.White;
            this.btnSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnSetting.Image")));
            this.btnSetting.ImageSize = new System.Drawing.Size(40, 40);
            this.btnSetting.Location = new System.Drawing.Point(231, 628);
            this.btnSetting.Name = "btnSetting";
            this.btnSetting.Size = new System.Drawing.Size(120, 45);
            this.btnSetting.TabIndex = 0;
            // 
            // btnSpeaker
            // 
            this.btnSpeaker.BorderThickness = 1;
            this.btnSpeaker.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnSpeaker.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnSpeaker.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnSpeaker.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnSpeaker.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.btnSpeaker.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSpeaker.ForeColor = System.Drawing.Color.White;
            this.btnSpeaker.Image = ((System.Drawing.Image)(resources.GetObject("btnSpeaker.Image")));
            this.btnSpeaker.ImageSize = new System.Drawing.Size(35, 35);
            this.btnSpeaker.Location = new System.Drawing.Point(1, 628);
            this.btnSpeaker.Name = "btnSpeaker";
            this.btnSpeaker.Size = new System.Drawing.Size(120, 45);
            this.btnSpeaker.TabIndex = 0;
            // 
            // PanelRight
            // 
            this.PanelRight.BorderRadius = 15;
            this.PanelRight.Controls.Add(this.txtPlayerName);
            this.PanelRight.Controls.Add(this.btnJoinByID);
            this.PanelRight.Controls.Add(this.btnBrowseMatch);
            this.PanelRight.Controls.Add(this.btnCreateMatch);
            this.PanelRight.Controls.Add(this.btnPlayRandom);
            this.PanelRight.Controls.Add(this.btnPlayBot);
            this.PanelRight.Controls.Add(this.pictureBox1);
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelRight.FillColor = System.Drawing.Color.LavenderBlush;
            this.PanelRight.Location = new System.Drawing.Point(352, 0);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(859, 673);
            this.PanelRight.TabIndex = 1;
            // 
            // txtPlayerName
            // 
            this.txtPlayerName.Location = new System.Drawing.Point(27, 12);
            this.txtPlayerName.Name = "txtPlayerName";
            this.txtPlayerName.Size = new System.Drawing.Size(100, 22);
            this.txtPlayerName.TabIndex = 3;
            // 
            // btnJoinByID
            // 
            this.btnJoinByID.BackColor = System.Drawing.Color.Transparent;
            this.btnJoinByID.BorderRadius = 20;
            this.btnJoinByID.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnJoinByID.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnJoinByID.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnJoinByID.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnJoinByID.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnJoinByID.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnJoinByID.ForeColor = System.Drawing.Color.White;
            this.btnJoinByID.Location = new System.Drawing.Point(336, 452);
            this.btnJoinByID.Name = "btnJoinByID";
            this.btnJoinByID.Size = new System.Drawing.Size(180, 45);
            this.btnJoinByID.TabIndex = 1;
            this.btnJoinByID.Text = "Join Match By ID";
            this.btnJoinByID.UseTransparentBackground = true;
            // 
            // btnBrowseMatch
            // 
            this.btnBrowseMatch.BorderRadius = 10;
            this.btnBrowseMatch.CustomBorderThickness = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnBrowseMatch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBrowseMatch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBrowseMatch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBrowseMatch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBrowseMatch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnBrowseMatch.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnBrowseMatch.ForeColor = System.Drawing.Color.White;
            this.btnBrowseMatch.ImageSize = new System.Drawing.Size(32, 32);
            this.btnBrowseMatch.Location = new System.Drawing.Point(27, 373);
            this.btnBrowseMatch.Name = "btnBrowseMatch";
            this.btnBrowseMatch.Size = new System.Drawing.Size(811, 60);
            this.btnBrowseMatch.TabIndex = 0;
            this.btnBrowseMatch.Text = "Browse Match";
            // 
            // btnCreateMatch
            // 
            this.btnCreateMatch.BorderRadius = 10;
            this.btnCreateMatch.CustomBorderThickness = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnCreateMatch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateMatch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnCreateMatch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnCreateMatch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnCreateMatch.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(150)))), ((int)(((byte)(243)))));
            this.btnCreateMatch.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnCreateMatch.ForeColor = System.Drawing.Color.White;
            this.btnCreateMatch.ImageSize = new System.Drawing.Size(32, 32);
            this.btnCreateMatch.Location = new System.Drawing.Point(27, 295);
            this.btnCreateMatch.Name = "btnCreateMatch";
            this.btnCreateMatch.Size = new System.Drawing.Size(811, 60);
            this.btnCreateMatch.TabIndex = 0;
            this.btnCreateMatch.Text = "Create Match";
            // 
            // btnPlayRandom
            // 
            this.btnPlayRandom.BorderRadius = 10;
            this.btnPlayRandom.CustomBorderThickness = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnPlayRandom.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPlayRandom.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPlayRandom.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnPlayRandom.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPlayRandom.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnPlayRandom.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnPlayRandom.ForeColor = System.Drawing.Color.White;
            this.btnPlayRandom.Image = ((System.Drawing.Image)(resources.GetObject("btnPlayRandom.Image")));
            this.btnPlayRandom.ImageSize = new System.Drawing.Size(32, 32);
            this.btnPlayRandom.Location = new System.Drawing.Point(27, 214);
            this.btnPlayRandom.Name = "btnPlayRandom";
            this.btnPlayRandom.Size = new System.Drawing.Size(397, 60);
            this.btnPlayRandom.TabIndex = 0;
            this.btnPlayRandom.Text = "Play Random Opponent";
            this.btnPlayRandom.Click += new System.EventHandler(this.btnPlayRandom_Click);
            // 
            // btnPlayBot
            // 
            this.btnPlayBot.BorderRadius = 10;
            this.btnPlayBot.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnPlayBot.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnPlayBot.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnPlayBot.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnPlayBot.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnPlayBot.Font = new System.Drawing.Font("Segoe UI Semibold", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnPlayBot.ForeColor = System.Drawing.Color.White;
            this.btnPlayBot.Image = ((System.Drawing.Image)(resources.GetObject("btnPlayBot.Image")));
            this.btnPlayBot.ImageSize = new System.Drawing.Size(30, 30);
            this.btnPlayBot.Location = new System.Drawing.Point(427, 214);
            this.btnPlayBot.Name = "btnPlayBot";
            this.btnPlayBot.Size = new System.Drawing.Size(411, 60);
            this.btnPlayBot.TabIndex = 0;
            this.btnPlayBot.Text = " Play Against a Bot";
            this.btnPlayBot.Click += new System.EventHandler(this.btnPlayBot_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.LavenderBlush;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(229, -46);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(386, 367);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // Lobby
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 673);
            this.Controls.Add(this.PanelRight);
            this.Controls.Add(this.PanelLeft);
            this.Name = "Lobby";
            this.Text = "BattleShip";
            this.Load += new System.EventHandler(this.Lobby_Load);
            this.PanelLeft.ResumeLayout(false);
            this.PanelRight.ResumeLayout(false);
            this.PanelRight.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel PanelLeft;
        private Guna.UI2.WinForms.Guna2Panel PanelRight;
        private Guna.UI2.WinForms.Guna2Button btnPlayRandom;
        private Guna.UI2.WinForms.Guna2Button btnPlayBot;
        private Guna.UI2.WinForms.Guna2Button btnBrowseMatch;
        private Guna.UI2.WinForms.Guna2Button btnCreateMatch;
        private Guna.UI2.WinForms.Guna2Button btnJoinByID;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Button btnLogOut;
        private Guna.UI2.WinForms.Guna2Button btnSetting;
        private Guna.UI2.WinForms.Guna2Button btnSpeaker;
        private Guna.UI2.WinForms.Guna2Button btnFriend;
        private Guna.UI2.WinForms.Guna2Button btnNews;
        private System.Windows.Forms.TextBox txtPlayerName;

    }
}