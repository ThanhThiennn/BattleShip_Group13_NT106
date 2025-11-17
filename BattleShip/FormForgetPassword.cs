using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class FormForgetPassword : Form
    {
        public FormForgetPassword()
        {
            InitializeComponent();
        }
              
        private async void btnSend_Click_1(object sender, EventArgs e)
        {
            string email = tbUsername.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Vui lòng nhập Email đã đăng ký.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // Gửi liên kết đặt lại mật khẩu
                await FirebaseService.authProvider
                    .SendPasswordResetEmailAsync(email);

                MessageBox.Show($"Liên kết đặt lại mật khẩu đã được gửi đến {email}. Vui lòng kiểm tra hộp thư!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Đóng Form và quay lại Đăng nhập
                new Login().Show();
                this.Close();
            }
            catch (FirebaseAuthException ex)
            {
                string errorMessage = "Không thể gửi yêu cầu đặt lại mật khẩu.";
                if (ex.Reason == AuthErrorReason.UnknownEmailAddress)
                    errorMessage = "Email này chưa được đăng ký trong hệ thống.";

                MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnBackToLogin_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Close();
        }
    }
}
