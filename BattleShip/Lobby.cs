
﻿using BattleShip;
using Firebase;
using Firebase.Database;
using Firebase.Database.Query;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;


namespace BattleShip
{
    public partial class Lobby : Form
    {

        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;
        public Lobby()
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
            Bot map1 = new Bot();
            map1.Show();
        }

        private async void btnPlayRandom_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPlayerName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên!");
                return;
            }

            var resp = await client.GetAsync("Rooms");
            var allRooms = resp.ResultAs<Dictionary<string, Room>>();

            string targetRoom = null;
            if (allRooms != null)
            {
                // Tìm phòng đang đợi
                targetRoom = allRooms.FirstOrDefault(x => x.Value.Status == "waiting").Key;
            }

            if (targetRoom != null)
            {
                // VÀO PHÒNG CÓ SẴN (Player 2)
                var p2 = new PlayerData { Name = txtPlayerName.Text, IsReady = false, ShipsLeft = 5 };

                // Cập nhật Player2 và đổi Status thành readying
                await client.UpdateAsync($"Rooms/{targetRoom}", new
                {
                    Player2 = p2,
                    Status = "readying"
                });
                OpenGame(targetRoom, "Player2");
            }
            else
            {
                string newId = "Room_" + new Random().Next(1000, 9999);
                var p1 = new PlayerData { Name = txtPlayerName.Text, IsReady = false, ShipsLeft = 5 };

                var newRoom = new Room
                {
                    Status = "waiting",
                    Player1 = p1,
                    Turn = "Player1"
                };
                await client.SetAsync($"Rooms/{newId}", newRoom);
                OpenGame(newId, "Player1");
            }
        }

        private void OpenGame(string id, string role)
        {
            this.Invoke(new Action(() => {
                Multiplayer gameForm = new Multiplayer(id, role);
                this.Hide();
                gameForm.FormClosed += async (s, args) =>
                {
                    try
                    {
                        if (role == "Player1")
                        {
                            await client.DeleteAsync($"Rooms/{id}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Lỗi khi xóa phòng: " + ex.Message);
                    }
                    finally
                    {
                        if (!this.IsDisposed)
                        {
                            this.Show();
                        }
                    }
                };
                gameForm.Show();
            }));
        }
        private async Task CreateRoom()
        {
            string newRoomId = "Room_" + DateTime.Now.Ticks.ToString().Substring(10); // Tạo ID ngẫu nhiên

            var roomData = new
            {
                Status = "waiting",
                Turn = "Player1", // Player1 đi trước mặc định
                Player1 = new
                {
                    Name = txtPlayerName.Text, // Lấy tên từ TextBox ở Lobby
                    IsReady = false,
                    ShipsLeft = 5
                }
            };

            // Tạo node mới trên Firebase
            await client.SetAsync($"Rooms/{newRoomId}", roomData);

            // Chuyển sang Form Multiplayer
            OpenGameForm(newRoomId, "Player1");
        }

        private async Task JoinRoom(string roomId)
        {
            var p2Data = new
            {
                Name = txtPlayerName.Text,
                IsReady = false,
                ShipsLeft = 5
            };

            // Cập nhật Player2 vào phòng đã tìm thấy
            await client.SetAsync($"Rooms/{roomId}/Player2", p2Data);

            // Cập nhật trạng thái phòng sang 'readying' (đang chuẩn bị)
            await client.SetAsync($"Rooms/{roomId}/Status", "readying");

            // Chuyển sang Form Multiplayer
            OpenGameForm(roomId, "Player2");
        }

        private void OpenGameForm(string roomId, string role)
        {
            Multiplayer gameForm = new Multiplayer(roomId, role);

            this.Hide(); 

            gameForm.ShowDialog(); // Hiển thị trận đấu 
            this.Show(); 
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


        private void Lobby_Load(object sender, EventArgs e)
        {
            // Lấy key từ biến môi trường
            string dbSecret = Environment.GetEnvironmentVariable("FIREBASE_SECRET_KEY", EnvironmentVariableTarget.User);
            if (!string.IsNullOrEmpty(dbSecret))
            {
                config.AuthSecret = dbSecret;
            }

            client = new FireSharp.FirebaseClient(config);
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

        private void btnSetting_Lobby_Click(object sender, EventArgs e)
        {
            FormSetting setting = new FormSetting();
            setting.Show();
        }
    }
}
