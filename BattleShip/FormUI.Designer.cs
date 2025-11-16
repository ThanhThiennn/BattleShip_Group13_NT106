namespace BattleShip
{
    partial class FormUI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormUI));
            this.PanelLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.PanelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.btnPlayRandom = new Guna.UI2.WinForms.Guna2Button();
            this.btnPlayBot = new Guna.UI2.WinForms.Guna2Button();
            this.btnCreateMatch = new Guna.UI2.WinForms.Guna2Button();
            this.btnBrowseMatch = new Guna.UI2.WinForms.Guna2Button();
            this.btnJoinByID = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PanelRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelLeft
            // 
            this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelLeft.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.PanelLeft.Location = new System.Drawing.Point(0, 0);
            this.PanelLeft.Name = "PanelLeft";
            this.PanelLeft.Size = new System.Drawing.Size(351, 673);
            this.PanelLeft.TabIndex = 0;
            // 
            // PanelRight
            // 
            this.PanelRight.BorderRadius = 15;
            this.PanelRight.Controls.Add(this.btnJoinByID);
            this.PanelRight.Controls.Add(this.btnBrowseMatch);
            this.PanelRight.Controls.Add(this.btnCreateMatch);
            this.PanelRight.Controls.Add(this.btnPlayRandom);
            this.PanelRight.Controls.Add(this.btnPlayBot);
            this.PanelRight.Controls.Add(this.pictureBox1);
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelRight.FillColor = System.Drawing.Color.LavenderBlush;
            this.PanelRight.Location = new System.Drawing.Point(351, 0);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(860, 673);
            this.PanelRight.TabIndex = 1;
            this.PanelRight.Paint += new System.Windows.Forms.PaintEventHandler(this.guna2Panel2_Paint);
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
            this.btnPlayRandom.Image = global::BattleShip.Properties.Resources.multiPlayer;
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
            this.btnCreateMatch.Click += new System.EventHandler(this.guna2Button1_Click);
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
            this.btnBrowseMatch.Click += new System.EventHandler(this.guna2Button2_Click);
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
            this.pictureBox1.Visible = false;
            // 
            // FormUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1211, 673);
            this.Controls.Add(this.PanelRight);
            this.Controls.Add(this.PanelLeft);
            this.Name = "FormUI";
            this.Text = "FormUI";
            this.PanelRight.ResumeLayout(false);
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
    }
}