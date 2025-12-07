// File: BattleShip/UIMyAccount.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Firebase.Database.Query;

namespace BattleShip
{
    public partial class UIMyAccount : UserControl
    {
        // Danh sách ảnh (Phải khớp thứ tự với FormCustomize)
        private List<Image> _avatars = new List<Image>()
        {
            Properties.Resources.avt1,
            Properties.Resources.avt2,
            Properties.Resources.avt3,
            Properties.Resources.avt4,
            Properties.Resources.avt5,
            Properties.Resources.avt6,
            Properties.Resources.avt7,
            Properties.Resources.avt8
        };

        public UIMyAccount()
        {
            InitializeComponent();
            LoadUserData(); // Tải dữ liệu khi mở
        }

        public async void LoadUserData()
        {
            if (!SessionManager.IsLoggedIn) return;

            try
            {
                var profile = await FirebaseService.firebaseClient
                    .Child("Users")
                    .Child(SessionManager.CurrentUserID)
                    .OnceSingleAsync<UserProfile>();

                if (profile != null)
                {
                    // Hiển thị tên (ưu tiên DisplayName, nếu không có thì dùng Email)
                    label2.Text = string.IsNullOrEmpty(profile.DisplayName)
                                  ? "Player"
                                  : profile.DisplayName;

                    label7.Text = profile.Email; // Gmail hiển thị ở dưới
                    label3.Text = $"Winner : {profile.TotalWins}";

                    // Hiển thị Avatar dựa trên ID
                    if (profile.AvatarId >= 0 && profile.AvatarId < _avatars.Count)
                    {
                        guna2CirclePictureBoxUser.Image = _avatars[profile.AvatarId];
                    }
                }
            }
            catch { }
        }

        // Gán sự kiện này cho nút Customize trong phần Design
        private void btnCustomize_Click(object sender, EventArgs e)
        {
            FormCustomize frm = new FormCustomize();

            // Mở form dưới dạng hộp thoại
            if (frm.ShowDialog() == DialogResult.OK)
            {
                // Nếu người dùng ấn Lưu thành công, tải lại dữ liệu để cập nhật ngay lập tức
                LoadUserData();
            }
        }
    }
}