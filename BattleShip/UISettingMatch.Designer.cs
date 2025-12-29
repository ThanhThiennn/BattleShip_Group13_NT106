namespace BattleShip
{
    partial class UISettingMatch
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UISettingMatch));
            this.PanelLeft = new Guna.UI2.WinForms.Guna2Panel();
            this.btnLogOut = new Guna.UI2.WinForms.Guna2Button();
            this.PanelRight = new Guna.UI2.WinForms.Guna2Panel();
            this.btnStart = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Panel1 = new Guna.UI2.WinForms.Guna2Panel();
            this.guna2HtmlLabel2 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.guna2HtmlLabel1 = new Guna.UI2.WinForms.Guna2HtmlLabel();
            this.btnLeaveMatch = new Guna.UI2.WinForms.Guna2Button();
            this.dgvRooms = new Guna.UI2.WinForms.Guna2DataGridView();
            this.btnOpenCreateForm = new Guna.UI2.WinForms.Guna2Button();
            this.btnFriend = new Guna.UI2.WinForms.Guna2Button();
            this.btnNews = new Guna.UI2.WinForms.Guna2Button();
            this.btnSetting = new Guna.UI2.WinForms.Guna2Button();
            this.btnSpeaker = new Guna.UI2.WinForms.Guna2Button();
            this.PanelLeft.SuspendLayout();
            this.PanelRight.SuspendLayout();
            this.guna2Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).BeginInit();
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
            this.PanelLeft.Size = new System.Drawing.Size(352, 674);
            this.PanelLeft.TabIndex = 1;
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
            // PanelRight
            // 
            this.PanelRight.BorderRadius = 15;
            this.PanelRight.Controls.Add(this.btnOpenCreateForm);
            this.PanelRight.Controls.Add(this.dgvRooms);
            this.PanelRight.Controls.Add(this.btnStart);
            this.PanelRight.Controls.Add(this.guna2Panel1);
            this.PanelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PanelRight.FillColor = System.Drawing.Color.LavenderBlush;
            this.PanelRight.Location = new System.Drawing.Point(352, 0);
            this.PanelRight.Name = "PanelRight";
            this.PanelRight.Size = new System.Drawing.Size(853, 674);
            this.PanelRight.TabIndex = 2;
            // 
            // btnStart
            // 
            this.btnStart.BorderRadius = 10;
            this.btnStart.CustomBorderThickness = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnStart.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnStart.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnStart.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnStart.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnStart.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnStart.Font = new System.Drawing.Font("Showcard Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnStart.ImageSize = new System.Drawing.Size(32, 32);
            this.btnStart.Location = new System.Drawing.Point(585, 81);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(245, 60);
            this.btnStart.TabIndex = 11;
            this.btnStart.Text = "Start";
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // guna2Panel1
            // 
            this.guna2Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel2);
            this.guna2Panel1.Controls.Add(this.guna2HtmlLabel1);
            this.guna2Panel1.Controls.Add(this.btnLeaveMatch);
            this.guna2Panel1.CustomBorderColor = System.Drawing.Color.LightGray;
            this.guna2Panel1.Location = new System.Drawing.Point(0, 0);
            this.guna2Panel1.Name = "guna2Panel1";
            this.guna2Panel1.Size = new System.Drawing.Size(853, 59);
            this.guna2Panel1.TabIndex = 1;
            // 
            // guna2HtmlLabel2
            // 
            this.guna2HtmlLabel2.AutoSize = false;
            this.guna2HtmlLabel2.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel2.Font = new System.Drawing.Font("Segoe UI Variable Text", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.guna2HtmlLabel2.Location = new System.Drawing.Point(17, 29);
            this.guna2HtmlLabel2.Name = "guna2HtmlLabel2";
            this.guna2HtmlLabel2.Size = new System.Drawing.Size(279, 30);
            this.guna2HtmlLabel2.TabIndex = 3;
            this.guna2HtmlLabel2.Text = "WAITING FOR PLAYER";
            // 
            // guna2HtmlLabel1
            // 
            this.guna2HtmlLabel1.AutoSize = false;
            this.guna2HtmlLabel1.BackColor = System.Drawing.Color.Transparent;
            this.guna2HtmlLabel1.Font = new System.Drawing.Font("Segoe UI Variable Display", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.guna2HtmlLabel1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.guna2HtmlLabel1.Location = new System.Drawing.Point(17, 3);
            this.guna2HtmlLabel1.Name = "guna2HtmlLabel1";
            this.guna2HtmlLabel1.Size = new System.Drawing.Size(327, 45);
            this.guna2HtmlLabel1.TabIndex = 2;
            this.guna2HtmlLabel1.Text = "Match Lobby";
            this.guna2HtmlLabel1.Click += new System.EventHandler(this.guna2HtmlLabel1_Click);
            // 
            // btnLeaveMatch
            // 
            this.btnLeaveMatch.BorderRadius = 8;
            this.btnLeaveMatch.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnLeaveMatch.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnLeaveMatch.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnLeaveMatch.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnLeaveMatch.FillColor = System.Drawing.Color.Red;
            this.btnLeaveMatch.Font = new System.Drawing.Font("Segoe UI Symbol", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLeaveMatch.ForeColor = System.Drawing.Color.White;
            this.btnLeaveMatch.Location = new System.Drawing.Point(688, 12);
            this.btnLeaveMatch.Name = "btnLeaveMatch";
            this.btnLeaveMatch.Size = new System.Drawing.Size(153, 36);
            this.btnLeaveMatch.TabIndex = 0;
            this.btnLeaveMatch.Text = "Leave Match";
            this.btnLeaveMatch.Click += new System.EventHandler(this.btnLeaveMatch_Click);
            // 
            // dgvRooms
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgvRooms.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRooms.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgvRooms.ColumnHeadersHeight = 4;
            this.dgvRooms.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRooms.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgvRooms.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRooms.Location = new System.Drawing.Point(17, 81);
            this.dgvRooms.Name = "dgvRooms";
            this.dgvRooms.RowHeadersVisible = false;
            this.dgvRooms.RowHeadersWidth = 51;
            this.dgvRooms.RowTemplate.Height = 24;
            this.dgvRooms.Size = new System.Drawing.Size(546, 581);
            this.dgvRooms.TabIndex = 12;
            this.dgvRooms.ThemeStyle.AlternatingRowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRooms.ThemeStyle.AlternatingRowsStyle.Font = null;
            this.dgvRooms.ThemeStyle.AlternatingRowsStyle.ForeColor = System.Drawing.Color.Empty;
            this.dgvRooms.ThemeStyle.AlternatingRowsStyle.SelectionBackColor = System.Drawing.Color.Empty;
            this.dgvRooms.ThemeStyle.AlternatingRowsStyle.SelectionForeColor = System.Drawing.Color.Empty;
            this.dgvRooms.ThemeStyle.BackColor = System.Drawing.Color.White;
            this.dgvRooms.ThemeStyle.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRooms.ThemeStyle.HeaderStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(88)))), ((int)(((byte)(255)))));
            this.dgvRooms.ThemeStyle.HeaderStyle.BorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvRooms.ThemeStyle.HeaderStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRooms.ThemeStyle.HeaderStyle.ForeColor = System.Drawing.Color.White;
            this.dgvRooms.ThemeStyle.HeaderStyle.HeaightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.EnableResizing;
            this.dgvRooms.ThemeStyle.HeaderStyle.Height = 4;
            this.dgvRooms.ThemeStyle.ReadOnly = false;
            this.dgvRooms.ThemeStyle.RowsStyle.BackColor = System.Drawing.Color.White;
            this.dgvRooms.ThemeStyle.RowsStyle.BorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvRooms.ThemeStyle.RowsStyle.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRooms.ThemeStyle.RowsStyle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            this.dgvRooms.ThemeStyle.RowsStyle.Height = 24;
            this.dgvRooms.ThemeStyle.RowsStyle.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(229)))), ((int)(((byte)(255)))));
            this.dgvRooms.ThemeStyle.RowsStyle.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(71)))), ((int)(((byte)(69)))), ((int)(((byte)(94)))));
            // 
            // btnOpenCreateForm
            // 
            this.btnOpenCreateForm.BorderRadius = 10;
            this.btnOpenCreateForm.CustomBorderThickness = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.btnOpenCreateForm.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnOpenCreateForm.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnOpenCreateForm.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnOpenCreateForm.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnOpenCreateForm.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnOpenCreateForm.Font = new System.Drawing.Font("Showcard Gothic", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenCreateForm.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.btnOpenCreateForm.ImageSize = new System.Drawing.Size(32, 32);
            this.btnOpenCreateForm.Location = new System.Drawing.Point(585, 163);
            this.btnOpenCreateForm.Name = "btnOpenCreateForm";
            this.btnOpenCreateForm.Size = new System.Drawing.Size(245, 60);
            this.btnOpenCreateForm.TabIndex = 13;
            this.btnOpenCreateForm.Text = "Create Room";
            this.btnOpenCreateForm.Click += new System.EventHandler(this.btnOpenCreateForm_Click);
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
            // UISettingMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1205, 674);
            this.Controls.Add(this.PanelRight);
            this.Controls.Add(this.PanelLeft);
            this.Name = "UISettingMatch";
            this.Text = "UISettingMatch";
            this.PanelLeft.ResumeLayout(false);
            this.PanelRight.ResumeLayout(false);
            this.guna2Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRooms)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Guna.UI2.WinForms.Guna2Panel PanelLeft;
        private Guna.UI2.WinForms.Guna2Button btnLogOut;
        private Guna.UI2.WinForms.Guna2Button btnFriend;
        private Guna.UI2.WinForms.Guna2Button btnNews;
        private Guna.UI2.WinForms.Guna2Button btnSetting;
        private Guna.UI2.WinForms.Guna2Button btnSpeaker;
        private Guna.UI2.WinForms.Guna2Panel PanelRight;
        private Guna.UI2.WinForms.Guna2Button btnLeaveMatch;
        private Guna.UI2.WinForms.Guna2Panel guna2Panel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel1;
        private Guna.UI2.WinForms.Guna2HtmlLabel guna2HtmlLabel2;
        private Guna.UI2.WinForms.Guna2Button btnStart;
        private Guna.UI2.WinForms.Guna2DataGridView dgvRooms;
        private Guna.UI2.WinForms.Guna2Button btnOpenCreateForm;
    }
}