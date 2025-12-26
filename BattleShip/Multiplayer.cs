using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Firebase.Database;
using Firebase.Database.Query;

namespace BattleShip
{
    public partial class Multiplayer : Form
    {
        private Point mouseOffset;
        private Control draggedShip = null;
        private bool isDragging = false;
        private int[,] playerGrid = new int[GRID_SIZE, GRID_SIZE];
        private int[,] botGrid = new int[GRID_SIZE, GRID_SIZE]; // Trong multiplayer, đây là lưới đối thủ

        private const int CELL_SIZE = 40;
        private const int GRID_SIZE = 11;

        private int playerHits = 0; // Số ô mình bị trúng
        private int botHits = 0;    // Số ô đối thủ bị trúng
        private const int TOTAL_SHIP_CELLS = 17;

        private List<Ship> playerShips = new List<Ship>();
        private List<Control> placedShips = new List<Control>();

        private Dictionary<Control, Point> initialShipLocations = new Dictionary<Control, Point>();
        private Dictionary<Control, bool> isVertical = new Dictionary<Control, bool>();
        private bool isSetupComplete = false;
        private bool isReady = false;

        // CẤU HÌNH FIREBASE - Thay URL của bạn vào đây
        FirebaseClient firebase = new FirebaseClient("https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/");
        string roomID;
        string myRole;     // "p1" hoặc "p2"
        bool isMyTurn = false;



        public Multiplayer(string id, string role)
        {
            InitializeComponent();
            this.roomID = id;
            this.myRole = role;

            this.UpdateStyles();
            pnlGameGrid.Paint += pnlGameGrid_Paint;
            pnlBotGrid.Paint += pnlGameGrid_Paint;

            StartListening();
        }

        // --- KHỞI TẠO PHÒNG ---
        public async void CreateRoom(string id)
        {
            roomID = id;
            myRole = "p1";
            var room = new RoomData { P1 = new PlayerData { Name = "Player 1", IsReady = false } };
            await firebase.Child("Matches").Child(roomID).PutAsync(room);
            StartListening();
        }

        public async void JoinRoom(string id)
        {
            roomID = id;
            myRole = "p2";
            var p2Data = new PlayerData { Name = "Player 2", IsReady = false };
            await firebase.Child("Matches").Child(roomID).Child("P2").PutAsync(p2Data);
            StartListening();
        }

        // --- LẮNG NGHE DỮ LIỆU TỪ FIREBASE ---
        private void StartListening()
        {
            // 1. Lắng nghe hành động bắn
            firebase.Child("Matches").Child(roomID).Child("Actions").Child("LastShot")
                .AsObservable<ShotData>()
                .Subscribe(shot => {
                    if (shot.Object == null) return;
                    var data = shot.Object;

                    // Nếu đối thủ bắn mình
                    if (data.By != myRole && data.Type == "Pending")
                    {
                        this.Invoke((MethodInvoker)delegate { ProcessOpponentShot(data.X, data.Y); });
                    }
                    // Nếu mình bắn và có kết quả phản hồi
                    else if (data.By == myRole && (data.Type == "Hit" || data.Type == "Miss"))
                    {
                        this.Invoke((MethodInvoker)delegate { HandleMyShotResult(data.X, data.Y, data.Type); });
                    }
                });

            // 2. Lắng nghe trạng thái trận đấu
            firebase.Child("Matches").Child(roomID)
        .AsObservable<RoomData>()
        .Subscribe(room => {
            if (room.Object == null) return;

            var data = room.Object;

            // CẬP NHẬT DANH SÁCH NGƯỜI CHƠI LÊN UI
            UpdatePlayerListUI(data);

            // Kiểm tra điều kiện bắt đầu game
            if (data.P1?.IsReady == true && data.P2?.IsReady == true)
            {
                this.Invoke((MethodInvoker)delegate {
                    if (isSetupComplete && !isReady) StartGame();
                });
            }
        });
        }

