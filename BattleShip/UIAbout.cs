using System;
using System.Windows.Forms;
using System.Diagnostics; // Cần thiết để sử dụng Process.Start

namespace BattleShip
{
    public partial class UIAbout : UserControl
    {
        // Thay thế bằng Discord và Email thực tế của bạn
        private const string DISCORD_LINK = "https://discord.gg/yourserverinvite";
        private const string GMAIL_ADDRESS = "YourEmail@gmail.com";

        public UIAbout()
        {
            InitializeComponent();
            // Cập nhật text Link Gmail
            this.linkLabelGmail.Text = GMAIL_ADDRESS;

            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.PerformAutoScale();
        }

        // Hàm xử lý sự kiện khi bấm vào link Discord
        private void linkLabelDiscord_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Mở trình duyệt với link Discord
                Process.Start(DISCORD_LINK);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở link Discord: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Hàm xử lý sự kiện khi bấm vào link Gmail
        private void linkLabelGmail_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Mở trình soạn thảo email mặc định của hệ thống
                Process.Start($"mailto:{GMAIL_ADDRESS}");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể mở ứng dụng email: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}