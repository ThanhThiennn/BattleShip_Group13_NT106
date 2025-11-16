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
                //Tạo tài khoản trên firebase auth
                FirebaseAuthLink authLink = await FirebaseService.authProvider
                    .CreateUserWithEmailAndPasswordAsync(email, password);

                //Lưu thông tin cơ bản vào rt databse
                string userId = authLink.User.LocalId;
                var userProfile = new UserProfile { Email = email, TotalWins = 0 };

                await FirebaseService.firebaseClient
                    .Child("Users") // Node gốc cho tất cả người dùng
                    .Child(userId)  // Key là UID của người dùng
                    .PutAsync(userProfile);

                MessageBox.Show("Đăng ký thành công! Vui lòng Đăng nhập.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

        private  void btnGoToLogin_Click(object sender, EventArgs e)
        {
            new Login().Show();
            this.Close();
        }
    }
}