        private void UpdatePlayerListUI(RoomData room)
        {
            // Sử dụng Invoke để tránh lỗi xung đột luồng (Cross-thread)
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => UpdatePlayerListUI(room)));
                return;
            }

            lstPlayers.Items.Clear();

            // Thêm Người chơi 1 nếu tồn tại
            if (room.P1 != null && !string.IsNullOrEmpty(room.P1.Name))
            {
                string status = room.P1.IsReady ? "[Sẵn sàng]" : "[Đang chuẩn bị]";
                lstPlayers.Items.Add($"P1: {room.P1.Name} {status}");
            }

            // Thêm Người chơi 2 nếu tồn tại
            if (room.P2 != null && !string.IsNullOrEmpty(room.P2.Name))
            {
                string status = room.P2.IsReady ? "[Sẵn sàng]" : "[Đang chuẩn bị]";
                lstPlayers.Items.Add($"P2: {room.P2.Name} {status}");
            }
        }

        private void StartGame()
        {
            isReady = true;
            pnlBotGrid.Visible = true;
            pnlBotGrid.BringToFront();
            isMyTurn = (myRole == "p1");
            lblStatus.Text = isMyTurn ? "TRẬN ĐẤU BẮT ĐẦU! LƯỢT CỦA BẠN." : "TRẬN ĐẤU BẮT ĐẦU! ĐỢI ĐỐI THỦ.";
            lblStatus.ForeColor = Color.SpringGreen;
        }

        // --- LOGIC BẮN PHÁ ---
        private async void pnlBotGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isReady || !isMyTurn) return;

            int x = e.X / CELL_SIZE;
            int y = e.Y / CELL_SIZE;

            if (x >= GRID_SIZE || y >= GRID_SIZE || botGrid[x, y] != 0) return;

            // Gửi tọa độ lên Firebase để đối thủ kiểm tra
            await firebase.Child("Matches").Child(roomID).Child("Actions").Child("LastShot")
                .PutAsync(new ShotData { By = myRole, X = x, Y = y, Type = "Pending" });

            isMyTurn = false;
            lblStatus.Text = "Đang đợi đối thủ phản hồi...";
            lblStatus.ForeColor = Color.White;
        }

        private async void ProcessOpponentShot(int x, int y)
        {
            string result = (playerGrid[x, y] == 1) ? "Hit" : "Miss";
            playerGrid[x, y] = (result == "Hit") ? 2 : -1;

            ShowHitMarker(pnlGameGrid, x, y, result);

            // Gửi kết quả ngược lại cho đối thủ
            await firebase.Child("Matches").Child(roomID).Child("Actions").Child("LastShot")
                .Child("Type").PutAsync(result);

            isMyTurn = true;
            lblStatus.Text = "Đối thủ vừa bắn! Đến lượt của bạn.";
            lblStatus.ForeColor = Color.SpringGreen;
        }

        private void HandleMyShotResult(int x, int y, string type)
        {
            botGrid[x, y] = (type == "Hit") ? 2 : -1;
            ShowHitMarker(pnlBotGrid, x, y, type);

            if (type == "Hit")
            {
                botHits++;
                lblStatus.Text = "TRÚNG RỒI! Bắn tiếp đi!";
                isMyTurn = true; // Quy tắc: Trúng được bắn tiếp
            }
            else
            {
                lblStatus.Text = "TRƯỢT RỒI! Đợi đối thủ.";
                isMyTurn = false;
            }

            if (botHits >= TOTAL_SHIP_CELLS) EndGame("BẠN THẮNG!");
        }

        private async void btnReady_Click(object sender, EventArgs e)
        {
            if (placedShips.Count < 5)
            {
                MessageBox.Show("Hãy đặt đủ 5 tàu!");
                return;
            }

            isSetupComplete = true;
            DisableSetupControls();
            btnReady.Enabled = false;
            btnReady.Text = "Đang đợi đối thủ...";

            await firebase.Child("Matches").Child(roomID).Child(myRole).Child("IsReady").PutAsync(true);
        }

        private void ShowHitMarker(Control parent, int x, int y, string type)
        {
            PictureBox marker = new PictureBox
            {
                Size = new Size(30, 30),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.Transparent,
                Location = new Point(x * CELL_SIZE + 5, y * CELL_SIZE + 5),
                Enabled = false,
                Image = (type == "Hit") ? Properties.Resources.hit_icon : Properties.Resources.miss_icon
            };
            if (type == "Reveal") marker.BackColor = Color.FromArgb(100, Color.Orange);

            parent.Controls.Add(marker);
            marker.BringToFront();
        }

        private void EndGame(string message)
        {
            isReady = false;
            lblStatus.Text = "GAME OVER: " + message;
            MessageBox.Show(message);
        }

        private void DisableSetupControls()
        {
            foreach (var ship in placedShips) ship.Enabled = false;
            if (pnlDeployment != null) pnlDeployment.Enabled = false;
        }

        private void pnlGameGrid_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen gridPen = new Pen(Color.FromArgb(150, Color.LightGray), 1) { DashStyle = DashStyle.Dot };
            for (int i = 0; i <= GRID_SIZE; i++)
            {
                int coord = i * CELL_SIZE;
                g.DrawLine(gridPen, coord, 0, coord, ((Control)sender).Height);
                g.DrawLine(gridPen, 0, coord, ((Control)sender).Width, coord);
            }
        }

        private void PlaceShipOnMatrix(int[,] grid, int x, int y, int length, bool isVertical)
        {
            for (int i = 0; i < length; i++)
            {
                int currX = isVertical ? x : x + i;
                int currY = isVertical ? y + i : y;
                grid[currX, currY] = 1;
            }
        }
    }
}