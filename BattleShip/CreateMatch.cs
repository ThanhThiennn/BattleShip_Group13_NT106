using FireSharp.Response;
using Newtonsoft.Json;
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
using Firebase.Database.Query;

namespace BattleShip
{
    public partial class CreateMatch : Form
    {
        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/",
            AuthSecret = Environment.GetEnvironmentVariable("FIREBASE_SECRET_KEY", EnvironmentVariableTarget.User)
        };
        IFirebaseClient client;
        public CreateMatch()
        {
            InitializeComponent();
            client = new FireSharp.FirebaseClient(config);
            
        }

        private void CreateMatch_Load(object sender, EventArgs e)
        {
            StartRealtimeUpdate();
        }

        private void btnCreateRoom_Click(object sender, EventArgs e)
        {
            CreateRoom frmCreate = new CreateRoom();
            frmCreate.Show();
            this.Hide();
        }
        // Hàm cập nhật danh sách phòng
        private void StartRealtimeUpdate()
        {
            client.OnAsync("Rooms", (sender, args, context) => {
                // Kiểm tra handle đã sẵn sàng và form chưa bị đóng
                if (!this.IsDisposed && this.IsHandleCreated)
                {
                    this.Invoke(new Action(() => UpdateGridData(args.Data)));
                }
            });
        }

        private async void UpdateGridData(string json)
        {
            try
            {
                FirebaseResponse response = await client.GetAsync("Rooms");

                if (response == null || response.Body == "null")
                {
                    dgvRooms.Rows.Clear();
                    return;
                }

                // Giải mã toàn bộ danh sách phòng
                var allRooms = JsonConvert.DeserializeObject<Dictionary<string, Room>>(response.Body);

                dgvRooms.Rows.Clear();

                foreach (var r in allRooms)
                {

                    int count = (r.Value.Player2 != null) ? 2 : 1;
                    string displayCount = count + "/2";

                    int rowIndex = dgvRooms.Rows.Add();
                    dgvRooms.Rows[rowIndex].Cells["colID"].Value = r.Key;

                    dgvRooms.Rows[rowIndex].Cells["colName"].Value = r.Value.RoomName ?? "No Name";
                    dgvRooms.Rows[rowIndex].Cells["colCount"].Value = displayCount;
                    if (count == 2)
                    {
                        dgvRooms.Rows[rowIndex].DefaultCellStyle.ForeColor = Color.Gray;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi nạp danh sách: " + ex.Message);
            }
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn dòng nào chưa
            if (dgvRooms.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một phòng trong danh sách!");
                return;
            }

            // 2. Lấy thông tin từ dòng đang chọn
            string selectedRoomId = dgvRooms.CurrentRow.Cells["colID"].Value?.ToString() ?? "";
            string occupancy = dgvRooms.CurrentRow.Cells["colCount"].Value?.ToString() ?? "1/2";

            // 3. Kiểm tra nếu phòng đã đầy
            if (occupancy == "2/2")
            {
                MessageBox.Show("Phòng này đã đầy, vui lòng chọn phòng khác!");
                return;
            }

            try
            {
                var profile = await FirebaseService.firebaseClient.Child("Users").Child(SessionManager.CurrentUserID).OnceSingleAsync<UserProfile>();

                // 4. Tạo dữ liệu cho Player2
                var p2Data = new PlayerData
                {
                    Name = string.IsNullOrEmpty(profile.DisplayName) ? profile.Email : profile.DisplayName,
                    AvatarId = profile.AvatarId,
                    IsReady = false,
                    ShipsLeft = 5
                };

                // 5. Cập nhật Player2 lên Firebase
                await client.SetAsync($"Rooms/{selectedRoomId}/Player2", p2Data);

                // 6. Mở form game với vai trò Player2
                Multiplayer gameForm = new Multiplayer(selectedRoomId, "Player2");
                gameForm.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tham gia phòng: " + ex.Message);
            }

        }

        private void btnLeaveMatch_Click(object sender, EventArgs e)
        {
            Lobby lobbyForm = new Lobby();
            lobbyForm.Show();
            this.Close();
        }
    }
}
