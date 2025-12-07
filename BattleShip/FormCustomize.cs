using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Firebase.Database.Query;
using Guna.UI2.WinForms;

namespace BattleShip
{
    public partial class FormCustomize : Form
    {
        // Biến lưu trữ ID ảnh đang được chọn
        private int _selectedAvatarId = 0;

        // Danh sách 8 ảnh mẫu từ Resources
        private List<Image> _avatarList = new List<Image>()
        {
            Properties.Resources.avt1, // ID 0
            Properties.Resources.avt2, // ID 1
            Properties.Resources.avt3, // ID 2
            Properties.Resources.avt4, // ID 3
            Properties.Resources.avt5, // ID 4
            Properties.Resources.avt6, // ID 5
            Properties.Resources.avt7, // ID 6
            Properties.Resources.avt8  // ID 7
        };

        public FormCustomize()
        {
            InitializeComponent();
            LoadUserData(); // Tải dữ liệu cũ lên trước
            LoadAvatarList(); // Tạo danh sách ảnh để chọn
        }

        // Tải thông tin người dùng từ Firebase
        private async void LoadUserData()
        {
            try
            {
                var userProfile = await FirebaseService.firebaseClient
                    .Child("Users")
                    .Child(SessionManager.CurrentUserID)
                    .OnceSingleAsync<UserProfile>();

                if (userProfile != null)
                {
                    // Hiển thị tên
                    txtName.Text = string.IsNullOrEmpty(userProfile.DisplayName)
                        ? userProfile.Email.Split('@')[0]
                        : userProfile.DisplayName;

                    // Lấy ID avatar hiện tại và highlight nó lên
                    _selectedAvatarId = userProfile.AvatarId;
                    HighlightSelectedAvatar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải dữ liệu: " + ex.Message);
            }
        }

        // Tạo danh sách ảnh hiển thị lên FlowLayoutPanel
        private void LoadAvatarList()
        {
            // panelAvatars là tên cái FlowLayoutPanel bạn đã kéo vào form
            panelAvatars.Controls.Clear();

            for (int i = 0; i < _avatarList.Count; i++)
            {
                Guna2CirclePictureBox pic = new Guna2CirclePictureBox();
                pic.Image = _avatarList[i];
                pic.SizeMode = PictureBoxSizeMode.Zoom;
                pic.Size = new Size(90, 90); // Kích thước ảnh
                pic.Cursor = Cursors.Hand;
                pic.Tag = i; // QUAN TRỌNG: Lưu ID (0-7) vào Tag để nhận biết
                pic.Margin = new Padding(10); // Khoảng cách giữa các ảnh

                // Cài đặt mặc định cho Shadow (Hiệu ứng viền sáng)
                pic.ShadowDecoration.Mode = Guna.UI2.WinForms.Enums.ShadowMode.Circle;
                pic.ShadowDecoration.Color = Color.Gold; // Màu vàng kim nổi bật
                pic.ShadowDecoration.Depth = 60; // Độ đậm của bóng
                pic.ShadowDecoration.Enabled = false; // Mặc định tắt (chưa chọn)

                // Gán sự kiện click
                pic.Click += Avatar_Click;

                panelAvatars.Controls.Add(pic);
            }

            // Gọi highlight lần đầu để sáng ảnh đang dùng
            HighlightSelectedAvatar();
        }

        // Sự kiện khi bấm vào một ảnh bất kỳ
        private void Avatar_Click(object sender, EventArgs e)
        {
            Guna2CirclePictureBox clickedPic = sender as Guna2CirclePictureBox;

            // Lấy ID từ Tag của ảnh vừa bấm
            _selectedAvatarId = (int)clickedPic.Tag;

            // Cập nhật lại giao diện để sáng ảnh mới, tối ảnh cũ
            HighlightSelectedAvatar();
        }

        // Hàm xử lý hiệu ứng "Dấu hiệu nhận biết"
        private void HighlightSelectedAvatar()
        {
            foreach (Control c in panelAvatars.Controls)
            {
                if (c is Guna2CirclePictureBox pic)
                {
                    int id = (int)pic.Tag;

                    if (id == _selectedAvatarId)
                    {
                        // Đây là ảnh đang chọn -> BẬT HIỆU ỨNG
                        pic.ShadowDecoration.Enabled = true; // Bật bóng sáng
                        pic.BorderStyle = BorderStyle.FixedSingle; // Hoặc thêm viền cứng nếu muốn
                    }
                    else
                    {
                        // Đây không phải ảnh chọn -> TẮT HIỆU ỨNG
                        pic.ShadowDecoration.Enabled = false;
                        pic.BorderStyle = BorderStyle.None;
                    }
                }
            }
        }

        // Nút Lưu thay đổi
        private async void btnSave_Click(object sender, EventArgs e)
        {
            string newName = txtName.Text.Trim();
            if (string.IsNullOrEmpty(newName))
            {
                MessageBox.Show("Tên không được để trống!");
                return;
            }

            try
            {
                // Cập nhật Tên hiển thị
                await FirebaseService.firebaseClient
                    .Child("Users")
                    .Child(SessionManager.CurrentUserID)
                    .Child("DisplayName")
                    .PutAsync($"\"{newName}\"");

                // Cập nhật ID ảnh đại diện
                await FirebaseService.firebaseClient
                    .Child("Users")
                    .Child(SessionManager.CurrentUserID)
                    .Child("AvatarId")
                    .PutAsync(_selectedAvatarId);

                MessageBox.Show("Cập nhật thành công!");
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi cập nhật: " + ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}