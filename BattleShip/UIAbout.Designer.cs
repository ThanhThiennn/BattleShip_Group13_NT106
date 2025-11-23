namespace BattleShip
{
    partial class UIAbout
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UIAbout));
            this.linkLabelGmail = new System.Windows.Forms.LinkLabel();
            this.guna2PictureBoxGmail = new Guna.UI2.WinForms.Guna2PictureBox();
            this.linkLabelDiscord = new System.Windows.Forms.LinkLabel();
            this.guna2PictureBoxDiscord = new Guna.UI2.WinForms.Guna2PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.guna2TextBox1 = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2TextBox2 = new Guna.UI2.WinForms.Guna2TextBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBoxGmail)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBoxDiscord)).BeginInit();
            this.SuspendLayout();
            // 
            // linkLabelGmail
            // 
            this.linkLabelGmail.AutoSize = true;
            this.linkLabelGmail.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelGmail.LinkColor = System.Drawing.Color.White;
            this.linkLabelGmail.Location = new System.Drawing.Point(433, 614);
            this.linkLabelGmail.Name = "linkLabelGmail";
            this.linkLabelGmail.Size = new System.Drawing.Size(220, 30);
            this.linkLabelGmail.TabIndex = 7;
            this.linkLabelGmail.TabStop = true;
            this.linkLabelGmail.Text = "YourEmail@gmail.com";
            this.linkLabelGmail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelGmail_LinkClicked);
            // 
            // guna2PictureBoxGmail
            // 
            this.guna2PictureBoxGmail.Image = ((System.Drawing.Image)(resources.GetObject("guna2PictureBoxGmail.Image")));
            this.guna2PictureBoxGmail.ImageRotate = 0F;
            this.guna2PictureBoxGmail.Location = new System.Drawing.Point(314, 567);
            this.guna2PictureBoxGmail.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.guna2PictureBoxGmail.Name = "guna2PictureBoxGmail";
            this.guna2PictureBoxGmail.Size = new System.Drawing.Size(122, 112);
            this.guna2PictureBoxGmail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBoxGmail.TabIndex = 6;
            this.guna2PictureBoxGmail.TabStop = false;
            // 
            // linkLabelDiscord
            // 
            this.linkLabelDiscord.AutoSize = true;
            this.linkLabelDiscord.Font = new System.Drawing.Font("Segoe UI", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabelDiscord.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(114)))), ((int)(((byte)(137)))), ((int)(((byte)(218)))));
            this.linkLabelDiscord.Location = new System.Drawing.Point(136, 614);
            this.linkLabelDiscord.Name = "linkLabelDiscord";
            this.linkLabelDiscord.Size = new System.Drawing.Size(146, 30);
            this.linkLabelDiscord.TabIndex = 4;
            this.linkLabelDiscord.TabStop = true;
            this.linkLabelDiscord.Text = "Discord Server";
            this.linkLabelDiscord.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabelDiscord_LinkClicked);
            // 
            // guna2PictureBoxDiscord
            // 
            this.guna2PictureBoxDiscord.Image = global::BattleShip.Properties.Resources.discord_icon;
            this.guna2PictureBoxDiscord.ImageRotate = 0F;
            this.guna2PictureBoxDiscord.Location = new System.Drawing.Point(14, 580);
            this.guna2PictureBoxDiscord.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.guna2PictureBoxDiscord.Name = "guna2PictureBoxDiscord";
            this.guna2PictureBoxDiscord.Size = new System.Drawing.Size(116, 99);
            this.guna2PictureBoxDiscord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.guna2PictureBoxDiscord.TabIndex = 5;
            this.guna2PictureBoxDiscord.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 38);
            this.label1.TabIndex = 8;
            this.label1.Text = "About Us";
            // 
            // guna2TextBox1
            // 
            this.guna2TextBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.guna2TextBox1.BorderColor = System.Drawing.Color.Black;
            this.guna2TextBox1.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox1.DefaultText = resources.GetString("guna2TextBox1.DefaultText");
            this.guna2TextBox1.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.guna2TextBox1.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TextBox1.ForeColor = System.Drawing.Color.White;
            this.guna2TextBox1.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox1.Location = new System.Drawing.Point(14, 83);
            this.guna2TextBox1.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.guna2TextBox1.Multiline = true;
            this.guna2TextBox1.Name = "guna2TextBox1";
            this.guna2TextBox1.PlaceholderText = "";
            this.guna2TextBox1.ReadOnly = true;
            this.guna2TextBox1.SelectedText = "";
            this.guna2TextBox1.Size = new System.Drawing.Size(744, 238);
            this.guna2TextBox1.TabIndex = 9;
            // 
            // guna2TextBox2
            // 
            this.guna2TextBox2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.guna2TextBox2.BorderColor = System.Drawing.Color.Black;
            this.guna2TextBox2.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.guna2TextBox2.DefaultText = "Phạm Thanh Thiên \r\nPhan Như Thuần\r\nTrương Đức Thịnh\r\n";
            this.guna2TextBox2.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.guna2TextBox2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.guna2TextBox2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox2.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.guna2TextBox2.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.guna2TextBox2.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2TextBox2.ForeColor = System.Drawing.Color.White;
            this.guna2TextBox2.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.guna2TextBox2.Location = new System.Drawing.Point(14, 434);
            this.guna2TextBox2.Margin = new System.Windows.Forms.Padding(5, 6, 5, 6);
            this.guna2TextBox2.Multiline = true;
            this.guna2TextBox2.Name = "guna2TextBox2";
            this.guna2TextBox2.PlaceholderText = "";
            this.guna2TextBox2.ReadOnly = true;
            this.guna2TextBox2.SelectedText = "";
            this.guna2TextBox2.Size = new System.Drawing.Size(744, 123);
            this.guna2TextBox2.TabIndex = 10;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(22, 378);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(197, 38);
            this.label2.TabIndex = 11;
            this.label2.Text = "Developed by";
            // 
            // UIAbout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(70)))));
            this.Controls.Add(this.label2);
            this.Controls.Add(this.guna2TextBox2);
            this.Controls.Add(this.guna2TextBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.linkLabelGmail);
            this.Controls.Add(this.guna2PictureBoxGmail);
            this.Controls.Add(this.guna2PictureBoxDiscord);
            this.Controls.Add(this.linkLabelDiscord);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UIAbout";
            this.Size = new System.Drawing.Size(788, 750);
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBoxGmail)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.guna2PictureBoxDiscord)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBoxGmail;
        private System.Windows.Forms.LinkLabel linkLabelGmail;
        private System.Windows.Forms.LinkLabel linkLabelDiscord;
        private Guna.UI2.WinForms.Guna2PictureBox guna2PictureBoxDiscord;
        private System.Windows.Forms.Label label1;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox1;
        private Guna.UI2.WinForms.Guna2TextBox guna2TextBox2;
        private System.Windows.Forms.Label label2;
    }
}