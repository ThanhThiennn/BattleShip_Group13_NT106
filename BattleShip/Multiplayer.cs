using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
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
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;


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
        private Dictionary<string, List<Point>> shipCoordinates = new Dictionary<string, List<Point>>();

        private bool isSetupComplete = false;
        private bool isReady = false;
        

        IFirebaseConfig config = new FirebaseConfig
        {
            AuthSecret = "l9115evyBBSX8xG7xxohZSYPQjA6mEg3QlK2JL3R",
            BasePath = "https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;

        string roomID = "Room_001"; // Sau này sẽ nhận từ Lobby
        string myRole = "Player1";  // Hoặc "Player2" tùy theo lúc vào phòng
        bool isMyTurn = false;
        bool isBattleStarted = false;



        public Multiplayer(string id, string role)
        {
            InitializeComponent();
            this.roomID = id;
            this.myRole = role;

            this.UpdateStyles();
            pnlGameGrid.Paint += pnlGameGrid_Paint;
            pnlBotGrid.Paint += pnlGameGrid_Paint;

            InitShipData();
        }

        private void InitShipData()
        {
            List<Control> shipControls = new List<Control> { picCarrier, picBattleShip, picCruiser1, picCruiser2, picDestroyer };
            foreach (var ship in shipControls)
            {
                if (ship == null) continue;
                if (!initialShipLocations.ContainsKey(ship))
                    initialShipLocations.Add(ship, ship.Location);
                if (!isVertical.ContainsKey(ship))
                    isVertical.Add(ship, false);

                ship.MouseDown += ship_MouseDown;
                ship.MouseMove += ship_MouseMove;
                ship.MouseUp += ship_MouseUp;
            }
        }
        private void Multiplayer_Load(object sender, EventArgs e)
        {
            try
            {
                client = new FireSharp.FirebaseClient(config);
                if (client != null)
                {
                    lblStatus.Text = "Đã kết nối Firebase. Hãy đặt tàu!";
                    ListenToRoom(); // Bắt đầu lắng nghe thay đổi từ đối thủ
                }
            }
            catch
            {
                MessageBox.Show("Kết nối thất bại, vui lòng kiểm tra mạng hoặc Secret Key!");
            }
            MessageBox.Show("Room: " + roomID + " | Role: " + myRole);
        }
        private async void ListenToRoom()
        {
            if (client == null) return;

            // --- PHẦN 1: LẤY DỮ LIỆU LẦN ĐẦU ---
            var response = await client.GetAsync($"Rooms/{roomID}");
            if (response.Body != "null")
            {
                var currentRoom = response.ResultAs<Room>();
                UpdatePlayerListUI(currentRoom);
            }

            // --- PHẦN 2: LẮNG NGHE THAY ĐỔI PHÒNG (Sửa lại hoàn chỉnh) ---
            client.OnAsync($"Rooms/{roomID}", (sender, args, context) => {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;
                string rawData = args.Data.Trim();
                if (!rawData.StartsWith("{")) return;

                try
                {
                    var roomData = JsonConvert.DeserializeObject<Room>(rawData);

                    // CƠ CHẾ SELF-HEAL: Nếu mất dữ liệu Player, chủ động Get lại toàn bộ phòng
                    if (roomData?.Player1 == null || roomData?.Player2 == null)
                    {
                        var reload = client.Get($"Rooms/{roomID}");
                        roomData = reload.ResultAs<Room>();
                    }

                    if (roomData == null) return;

                    this.Invoke(new Action(() => {
                        UpdatePlayerListUI(roomData);

                        // KIỂM TRA ĐIỀU KIỆN VÀO TRẬN
                        if (roomData.Player1 != null && roomData.Player2 != null)
                        {
                            bool p1 = roomData.Player1.IsReady;
                            bool p2 = roomData.Player2.IsReady;

                            System.Diagnostics.Debug.WriteLine($"DEBUG: P1={p1}, P2={p2}");

                            if (p1 && p2 && !isBattleStarted)
                            {
                                // Đổi trạng thái sang playing (chỉ Player1 gửi để tránh xung đột)
                                if (myRole == "Player1")
                                {
                                    client.SetAsync($"Rooms/{roomID}/Status", "playing");
                                }

                                StartBattle(roomData.Turn ?? "Player1");
                            }
                        }
                    }));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Lỗi OnAsync: " + ex.Message);
                }
            });

            // --- PHẦN 3: LẮNG NGHE LƯỢT CHƠI (Turn) ---
            client.OnAsync($"Rooms/{roomID}/Turn", (sender, args, context) => {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;

                string currentTurn = args.Data.Replace("\"", "").Trim();

                this.Invoke(new Action(() => {
                    isMyTurn = (currentTurn == myRole);
                    if (isBattleStarted) // Chỉ hiện thông báo lượt khi đã vào trận
                    {
                        if (isMyTurn)
                        {
                            lblStatus.Text = "Đến lượt bạn bắn!";
                            lblStatus.ForeColor = Color.Lime;
                        }
                        else
                        {
                            lblStatus.Text = "Đối thủ đang suy nghĩ...";
                            lblStatus.ForeColor = Color.White;
                        }
                    }
                }));
            });

            // --- PHẦN 4: LẮNG NGHE CÚ BẮN TỪ ĐỐI THỦ (LastShot) ---
            client.OnAsync($"Rooms/{roomID}/LastShot", (sender, args, context) => {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;

                var shot = JsonConvert.DeserializeObject<dynamic>(args.Data);
                if (shot.Attacker == myRole) return; // Bỏ qua nếu mình là người bắn

                int x = (int)shot.X;
                int y = (int)shot.Y;

                // Kiểm tra trúng/trượt trên lưới của mình
                string result = (playerGrid[x, y] == 1) ? "Hit" : "Miss";
                if (result == "Hit") playerGrid[x, y] = 2; // Đánh dấu ô tàu đã bị bắn trúng

                SendResponse(x, y, result); // Gửi phản hồi kết quả cho đối thủ

                this.Invoke(new Action(() => {
                    ShowHitMarker(pnlGameGrid, x, y, result); // Hiện dấu X/O lên lưới trái
                    if (result == "Hit") CheckIfMyShipSunk(x, y); // Kiểm tra tàu chìm
                }));
            });

            // --- PHẦN 5: LẮNG NGHE PHẢN HỒI KẾT QUẢ CÚ BẮN CỦA MÌNH (LastResponse) ---
            client.OnAsync($"Rooms/{roomID}/LastResponse", (sender, args, context) => {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;

                var res = JsonConvert.DeserializeObject<dynamic>(args.Data);
                if (res.Responder == myRole) return;

                this.Invoke(new Action(() => {
                    ShowHitMarker(pnlBotGrid, (int)res.X, (int)res.Y, (string)res.Result);
                    // Lưu kết quả vào mảng để không bắn lại ô cũ
                    botGrid[(int)res.X, (int)res.Y] = (res.Result == "Hit") ? 2 : 3;
                }));
            });
        }

        private void UpdatePlayerListUI(Room room)
        {
            if (room == null) return;

            // Sử dụng Invoke để đảm bảo cập nhật UI từ luồng khác không gây crash
            if (this.lstPlayers.InvokeRequired)
            {
                this.lstPlayers.Invoke(new Action(() => UpdatePlayerListUI(room)));
                return;
            }

            lstPlayers.Items.Clear();

            // Hiển thị Player 1
            if (room.Player1 != null)
            {
                // Nếu tên trống thì hiện "Đang chờ..." thay vì để trắng
                string p1Name = string.IsNullOrEmpty(room.Player1.Name) ? "Người chơi 1" : room.Player1.Name;
                string p1Ready = room.Player1.IsReady ? " [Sẵn sàng]" : "";
                lstPlayers.Items.Add(p1Name + p1Ready);
            }

            // Hiển thị Player 2
            if (room.Player2 != null)
            {
                string p2Name = string.IsNullOrEmpty(room.Player2.Name) ? "Người chơi 2" : room.Player2.Name;
                string p2Ready = room.Player2.IsReady ? " [Sẵn sàng]" : "";
                lstPlayers.Items.Add(p2Name + p2Ready);
            }
        }
        private async void SendResponse(int x, int y, string result)
        {
            var responseData = new
            {
                Responder = myRole, 
                X = x,
                Y = y,
                Result = result, 
                Time = DateTime.Now.Ticks.ToString()
            };

            await client.SetAsync($"Rooms/{roomID}/LastResponse", responseData);
        }

        private async void pnlBotGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isBattleStarted || !isMyTurn) return;

            int x = e.X / CELL_SIZE;
            int y = e.Y / CELL_SIZE;

            if (x >= GRID_SIZE || y >= GRID_SIZE || botGrid[x, y] != 0) return;

            // 1. Tạm thời khóa lượt bắn
            isMyTurn = false;
            lblStatus.Text = "Đang bắn...";

            // 2. Gửi cú bắn
            var shotData = new { Attacker = myRole, X = x, Y = y, Time = DateTime.Now.Ticks.ToString() };
            await client.SetAsync($"Rooms/{roomID}/LastShot", shotData);

            // 3. CHUYỂN LƯỢT SANG ĐỐI THỦ (Quan trọng)
            string nextTurn = (myRole == "Player1") ? "Player2" : "Player1";
            await client.SetAsync($"Rooms/{roomID}/Turn", nextTurn);
        }


        private void StartBattle(string Turn)
        {
            isBattleStarted = true; // Đánh dấu game đã bắt đầu

            // 1. Chuyển đổi giao diện
            pnlDeployment.Visible = false;  // Ẩn bảng xám đặt tàu
            pnlBotGrid.Visible = true;      // Hiện bảng xanh để bắn đối thủ

            // 2. Xác định ai bắn trước
            isMyTurn = (Turn == myRole);

            // 3. Cập nhật thông báo
            if (isMyTurn)
            {
                lblStatus.Text = "TRẬN ĐẤU BẮT ĐẦU! Đến lượt bạn bắn.";
                lblStatus.ForeColor = Color.Lime;
            }
            else
            {
                lblStatus.Text = "TRẬN ĐẤU BẮT ĐẦU! Đối thủ bắn trước.";
                lblStatus.ForeColor = Color.White;
            }
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
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => EnablePlacement(enable)));
                return;
            }
            if (pnlDeployment != null) pnlDeployment.Enabled = enable;
            foreach (var ship in initialShipLocations.Keys)
            {
                if (ship != null) ship.Enabled = enable;
            }
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
            parent.Controls.Add(marker);
            marker.BringToFront();
        }

        private async void CheckIfMyShipSunk(int x, int y)
        {
            foreach (var ship in shipCoordinates)
            {
                // Kiểm tra xem tọa độ vừa bị bắn có thuộc con tàu này không
                if (ship.Value.Contains(new Point(x, y)))
                {
                    // Kiểm tra xem tất cả các ô của tàu này đã bị đối thủ bắn trúng chưa
                    // (Dựa trên playerGrid: nếu trúng rồi bạn có thể đánh dấu playerGrid[x,y] = 2)
                    bool allHit = ship.Value.All(p => playerGrid[p.X, p.Y] == 2);

                    if (allHit)
                    {
                        // Gửi lệnh báo tàu này đã chìm lên Firebase
                        await client.SetAsync($"Rooms/{roomID}/{myRole}/Sunk/{ship.Key}", true);

                        // Tự làm xám tàu mình ở hàng dưới
                        SyncShipSunkUI(myRole, ship.Key);
                    }
                    break;
                }
            }
        }

        private void SyncShipSunkUI(string role, string shipName)
        {
            // Quy tắc đặt tên của bạn: pbPlayerCarrier hoặc pbBotCarrier
            string pbName = (role == myRole) ? "pbPlayer" + shipName : "pbBot" + shipName;

            // Tìm PictureBox trong danh sách Control của Form
            Control[] controls = this.Controls.Find(pbName, true);
            if (controls.Length > 0 && controls[0] is PictureBox pb)
            {
                this.Invoke(new Action(() => {
                    MakeGrayscaleAndSink(pb);
                }));
            }
        }

        private void MakeGrayscaleAndSink(PictureBox pb)
        {
            if (pb.Image == null) return;
            Bitmap bmp = new Bitmap(pb.Image);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            Bitmap grayImage = new Bitmap(bmp.Width, bmp.Height);
            using (Graphics g = Graphics.FromImage(grayImage))
            {
                System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
                    new float[][] {
         new float[] {.3f, .3f, .3f, 0, 0},
         new float[] {.59f, .59f, .59f, 0, 0},
         new float[] {.11f, .11f, .11f, 0, 0},
         new float[] {0, 0, 0, 1, 0},
         new float[] {0, 0, 0, 0, 1}
                    });
                using (var attributes = new System.Drawing.Imaging.ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height),
                        0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            pb.Image = grayImage;
            pb.Enabled = false;
        }

        private void EndGame(string message)
        {
            isReady = false;
            lblStatus.Text = message;
            MessageBox.Show(message);
        }

        private void ship_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return;

            draggedShip = (Control)sender;
            mouseOffset = e.Location;
            isDragging = true;

            Point screenPos = draggedShip.Parent.PointToScreen(draggedShip.Location);
            Point formPos = this.PointToClient(screenPos);

            draggedShip.Parent = this;
            draggedShip.Location = formPos;
            draggedShip.BringToFront();
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
            if (draggedShip == null) return;

            // 1. Tính toán vị trí Snap vào Grid
            Point gridScreen = pnlGameGrid.PointToScreen(Point.Empty);
            Point shipScreen = draggedShip.PointToScreen(Point.Empty);

            int relativeX = shipScreen.X - gridScreen.X;
            int relativeY = shipScreen.Y - gridScreen.Y;

            int snapX = (relativeX + CELL_SIZE / 2) / CELL_SIZE * CELL_SIZE;
            int snapY = (relativeY + CELL_SIZE / 2) / CELL_SIZE * CELL_SIZE;

            // 2. Kiểm tra giới hạn (Bounds Check)
            if (snapX < 0 || snapY < 0 ||
                snapX + draggedShip.Width > pnlGameGrid.Width ||
                snapY + draggedShip.Height > pnlGameGrid.Height)
            {
                MessageBox.Show("Tàu phải nằm hoàn toàn trong lưới!", "Lỗi");
                draggedShip.Parent = pnlDeployment;
                draggedShip.Location = initialShipLocations[draggedShip];
                ResetDrag();
                return;
            }

            // 3. Cập nhật vị trí UI
            draggedShip.Parent = pnlGameGrid;
            draggedShip.Location = new Point(snapX, snapY);

            // 4. Lưu logic vào mảng và Dictionary tọa độ
            int gridX = snapX / CELL_SIZE;
            int gridY = snapY / CELL_SIZE;
            int length = int.Parse(draggedShip.Tag.ToString());
            bool isVert = isVertical[draggedShip];
            string shipType = draggedShip.Name.Replace("pic", ""); // Lấy tên như "Carrier", "BattleShip"

            // Xóa dữ liệu cũ của tàu này nếu nó đã được đặt trước đó
            if (shipCoordinates.ContainsKey(shipType))
            {
                foreach (Point p in shipCoordinates[shipType]) playerGrid[p.X, p.Y] = 0;
            }

            List<Point> coords = new List<Point>();
            for (int i = 0; i < length; i++)
            {
                int currX = isVert ? gridX : gridX + i;
                int currY = isVert ? gridY + i : gridY;

                playerGrid[currX, currY] = 1; // 1: Có tàu
                coords.Add(new Point(currX, currY));
            }

            shipCoordinates[shipType] = coords; // Lưu danh sách ô để check Sunk

            if (!placedShips.Contains(draggedShip)) placedShips.Add(draggedShip);
            lblStatus.Text = $"Đã đặt {placedShips.Count}/5 tàu.";
            ResetDrag();
        }
        private void ResetDrag()
        {
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
            Rectangle gridBounds = new Rectangle(pnlGameGrid.Location.X, pnlGameGrid.Location.Y,
                                                 pnlGameGrid.Width, pnlGameGrid.Height);
            Rectangle shipBounds = new Rectangle(newLocation.X, newLocation.Y,
                                                 ship.Width, ship.Height);
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

        private async void btnReady_Click_1(object sender, EventArgs e)
        {
            if (placedShips.Count < 5)
            {
                MessageBox.Show($"Bạn mới đặt {placedShips.Count}/5 tàu. Vui lòng đặt đủ!");
                return;
            }

            if (string.IsNullOrEmpty(roomID) || string.IsNullOrEmpty(myRole))
            {
                MessageBox.Show("Lỗi: Không tìm thấy thông tin phòng!");
                return;
            }

            try
            {
                // Gửi trạng thái IsReady trực tiếp vào node của Player đó
                string path = $"Rooms/{roomID}/{myRole}/IsReady";
                var response = await client.SetAsync(path, true);

                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    btnReady.Enabled = false;
                    btnRandom.Enabled = false; // Khóa nút Random tàu luôn
                    lblStatus.Text = "Đã sẵn sàng! Đang đợi đối thủ...";
                    lblStatus.ForeColor = Color.Yellow;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi kết nối Firebase: " + ex.Message);
            }
        }
    }
}