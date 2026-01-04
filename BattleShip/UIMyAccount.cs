using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Firebase.Database.Query;

namespace BattleShip
{
    public partial class UIMyAccount : UserControl
    {
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
            lblYourGmail.Text = SessionManager.CurrentEmail;
            try
            {
                var profile = await FirebaseService.firebaseClient
                    .Child("Users")
                    .Child(SessionManager.CurrentUserID)
                    .OnceSingleAsync<UserProfile>();

                if (profile != null)
                {
                    // Hiển thị tên (ưu tiên DisplayName, nếu không có thì dùng Email)
                    lblUserName.Text = string.IsNullOrEmpty(profile.DisplayName) ? profile.Email : profile.DisplayName;
                    
                    lblWin.Text = $"Win : {profile.TotalWins}";
                    lblLoss.Text = $"Loss : {profile.TotalLosses}";
                    
                    // Hiển thị Avatar dựa trên ID
                    if (profile.AvatarId >= 0 && profile.AvatarId < _avatars.Count)
                    {
                        guna2CirclePictureBoxUser.Image = _avatars[profile.AvatarId];
                    }

                    // Chọn gender
                    if (!string.IsNullOrEmpty(profile.Gender))
                    {
                        lblGender.Text = profile.Gender;
                    }
                    else
                    {
                        lblGender.Text = "Chưa cập nhật";
                    }
                        
                    // date of birth
                    if (!string.IsNullOrEmpty(profile.DateOfBirth))
                    {
                        try
                        {
                            DateTime dob = DateTime.ParseExact(profile.DateOfBirth, "dd/MM/yyyy", null);
                            dtpDoB.Value = dob;
                        }
                        catch
                        {                           
                            dtpDoB.Value = DateTime.Now;
                        }
                    }
                }
            }
            catch 
            {
                MessageBox.Show("Lỗi tải dữ liệu: ");
            }
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