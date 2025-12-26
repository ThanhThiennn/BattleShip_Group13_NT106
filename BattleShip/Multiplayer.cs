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
        private int[,] botGrid = new int[GRID_SIZE, GRID_SIZE]; // Lưới đối thủ

        private const int CELL_SIZE = 40;
        private const int GRID_SIZE = 11;
        private int playerHits = 0;
        private int botHits = 0;
        private const int TOTAL_SHIP_CELLS = 17;

        private List<Ship> playerShips = new List<Ship>();
        private List<Control> placedShips = new List<Control>();
        private Dictionary<Control, Point> initialShipLocations = new Dictionary<Control, Point>();
        private Dictionary<Control, bool> isVertical = new Dictionary<Control, bool>();

        private bool isSetupComplete = false;
        private bool isReady = false;

        // Firebase Client
        FirebaseClient firebase = new FirebaseClient("https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/");
        string roomID;
        string myRole; // "p1" hoặc "p2"
        bool isMyTurn = false;

        public Multiplayer(string id, string role)
        {
            InitializeComponent();
            this.roomID = id;
            this.myRole = role;

            this.UpdateStyles();
            pnlGameGrid.Paint += pnlGameGrid_Paint;
            pnlBotGrid.Paint += pnlGameGrid_Paint;
            lblRoomCode.Text = "Phòng: " + id;

            // Khởi tạo trạng thái ban đầu
            EnablePlacement(false);
            StartListening();
        }

        private void Multiplayer_Load(object sender, EventArgs e)
        {
            this.KeyPreview = true;
            this.KeyUp += Multiplayer_KeyUp;

            // Khởi tạo danh sách tàu và sự kiện kéo thả
            List<Control> shipControls = new List<Control> { picCarrier, picBattleShip, picCruiser1, picCruiser2, picDestroyer };
            foreach (var ship in shipControls)
            {
                if (!initialShipLocations.ContainsKey(ship))
                    initialShipLocations.Add(ship, ship.Location);
                if (!isVertical.ContainsKey(ship))
                    isVertical.Add(ship, false);

                ship.MouseDown += ship_MouseDown;
                ship.MouseMove += ship_MouseMove;
                ship.MouseUp += ship_MouseUp;
            }
        }

        // --- HÀM LẮNG NGHE FIREBASE (ĐÃ TỐI ƯU) ---
        private void StartListening()
        {
            if (string.IsNullOrEmpty(roomID)) return;

            // 1. Lắng nghe hành động bắn
            firebase.Child("Matches").Child(roomID).Child("Actions").Child("LastShot")
                .AsObservable<ShotData>()
                .Subscribe(shot => {
                    if (shot.Object == null) return;
                    var data = shot.Object;

                    if (data.By != "" && data.By != myRole && data.Type == "Pending")
                    {
                        this.Invoke((MethodInvoker)delegate { ProcessOpponentShot(data.X, data.Y); });
                    }
                    else if (data.By == myRole && (data.Type == "Hit" || data.Type == "Miss"))
                    {
                        this.Invoke((MethodInvoker)delegate { ApplyShotResultFromServer(data.X, data.Y, data.Type); });
                    }
                });

            // 2. Lắng nghe trạng thái phòng và người chơi
            firebase.Child("Matches").Child(roomID)
                .AsObservable<RoomData>()
                .Subscribe(room => {
                    if (room.Object == null) return;
                    var data = room.Object;

                    this.Invoke((MethodInvoker)delegate {
                        UpdatePlayerListUI(data);

                        // Kiểm tra nếu đủ 2 người thì cho phép dàn trận
                        if (data.P1 != null && data.P2 != null)
                        {
                            if (!isSetupComplete)
                            {
                                lblStatus.Text = "Đối thủ đã vào! Hãy sắp xếp tàu.";
                                lblStatus.ForeColor = Color.Yellow;
                                EnablePlacement(true);
                            }
                        }

                        // Kiểm tra bắt đầu game
                        if (data.P1?.IsReady == true && data.P2?.IsReady == true)
                        {
                            if (isSetupComplete && !isReady) StartOnlineGame();
                        }
                    });
                });
        }

        // --- LOGIC XỬ LÝ BẮN (MULTI PLAYER) ---
        private async void pnlBotGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isReady || !isMyTurn) return;

            int x = e.X / CELL_SIZE;
            int y = e.Y / CELL_SIZE;

            if (x >= GRID_SIZE || y >= GRID_SIZE || botGrid[x, y] != 0) return;

            // Gửi cú bắn lên Firebase
            var shot = new ShotData { By = myRole, X = x, Y = y, Type = "Pending" };
            await firebase.Child("Matches").Child(roomID).Child("Actions").Child("LastShot").PutAsync(shot);

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
            await firebase.Child("Matches").Child(roomID).Child("Actions").Child("LastShot").Child("Type").PutAsync(result);

            isMyTurn = true;
            lblStatus.Text = "Đến lượt của bạn!";
            lblStatus.ForeColor = Color.SpringGreen;
        }

        private void ApplyShotResultFromServer(int x, int y, string type)
        {
            if (botGrid[x, y] != 0) return; // Tránh xử lý lặp

            botGrid[x, y] = (type == "Hit") ? 2 : -1;
            ShowHitMarker(pnlBotGrid, x, y, type);

            if (type == "Hit")
            {
                botHits++;
                lblStatus.Text = "TRÚNG RỒI! Bắn tiếp!";
                isMyTurn = true;
            }
            else
            {
                lblStatus.Text = "TRƯỢT RỒI! Đợi đối thủ.";
                isMyTurn = false;
            }

            if (botHits >= TOTAL_SHIP_CELLS) EndGame("BẠN CHIẾN THẮNG!");
        }

        private void StartOnlineGame()
        {
            isReady = true;
            pnlBotGrid.Visible = true;
            pnlBotGrid.BringToFront();
            isMyTurn = (myRole == "p1");
            lblStatus.Text = isMyTurn ? "TRẬN ĐẤU BẮT ĐẦU! LƯỢT CỦA BẠN." : "TRẬN ĐẤU BẮT ĐẦU! ĐỢI ĐỐI THỦ.";
            lblStatus.ForeColor = Color.SpringGreen;
        }

        // --- CÁC HÀM HỖ TRỢ GIAO DIỆN ---
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

            // Nếu là P1, reset hành động cũ để ván mới sạch sẽ
            if (myRole == "p1")
                await firebase.Child("Matches").Child(roomID).Child("Actions").DeleteAsync();

            await firebase.Child("Matches").Child(roomID).Child(myRole).Child("IsReady").PutAsync(true);
        }

        private void Multiplayer_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R && draggedShip != null) RotateShip(draggedShip);
        }

        private void RotateShip(Control ship)
        {
            int temp = ship.Width;
            ship.Width = ship.Height;
            ship.Height = temp;
            ((PictureBox)ship).Image?.RotateFlip(RotateFlipType.Rotate90FlipNone);
            isVertical[ship] = !isVertical[ship];
            this.Refresh();
        }

        private void EnablePlacement(bool enable)
        {
            if (pnlDeployment != null) pnlDeployment.Enabled = enable;
            foreach (var ship in initialShipLocations.Keys) ship.Enabled = enable;
        }

        private void UpdatePlayerListUI(RoomData room)
        {
            lstPlayers.Items.Clear();
            if (room.P1 != null) lstPlayers.Items.Add($"P1: {room.P1.Name} {(room.P1.IsReady ? "[Sẵn sàng]" : "[Đang chuẩn bị]")}");
            if (room.P2 != null) lstPlayers.Items.Add($"P2: {room.P2.Name} {(room.P2.IsReady ? "[Sẵn sàng]" : "[Đang chuẩn bị]")}");
        }

        // --- LOGIC VẼ VÀ MARKER (GIỮ NGUYÊN) ---
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
            parent.Controls.Add(marker);
            marker.BringToFront();
        }

        private void EndGame(string message)
        {
            isReady = false;
            lblStatus.Text = message;
            MessageBox.Show(message);
        }

        private void ship_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)

            {
                draggedShip = (Control)sender;
                mouseOffset = new Point(e.X, e.Y);
                isDragging = true;
                Point oldScreenLocation = draggedShip.Parent.PointToScreen(draggedShip.Location);
                Point newFormLocation = this.PointToClient(oldScreenLocation);
                // 2. Chuyển Control cha sang Form chính (Reparenting)
                draggedShip.Parent = this;
                // 3. Đặt lại vị trí đã chuyển đổi
                draggedShip.Location = newFormLocation;
                // 4. Đưa tàu lên trên cùng
                draggedShip.BringToFront();
            }
        }


        private void ship_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && draggedShip != null)
            {
                int newX = draggedShip.Left + e.X - mouseOffset.X;
                int newY = draggedShip.Top + e.Y - mouseOffset.Y;
                draggedShip.Location = new Point(newX, newY);

            }
        }
        private void ship_MouseUp(object sender, MouseEventArgs e)

        {
            if (draggedShip != null)
            {
                Point gridOrigin = pnlGameGrid.Location;
                Point snappedLocation = SnapShipToGrid(draggedShip, gridOrigin);

                if (CheckPlacementRules(draggedShip, snappedLocation))
                {
                    draggedShip.Parent = pnlGameGrid;
                    draggedShip.Location = new Point(
                        snappedLocation.X - pnlGameGrid.Location.X,
                        snappedLocation.Y - pnlGameGrid.Location.Y
                    );

                    int gridX = (draggedShip.Left) / CELL_SIZE;

                    int gridY = (draggedShip.Top) / CELL_SIZE;

                    int length = int.Parse(draggedShip.Tag.ToString());

                    bool isVert = isVertical[draggedShip];

                    PlaceShipOnMatrix(playerGrid, gridX, gridY, length, isVert);

                    string shipName = draggedShip.Name.Replace("pic", "");
                    Ship newPlayerShip = new Ship(shipName);
                    for (int i = 0; i < length; i++)
                    {
                        int currX = isVert ? gridX : gridX + i;
                        int currY = isVert ? gridY + i : gridY;
                        newPlayerShip.Coordinates.Add(new Point(currX, currY));
                    }
                    playerShips.Add(newPlayerShip);
                    if (!placedShips.Contains(draggedShip))
                    {
                        placedShips.Add(draggedShip);
                    }
                    lblStatus.Text = $"Đã đặt {placedShips.Count}/5 tàu.";
                }
                else
                {
                    draggedShip.Parent = pnlShipList;
                    draggedShip.Location = initialShipLocations[draggedShip];
                }
            }
            isDragging = false;
            draggedShip = null;
        }

        private Point SnapShipToGrid(Control ship, Point gridOrigin)

        {
            int absoluteX = ship.Left;
            int absoluteY = ship.Top;
            int gridRelativeX = absoluteX - gridOrigin.X;
            int gridRelativeY = absoluteY - gridOrigin.Y;
            int snapX = (gridRelativeX + CELL_SIZE / 2) / CELL_SIZE * CELL_SIZE;
            int snapY = (gridRelativeY + CELL_SIZE / 2) / CELL_SIZE * CELL_SIZE;
            return new Point(gridOrigin.X + snapX, gridOrigin.Y + snapY);
        }



        private bool CheckPlacementRules(Control ship, Point newLocation)
        {
            int shipRelativeX = newLocation.X - pnlGameGrid.Location.X;
            int shipRelativeY = newLocation.Y - pnlGameGrid.Location.Y;
            Rectangle gridBounds = new Rectangle(0, 0, pnlGameGrid.Width, pnlGameGrid.Height);
            Rectangle shipBounds = new Rectangle(shipRelativeX, shipRelativeY, ship.Width, ship.Height);
            if (!gridBounds.Contains(shipBounds))
            {
                MessageBox.Show("Tàu phải nằm hoàn toàn trong khu vực lưới.", "Lỗi đặt tàu");
                return false;
            }
            return true;
        }

        private void PlaceShipOnMatrix(int[,] grid, int x, int y, int length, bool isVertical)
        {
            for (int i = 0; i < length; i++)
            {
                int currX = isVertical ? x : x + i;
                int currY = isVertical ? y + i : y;
                grid[currX, currY] = 1; // 1 nghĩa là có tàu
            }
        }

        private void DisableSetupControls()
        {
            foreach (var ship in placedShips)
            {
                ship.Enabled = false;
            }
            pnlDeployment.Enabled = false;
        }

        protected override async void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (!string.IsNullOrEmpty(roomID))
                await firebase.Child("Matches").Child(roomID).Child(myRole).DeleteAsync();
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
    }
}