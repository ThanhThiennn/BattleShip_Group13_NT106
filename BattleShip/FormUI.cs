using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BattleShip;

namespace BattleShip
{
    public partial class FormUI : Form
    {
        public FormUI()
        {
            InitializeComponent();
            UpdateButtonState();
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

        }

        private void btnPlayBot_Click(object sender, EventArgs e)
        {

        }

        private void btnPlayRandom_Click(object sender, EventArgs e)
        {

        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
        "Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?",
        "Xác Nhận Đăng Xuất",
        MessageBoxButtons.YesNo,
        MessageBoxIcon.Question
    );

            if (result == DialogResult.Yes)
            {
                try
                {
                    Properties.Settings.Default.FirebaseRefreshToken = null;
                    Properties.Settings.Default.Save();
                    SessionManager.ClearSession();
                    Login loginForm = new Login();
                    loginForm.Show();

                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Không thể mở Form Đăng nhập: " + ex.Message, "Lỗi Hệ Thống", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnSpeaker_Click(object sender, EventArgs e)
        {
            AudioManager.ToggleMute();
            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            if (AudioManager.IsMuted)
            {
                // TRẠNG THÁI: TẮT TIẾNG
                btnSpeaker.Text = "OFF"; // Hoặc để trống nếu dùng icon
                btnSpeaker.FillColor = Color.Gray;

                // Nếu muốn đổi icon (nếu btnSpeaker là Guna2Button)
                // btnSpeaker.Image = Properties.Resources.mute_icon; 
            }
            else
            {
                // TRẠNG THÁI: ĐANG BẬT
                btnSpeaker.Text = "ON";
                btnSpeaker.FillColor = Color.FromArgb(0, 118, 212);

                // Nếu muốn đổi icon
                // btnSpeaker.Image = Properties.Resources.speaker_icon;
            }
        }
    }
}
