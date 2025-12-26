using BattleShip;
using Firebase;
using Firebase.Database;
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
    public partial class FormUI : Form
    {
        FirebaseClient firebase = new FirebaseClient("https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/");
        public FormUI()
        {
            InitializeComponent();
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
            Bot map1 = new Bot();
            map1.Show();
        }

        private async void btnPlayRandom_Click(object sender, EventArgs e)
        {
            using (var codeForm = new RoomCode())
            {
                // Hiện Form nhập mã dưới dạng Dialog
                if (codeForm.ShowDialog() == DialogResult.OK)
                {
                    string id = codeForm.RoomID;

                    // 1. Kiểm tra phòng trên Firebase
                    var roomSnapshot = await firebase.Child("Matches").Child(id).OnceSingleAsync<RoomData>();

                    string role = "";
                    if (roomSnapshot == null)
                    {
                        // Phòng chưa có -> Bạn là chủ phòng (p1)
                        role = "p1";
                        await firebase.Child("Matches").Child(id).PutAsync(new RoomData
                        {
                            P1 = new PlayerData { Name = "Chủ phòng", IsReady = false }
                        });
                    }
                    else if (roomSnapshot.P2 == null)
                    {
                        // Phòng đã có p1 nhưng chưa có p2 -> Bạn vào vai p2
                        role = "p2";
                        await firebase.Child("Matches").Child(id).Child("P2").PutAsync(new PlayerData
                        {
                            Name = "Khách",
                            IsReady = false
                        });
                    }
                    else
                    {
                        MessageBox.Show("Phòng này đã đầy người!");
                        return;
                    }

                    // 2. Chuyển sang Form Multiplayer với thông tin đã có
                    Multiplayer gameForm = new Multiplayer(id, role);
                    gameForm.Show();
                    this.Hide(); // Ẩn Form Menu/Lobby
                }
            }
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
    }
}
