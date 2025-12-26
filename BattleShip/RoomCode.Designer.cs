namespace BattleShip
{
    partial class RoomCode
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
            this.tbRoomCode = new System.Windows.Forms.TextBox();
            this.lbRoomCode = new System.Windows.Forms.Label();
            this.btnJoin = new Guna.UI2.WinForms.Guna2Button();
            this.SuspendLayout();
            // 
            // tbRoomCode
            // 
            this.tbRoomCode.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.tbRoomCode.Location = new System.Drawing.Point(199, 77);
            this.tbRoomCode.Name = "tbRoomCode";
            this.tbRoomCode.Size = new System.Drawing.Size(189, 31);
            this.tbRoomCode.TabIndex = 0;
            this.tbRoomCode.TextChanged += new System.EventHandler(this.tbRoomCode_TextChanged);
            // 
            // lbRoomCode
            // 
            this.lbRoomCode.AutoSize = true;
            this.lbRoomCode.Font = new System.Drawing.Font("Segoe UI", 10.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lbRoomCode.Location = new System.Drawing.Point(98, 80);
            this.lbRoomCode.Name = "lbRoomCode";
            this.lbRoomCode.Size = new System.Drawing.Size(95, 25);
            this.lbRoomCode.TabIndex = 1;
            this.lbRoomCode.Text = "Mã phòng";
            // 
            // btnJoin
            // 
            this.btnJoin.BackColor = System.Drawing.SystemColors.Control;
            this.btnJoin.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnJoin.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnJoin.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnJoin.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnJoin.FillColor = System.Drawing.Color.Lime;
            this.btnJoin.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btnJoin.ForeColor = System.Drawing.Color.White;
            this.btnJoin.Location = new System.Drawing.Point(178, 128);
            this.btnJoin.Name = "btnJoin";
            this.btnJoin.Size = new System.Drawing.Size(136, 36);
            this.btnJoin.TabIndex = 2;
            this.btnJoin.Text = "Join ";
            this.btnJoin.Click += new System.EventHandler(this.btnJoin_Click);
            // 
            // RoomCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 235);
            this.Controls.Add(this.btnJoin);
            this.Controls.Add(this.lbRoomCode);
            this.Controls.Add(this.tbRoomCode);
            this.Name = "RoomCode";
            this.Text = "RoomCode";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbRoomCode;
        private System.Windows.Forms.Label lbRoomCode;
        private Guna.UI2.WinForms.Guna2Button btnJoin;
    }
}