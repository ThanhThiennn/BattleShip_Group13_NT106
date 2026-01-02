using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace BattleShip
{
    public partial class CreateRoom : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/",
            AuthSecret = Environment.GetEnvironmentVariable("FIREBASE_SECRET_KEY", EnvironmentVariableTarget.User)
        };

        IFirebaseClient client;
        public CreateRoom()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtRoomName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên phòng!");
                return;
            }

            // 1. Tạo mã phòng ngẫu nhiên 4 số: Room_xxxx
            string randomID = new Random().Next(1000, 9999).ToString();
            string roomId = "Room_" + randomID;

            // 2. Tạo đối tượng Room theo Model.cs bạn đã có
            var newRoom = new Room
            {
                RoomName = txtRoomName.Text,
                Status = "readying",
                Turn = "Player1",
                Player1 = new PlayerData
                {
                    Name = Login.CurrentUserEmail,
                    IsReady = false,
                    ShipsLeft = 5
                }
            };

            try
            {
                // 3. Đẩy lên Firebase
                await client.SetAsync($"Rooms/{roomId}", newRoom);

                // 4. Mở form Multiplayer và truyền roomId, role là Player1
                Multiplayer gameForm = new Multiplayer(roomId, "Player1");
                gameForm.Show();
                this.Close(); // Đóng form tạo phòng
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tạo phòng: " + ex.Message);
            }
        }

        private void CreateRoom_FormClosing(object sender, FormClosingEventArgs e)
        {
            CreateMatch formMatch = new CreateMatch();
            formMatch.Show();
        }
    }
}
