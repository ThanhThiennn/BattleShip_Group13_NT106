using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Auth;

namespace BattleShip
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e)
        {
            string email = tbUsername.Text.Trim();
            string password = tbPassword.Text;

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ Email và Mật khẩu.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                FirebaseAuthLink authLink = await FirebaseService.authProvider
                    .SignInWithEmailAndPasswordAsync(email, password);

                MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                FormUI gameForm = new FormUI(); 
                gameForm.Show();
                this.Hide();
            }
            catch (FirebaseAuthException ex)
            {
                string errorMessage = "Tài khoản hoặc mật khẩu không đúng. Vui lòng kiểm tra lại";
                if (ex.Reason == AuthErrorReason.UnknownEmailAddress)
                    errorMessage = "Email này chưa được đăng ký.";
                else if (ex.Reason == AuthErrorReason.WrongPassword)
                    errorMessage = "Mật khẩu không chính xác.";

                MessageBox.Show(errorMessage, "Lỗi Đăng nhập", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            new FormRegister().Show();
            this.Hide();
        }

        private void llbForgotPassword_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new FormForgetPassword().Show();
        }
    }
}
