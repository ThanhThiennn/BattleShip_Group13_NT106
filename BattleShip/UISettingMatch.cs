using Firebase.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using Firebase.Database.Query;
using System.Threading;

namespace BattleShip
{
    public partial class UISettingMatch : Form
    {
        FirebaseClient firebaseClient = new FirebaseClient("https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/");
        Socket client;
        Socket server;
        bool isHost = false;
        public UISettingMatch()
        {
            InitializeComponent();
        }
        public class RoomModel
        {
            public string RoomName { get; set; }
            public int PlayerCount { get; set; }
            public string HostIP { get; set; }
        }
        public string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "127.0.0.1";
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void btnLeaveMatch_Click(object sender, EventArgs e)
        {
            Lobby nextForm = new Lobby();
            nextForm.Show();
            this.Close();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn đăng xuất khỏi hệ thống?","Xác Nhận Đăng Xuất",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

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

        private async void btnStart_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra xem người dùng đã chọn dòng nào trong DataGridView chưa
            if (dgvRooms.CurrentRow == null)
            {
                MessageBox.Show("Vui lòng chọn một phòng trong danh sách trước!");
                return;
            }

            // 2. Lấy RoomID từ cột đầu tiên của dòng đang chọn (Giả định cột 0 là ID)
            string selectedRID = dgvRooms.CurrentRow.Cells[0].Value.ToString();

            try
            {
                // 3. Truy vấn dữ liệu từ Firebase dựa trên mã phòng đã chọn
                var room = await firebaseClient.Child("Rooms").Child(selectedRID).OnceSingleAsync<RoomModel>();

                if (room == null)
                {
                    MessageBox.Show("Phòng không còn tồn tại!");
                    LoadRoomList(); // Refresh lại danh sách
                }
                else if (room.PlayerCount >= 2)
                {
                    MessageBox.Show("Phòng đã đầy (2/2)!");
                }
                else
                {
                    // 4. Cập nhật số người lên 2/2 lên Firebase
                    await firebaseClient.Child("Rooms").Child(selectedRID).Child("PlayerCount").PutAsync(2);

                    // 5. Kết nối Socket đến IP của chủ phòng đã lưu trên Firebase
                    // Lưu ý: Đảm bảo bạn đã viết hàm ConnectToHost(string ip)
                    ConnectToHost(room.HostIP);

                    MessageBox.Show($"Đang tham gia vào phòng: {room.RoomName}");

                    // Chuyển sang màn hình chơi game hoặc form chat
                    // GoToGameForm(); 
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tham gia phòng: " + ex.Message);
            }
        }

        private async void btnOpenCreateForm_Click(object sender, EventArgs e)
        {
            using (FormCreateRoom f = new FormCreateRoom())
            {
                // Hiển thị Form dưới dạng Dialog (buộc người dùng tương tác xong mới quay lại)
                if (f.ShowDialog() == DialogResult.OK)
                {
                    string rName = f.RoomName;
                    string rID = f.RoomID;

                    // 2. Kiểm tra trùng mã phòng trên Firebase
                    var checkRoom = await firebaseClient.Child("Rooms").Child(rID).OnceSingleAsync<dynamic>();

                    if (checkRoom != null)
                    {
                        MessageBox.Show("Mã phòng này đã tồn tại trên hệ thống. Vui lòng tạo mã khác!");
                    }
                    else
                    {
                        // 3. Tiến hành tạo phòng và đợi kết nối (Listen)
                        CreateRoomOnFirebase(rID, rName);
                    }
                }
            }
        }
        private async void CreateRoomOnFirebase(string id, string name)
        {
            var newRoom = new
            {
                RoomName = name,
                PlayerCount = 1,
                HostIP = GetLocalIPAddress()
            };

            // 1. Đẩy dữ liệu lên Firebase
            await firebaseClient.Child("Rooms").Child(id).PutAsync(newRoom);

            StartServer();
            MessageBox.Show($"Đã tạo phòng {name}. Đang đợi đối thủ vào...");
        }
        bool isLoading = false;
        private async void LoadRoomList()
        {
            if (isLoading) return;
            isLoading = true;
            try
            {
                var rooms = await firebaseClient.Child("Rooms").OnceAsync<RoomModel>();

                // Lưu lại vị trí dòng đang chọn hiện tại để không bị nhảy chuột
                int selectedIndex = dgvRooms.CurrentRow?.Index ?? -1;

                dgvRooms.Rows.Clear();
                foreach (var r in rooms)
                {
                    dgvRooms.Rows.Add(r.Key, r.Object.RoomName, r.Object.PlayerCount + "/2");
                }

                // Khôi phục lựa chọn
                if (selectedIndex >= 0 && selectedIndex < dgvRooms.Rows.Count)
                    dgvRooms.Rows[selectedIndex].Selected = true;
            }
            catch { /* Tránh hiện thông báo lỗi liên tục nếu mất mạng tạm thời */ }
        }
        private bool IsValidRoomID(string id)
        {
            // Kiểm tra có đúng 8 ký tự và đều là số không
            return id.Length == 8 && id.All(char.IsDigit);
        }
        void StartServer()
        {
            try
            {
                isHost = true; // Đánh dấu mình là chủ phòng
                IPEndPoint ip = new IPEndPoint(IPAddress.Any, 9999);
                server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                server.Bind(ip);

                Thread listenThread = new Thread(() => {
                    server.Listen(1);
                    client = server.Accept();

                    // XÓA phần khởi chạy Thread Receive tại đây

                    this.Invoke(new MethodInvoker(delegate {
                        MessageBox.Show("Đối thủ đã kết nối! Bắt đầu trận đấu.");
                        GoToGameForm(); // Hàm chuyển sang Form trận đấu
                    }));
                });
                listenThread.IsBackground = true;
                listenThread.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi Server: " + ex.Message);
            }
        }
        void ConnectToHost(string hostIP)
        {
            try
            {
                isHost = false; // Mình là người tham gia
                IPEndPoint ip = new IPEndPoint(IPAddress.Parse(hostIP), 9999);
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                client.Connect(ip);

                // XÓA phần khởi chạy Thread Receive tại đây

                GoToGameForm(); // Hàm chuyển sang Form trận đấu
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không thể kết nối đến chủ phòng: " + ex.Message);
            }
        }
        void GoToGameForm()
        {
            // Giả sử bạn của bạn đặt tên Form là FormGame
            // Bạn cần đảm bảo Constructor của FormGame được sửa thành public FormGame(Socket s, bool host)
            try
            {
                //____FormGame matchForm = new Form(this.client, this.isHost);  //Nơi vào form game

                this.Hide();
                //____matchForm.ShowDialog(); // Mở form trận đấu
                this.Close(); // Đóng form sảnh sau khi chơi xong
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi vào trận: " + ex.Message);
            }
        }

        private void UISettingMatch_Load(object sender, EventArgs e)
        {
            // Cấu hình DataGridView trước khi nạp dữ liệu
            dgvRooms.Columns.Clear();
            dgvRooms.Columns.Add("RoomID", "Mã Phòng");
            dgvRooms.Columns.Add("RoomName", "Tên Phòng");
            dgvRooms.Columns.Add("PlayerCount", "Số Người");
            dgvRooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Tùy chỉnh giao diện
            dgvRooms.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRooms.ReadOnly = true;
            
            LoadRoomList(); // Gọi lần đầu để hiện danh sách

            // Khởi động Timer để tự động làm mới (xem bước 2)
            timerRefresh.Enabled = true;
            timerRefresh.Start();
        }
        
        private void timerRefresh_Tick_1(object sender, EventArgs e)
        {
            LoadRoomList();
        }
    }
}
