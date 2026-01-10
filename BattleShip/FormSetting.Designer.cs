using System.Drawing;

namespace BattleShip
{
    partial class FormSetting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSetting));
            this.PanelLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.btnBack = new Guna.UI2.WinForms.Guna2Button();
            this.btnAbout = new Guna.UI2.WinForms.Guna2Button();
            this.btnMyAccount = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.PanelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.PanelLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // PanelLeft
            // 
            this.PanelLeft.Controls.Add(this.btnBack);
            this.PanelLeft.Controls.Add(this.btnAbout);
            this.PanelLeft.Controls.Add(this.btnMyAccount);
            this.PanelLeft.Controls.Add(this.pictureBox1);
            this.PanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.PanelLeft.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.PanelLeft.Location = new System.Drawing.Point(0, 0);
            this.PanelLeft.Name = "PanelLeft";
            this.PanelLeft.Size = new System.Drawing.Size(223, 575);
            this.PanelLeft.TabIndex = 1;
            // 
            // btnBack
            // 
            this.btnBack.BackColor = System.Drawing.Color.Transparent;
            this.btnBack.BorderRadius = 20;
            this.btnBack.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnBack.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnBack.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnBack.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnBack.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnBack.ForeColor = System.Drawing.Color.White;
            this.btnBack.ImageSize = new System.Drawing.Size(30, 30);
            this.btnBack.Location = new System.Drawing.Point(12, 507);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(204, 42);
            this.btnBack.TabIndex = 2;
            this.btnBack.Text = "Back to game";
            this.btnBack.UseTransparentBackground = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnAbout
            // 
            this.btnAbout.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.btnAbout.BorderThickness = 1;
            this.btnAbout.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnAbout.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnAbout.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnAbout.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnAbout.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.btnAbout.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAbout.ForeColor = System.Drawing.Color.White;
            this.btnAbout.ImageSize = new System.Drawing.Size(35, 35);
            this.btnAbout.Location = new System.Drawing.Point(0, 188);
            this.btnAbout.Name = "btnAbout";
            this.btnAbout.Size = new System.Drawing.Size(223, 45);
            this.btnAbout.TabIndex = 3;
            this.btnAbout.Text = "About";
            this.btnAbout.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnAbout.Click += new System.EventHandler(this.btnAbout_Click);
            // 
            // btnMyAccount
            // 
            this.btnMyAccount.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(61)))));
            this.btnMyAccount.BorderThickness = 1;
            this.btnMyAccount.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnMyAccount.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnMyAccount.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnMyAccount.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnMyAccount.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(63)))), ((int)(((byte)(71)))));
            this.btnMyAccount.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMyAccount.ForeColor = System.Drawing.Color.White;
            this.btnMyAccount.ImageSize = new System.Drawing.Size(35, 35);
            this.btnMyAccount.Location = new System.Drawing.Point(0, 146);
            this.btnMyAccount.Name = "btnMyAccount";
            this.btnMyAccount.Size = new System.Drawing.Size(223, 45);
            this.btnMyAccount.TabIndex = 1;
            this.btnMyAccount.Text = "My Account";
            this.btnMyAccount.TextAlign = System.Windows.Forms.HorizontalAlignment.Left;
            this.btnMyAccount.Click += new System.EventHandler(this.btnMyAccount_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(-59, -44);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(342, 223);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // PanelRight
            // 
            this.PanelRight.AutoScroll = true;
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.PanelRight.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(54)))), ((int)(((byte)(70)))));
            this.PanelRight.Location = new System.Drawing.Point(223, 0);
            this.PanelRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(700, 575);
            this.PanelRight.TabIndex = 2;
            // 
            // FormSetting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 575);
            this.Controls.Add(this.PanelRight);
            this.Controls.Add(this.PanelLeft);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormSetting";
            this.Text = "Setting";
            this.PanelLeft.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel PanelLeft;
        private Guna.UI2.WinForms.Guna2Panel PanelRight;
        private Guna.UI2.WinForms.Guna2Button btnMyAccount;
        private Guna.UI2.WinForms.Guna2Button btnAbout;
        private Guna.UI2.WinForms.Guna2Button btnBack;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}