using Firebase.Auth;
using Firebase.Database.Query;
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
    public partial class FormRegister : Form
    {
        public FormRegister()
        {
            InitializeComponent();
        }
                      
        private void llbGoToLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            new Login().Show();
            this.Close();
        }

        private async void btnRegister_Click(object sender, EventArgs e)
        {
            string email = tbUsername.Text.Trim();
            string password = tbPassword.Text;
            string confirmPassword = tbConfirmPassword.Text;

            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu xác nhận không khớp.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (password.Length < 6)
            {
                MessageBox.Show("Mật khẩu phải có ít nhất 6 ký tự.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                // 1. Tạo tài khoản trên Firebase Auth
                FirebaseAuthLink authLink = await FirebaseService.authProvider
                    .CreateUserWithEmailAndPasswordAsync(email, password);

                // 2. Lưu thông tin người dùng vào Realtime Database
                string userId = authLink.User.LocalId;

                var userProfile = new UserProfile
                {
                    Email = email,
                    TotalWins = 0,
                    TotalLosses = 0,
                    AvatarId = 0,
                    DisplayName = email,
                    DateOfBirth = "01/01/2000",
                    Gender = "Khác"

                };

                await FirebaseService.firebaseClient
                    .Child("Users")
                    .Child(userId)
                    .PutAsync(userProfile);

                MessageBox.Show("Đăng ký thành công! Vui lòng Đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Chuyển sang màn hình đăng nhập
                new Login().Show();
                this.Close();
            }
            catch (FirebaseAuthException ex)
            {
                string errorMessage = "Đăng ký thất bại.";
                if (ex.Reason == AuthErrorReason.EmailExists)
                    errorMessage = "Email này đã được sử dụng. Vui lòng chọn Email khác.";

                MessageBox.Show(errorMessage, "Lỗi Đăng ký", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
