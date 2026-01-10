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
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


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
        int totalShots = 0;

        private List<Ship> playerShips = new List<Ship>();
        private List<Control> placedShips = new List<Control>();
        private Dictionary<Control, Point> initialShipLocations = new Dictionary<Control, Point>();
        private Dictionary<Control, bool> isVertical = new Dictionary<Control, bool>();
        private Dictionary<string, List<Point>> shipCoordinates = new Dictionary<string, List<Point>>();
        private HashSet<string> handledResponses = new HashSet<string>();
        private HashSet<string> handledShots = new HashSet<string>();
        private List<ShotResponse> pendingResponses = new List<ShotResponse>();
        private bool isShowingSpecialMessage = false;

        private bool isReady = false;

        // --- CÁC BIẾN MỚI CHO AVATAR VÀ STATUS ---
        private Timer _blinkTimer;
        private int _dotCount = 0;
        private Room _currentRoomData; // Lưu dữ liệu phòng để Timer dùng
        private List<Image> _avatars = new List<Image>()
        {
            Properties.Resources.avt1, Properties.Resources.avt2,
            Properties.Resources.avt3, Properties.Resources.avt4,
            Properties.Resources.avt5, Properties.Resources.avt6,
            Properties.Resources.avt7, Properties.Resources.avt8
        };
        // ------------------------------------------

        IFirebaseConfig config = new FirebaseConfig
        {
            BasePath = "https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app/"
        };
        IFirebaseClient client;

        string roomID = "Room_001";
        string myRole = "Player1";
        bool isMyTurn = false;
        bool isBattleStarted = false;


        public Multiplayer(string id, string role)
        {
            InitializeComponent();
            this.roomID = id;
            this.myRole = role;

            // --- KHỞI TẠO TIMER NHẤP NHÁY ---
            _blinkTimer = new Timer();
            _blinkTimer.Interval = 500; // 0.5 giây
            _blinkTimer.Tick += BlinkTimer_Tick;
            _blinkTimer.Start();
            // --------------------------------

            this.UpdateStyles();
            pnlGameGrid.Paint += pnlGameGrid_Paint;
            pnlBotGrid.Paint += pnlGameGrid_Paint;
            pnlBotGrid.MouseClick += pnlBotGrid_MouseClick;
            this.KeyPreview = true;
            this.KeyUp += Multiplayer_KeyUp;

            InitShipData();
        }

        // --- SỰ KIỆN TIMER ---
        private void BlinkTimer_Tick(object sender, EventArgs e)
        {
            _dotCount = (_dotCount + 1) % 4;

            // Nếu đã có dữ liệu phòng, liên tục cập nhật UI (để tạo hiệu ứng nhấp nháy cho người chưa Ready)
            if (_currentRoomData != null)
            {
                UpdatePlayerStatusUI(_currentRoomData.Player1, lblUserNameP1, avtPlayer1);
                UpdatePlayerStatusUI(_currentRoomData.Player2, lblUserNameP2, avtPlayer2);
            }
        }
        // ---------------------

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

        private async void Multiplayer_Load(object sender, EventArgs e)
        {
            string testKey = Environment.GetEnvironmentVariable("FIREBASE_SECRET_KEY", EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(testKey))
            {
                MessageBox.Show("CẢNH BÁO: Máy chưa nhận biến môi trường FIREBASE_SECRET_KEY!");
            }
            else
            {
                config.AuthSecret = testKey;
            }

            try
            {
                client = new FireSharp.FirebaseClient(config);

                if (client != null)
                {
                    lblStatus.Text = "Đã kết nối. Đang đồng bộ...";

                    ListenToRoom();

                    // Lấy dữ liệu lần đầu
                    var response = await client.GetAsync($"Rooms/{roomID}");
                    if (response != null && response.Body != "null")
                    {
                        var currentRoom = response.ResultAs<Room>();
                        ProcessRoomUpdate(currentRoom); // Gọi hàm xử lý chung
                    }

                    lblStatus.Text = "Kết nối thành công. Hãy đặt tàu!";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Kết nối thất bại: " + ex.Message);
            }
            this.ClientSize = new Size(1334, 670);
            this.CenterToScreen();
        }

        private void ListenToRoom()
        {
            if (client == null) return;

            client.OnAsync($"Rooms/{roomID}", (sender, args, context) =>
            {
                _ = RefreshDataFromServer();
            });

            // Lắng nghe lượt chơi (Turn) - ĐÃ SỬA LỖI BIẾN response
            client.OnAsync($"Rooms/{roomID}/Turn", async (sender, args, context) => {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;
                string currentTurn = args.Data.Replace("\"", "").Trim();

                // Lấy lại dữ liệu phòng mới nhất để cập nhật UI
                var resp = await client.GetAsync($"Rooms/{roomID}");
                var room = resp.ResultAs<Room>();
                _currentRoomData = room;

                this.Invoke(new Action(() => {
                    // Cập nhật giao diện Avatar/Tên
                    UpdatePlayerStatusUI(room.Player1, lblUserNameP1, avtPlayer1);
                    UpdatePlayerStatusUI(room.Player2, lblUserNameP2, avtPlayer2);

                    bool oldTurn = isMyTurn;
                    isMyTurn = (currentTurn == myRole);

                    if (isBattleStarted)
                    {
                        if (isMyTurn)
                        {
                            lblStatus.Text = "Đến lượt bạn bắn!";
                            lblStatus.ForeColor = Color.Lime;
                            pnlBotGrid.Enabled = true;
                        }
                        else
                        {
                            lblStatus.Text = "Đối thủ đang suy nghĩ...";
                            lblStatus.ForeColor = Color.Black;
                        }
                    }
                }));
            });

            // Polling Shots
            System.Threading.Tasks.Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (isBattleStarted) await CheckForNewShots();
                        await System.Threading.Tasks.Task.Delay(500);
                    }
                    catch { }
                }
            });

            // Polling Responses
            System.Threading.Tasks.Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (isBattleStarted) await CheckForNewResponses();
                        await System.Threading.Tasks.Task.Delay(500);
                    }
                    catch { }
                }
            });

            // Lắng nghe tàu chìm
            string opponentRole = (myRole == "Player1") ? "Player2" : "Player1";
            client.OnAsync($"Rooms/{roomID}/{opponentRole}/SunkShips", (sender, args, context) =>
            {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;
                try
                {
                    string shipName = args.Path.Replace("/", "");
                    if (string.IsNullOrEmpty(shipName))
                    {
                        var sunkShips = JsonConvert.DeserializeObject<Dictionary<string, bool>>(args.Data);
                        foreach (var ship in sunkShips)
                        {
                            if (ship.Value) HandleShipSunkNotification(opponentRole, ship.Key);
                        }
                    }
                    else
                    {
                        if (args.Data.ToLower().Contains("true"))
                        {
                            HandleShipSunkNotification(opponentRole, shipName);
                        }
                    }
                }
                catch { }
            });
        }
        private void HandleOpponentQuit()
        {
            // Ngăn chặn việc FormClosing chạy lại logic xóa phòng một lần nữa (vì phòng đã bị xóa rồi)
            roomID = null;

            MessageBox.Show("Đối thủ đã thoát trận. Phòng đã bị đóng!", "Thông báo");

            // Mở lại form CreateMatch và đóng form hiện tại
            CreateMatch createMatchForm = new CreateMatch();
            createMatchForm.Show();
            this.Close();
        }
        private void HandleShipSunkNotification(string role, string shipName)
        {
            string opponentRole = (myRole == "Player1") ? "Player2" : "Player1";
            this.Invoke(new Action(() => {
                SyncShipSunkUI(role, shipName);
                if (role == opponentRole)
                    UpdateStatusWithPriority($"TUYỆT VỜI! Bạn đã bắn hạ tàu {shipName}!", Color.Red, 3000);
                else
                    UpdateStatusWithPriority($"CẨN THẬN! Tàu {shipName} của bạn đã bị chìm!", Color.Red, 3000);
            }));
        }

        private async System.Threading.Tasks.Task CheckForNewShots()
        {
            try
            {
                var response = await client.GetAsync($"Rooms/{roomID}/Shots");
                if (response == null || response.Body == "null") return;

                var shots = JsonConvert.DeserializeObject<Dictionary<string, Shot>>(response.Body);
                if (shots == null) return;

                foreach (var kv in shots)
                {
                    string shotId = kv.Key;
                    Shot shot = kv.Value;

                    if (shot.Attacker == myRole) continue;
                    if (handledShots.Contains(shotId)) continue;

                    handledShots.Add(shotId);

                    this.Invoke(new Action(() =>
                    {
                        int x = shot.X;
                        int y = shot.Y;
                        string result = (playerGrid[x, y] == 1) ? "Hit" : "Miss";
                        if (result == "Hit")
                        {
                            playerGrid[x, y] = 2;
                            CheckIfMyShipSunk(x, y);
                        }
                        ShowHitMarker(pnlGameGrid, x, y, result);
                        _ = SendResponseAsync(shotId, x, y, result);

                        int myRemainingCells = 0;
                        for (int i = 0; i < 10; i++)
                            for (int j = 0; j < 10; j++)
                                if (playerGrid[i, j] == 1) myRemainingCells++;

                        if (myRemainingCells == 0) ShowFinalResult("DEFEAT");
                    }));
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Lỗi CheckForNewShots: {ex.Message}"); }
        }

        private async System.Threading.Tasks.Task CheckForNewResponses()
        {
            try
            {
                var response = await client.GetAsync($"Rooms/{roomID}/Responses");
                if (response == null || response.Body == "null") return;

                var responses = JsonConvert.DeserializeObject<Dictionary<string, ShotResponse>>(response.Body);
                if (responses == null) return;

                foreach (var kv in responses)
                {
                    string responseId = kv.Key;
                    ShotResponse res = kv.Value;

                    if (res.Responder == myRole || handledResponses.Contains(responseId)) continue;
                    handledResponses.Add(responseId);

                    this.Invoke(new Action(() =>
                    {
                        ShowHitMarker(pnlBotGrid, res.X, res.Y, res.Result);
                        botGrid[res.X, res.Y] = (res.Result == "Hit") ? 2 : 3;

                        if (res.Result == "Hit")
                        {
                            playerHits++;
                            totalShots++;
                            UpdateStatusWithPriority($"Bạn đã bắn trúng! ({playerHits}/{TOTAL_SHIP_CELLS})", Color.OrangeRed);
                            lblStatus.ForeColor = Color.OrangeRed;
                            isMyTurn = true;
                            if (playerHits >= 17) ShowFinalResult("VICTORY");
                        }
                        else
                        {
                            totalShots++;
                            lblStatus.Text = "Hụt rồi! Đợi đối thủ...";
                            lblStatus.ForeColor = Color.Black;
                            isMyTurn = false;
                        }
                    }));
                }
            }
            catch (Exception ex) { System.Diagnostics.Debug.WriteLine($"Lỗi CheckForNewResponses: {ex.Message}"); }
        }

        private void ShowFinalResult(string status)
        {
            this.Invoke(new Action(() => {
                isBattleStarted = false;
                using (MatchResult resultForm = new MatchResult(status, totalShots, "Đối thủ"))
                {
                    if (resultForm.ShowDialog() == DialogResult.OK)
                    {
                        this.DialogResult = DialogResult.OK;
                        this.Close();
                    }
                }
            }));
        }

        private async System.Threading.Tasks.Task SendResponseAsync(string shotId, int x, int y, string result)
        {
            try
            {
                var responseData = new { Responder = myRole, X = x, Y = y, Result = result };
                await client.SetAsync($"Rooms/{roomID}/Responses/{shotId}", responseData);

                string attackerRole = (myRole == "Player1") ? "Player2" : "Player1";
                string nextTurn = (result == "Hit") ? attackerRole : myRole;

                await client.SetAsync($"Rooms/{roomID}/Turn", nextTurn);

                this.Invoke(new Action(() => {
                    isMyTurn = (nextTurn == myRole);
                    if (isMyTurn)
                    {
                        lblStatus.Text = "Đối thủ bắn hụt! Đến lượt bạn.";
                        lblStatus.ForeColor = Color.Lime;
                    }
                    else if (result == "Hit")
                    {
                        lblStatus.Text = "Bạn bị bắn trúng! Đối thủ bắn tiếp.";
                        lblStatus.ForeColor = Color.Red;
                    }
                }));

                _ = System.Threading.Tasks.Task.Run(async () => {
                    await System.Threading.Tasks.Task.Delay(2000);
                    await client.DeleteAsync($"Rooms/{roomID}/Shots/{shotId}");
                });
            }
            catch (Exception ex) { MessageBox.Show("Loi: " + ex); }
        }

        private async Task RefreshDataFromServer()
        {
            try
            {
                var response = await client.GetAsync($"Rooms/{roomID}");
                if (response != null && response.Body != "null")
                {
                    var roomData = response.ResultAs<Room>();
                    if (roomData != null)
                    {
                        this.Invoke(new Action(() => ProcessRoomUpdate(roomData)));
                    }
                }
            }
            catch { }
        }

        private void ProcessRoomUpdate(Room roomData)
        {
            _currentRoomData = roomData; // Lưu lại dữ liệu cho Timer dùng

            // Cập nhật giao diện Avatar và Tên (MỚI)
            UpdatePlayerStatusUI(roomData.Player1, lblUserNameP1, avtPlayer1);
            UpdatePlayerStatusUI(roomData.Player2, lblUserNameP2, avtPlayer2);

            if (roomData.Player1 != null && roomData.Player2 != null)
            {
                bool p1Ready = roomData.Player1.IsReady;
                bool p2Ready = roomData.Player2.IsReady;

                if (p1Ready && p2Ready && !isBattleStarted)
                {
                    isBattleStarted = true;
                    if (myRole == "Player1")
                    {
                        Task.Run(() => client.SetAsync($"Rooms/{roomID}/Status", "playing"));
                    }
                    StartBattle(roomData.Turn ?? "Player1");
                }
            }
        }
    
        // --- HÀM CẬP NHẬT GIAO DIỆN AVATAR VÀ TÊN (MỚI) ---
        private void UpdatePlayerStatusUI(PlayerData player, Label lblName, PictureBox picAvatar)
        {
            if (player == null)
            {
                lblName.Text = "Waiting...";
                lblName.ForeColor = Color.Gray;
                picAvatar.Image = null;
                return;
            }

            // 1. Cập nhật Avatar
            if (player.AvatarId >= 0 && player.AvatarId < _avatars.Count)
            {
                picAvatar.Image = _avatars[player.AvatarId];
            }

            // 2. Cập nhật Trạng thái và Màu sắc
            if (player.IsReady)
            {
                // SẴN SÀNG: Chữ xanh, tĩnh
                lblName.Text = player.Name;
                lblName.ForeColor = Color.LimeGreen;
            }
            else
            {
                // CHƯA SẴN SÀNG: Chữ trắng, nhấp nháy dấu ...
                string dots = new string('.', _dotCount);
                lblName.Text = player.Name + dots;
                lblName.ForeColor = Color.White;
            }
        }
        // --------------------------------------------------

        private async void pnlBotGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isBattleStarted || !isMyTurn)
            {
                System.Diagnostics.Debug.WriteLine("❌ Không được phép bắn!");
                return;
            }
            int x = e.X / CELL_SIZE;
            int y = e.Y / CELL_SIZE;

            if (x >= GRID_SIZE || y >= GRID_SIZE || botGrid[x, y] != 0) return;

            isMyTurn = false;
            lblStatus.Text = "Đang bắn...";

            string shotId = Guid.NewGuid().ToString();
            var shotData = new Shot { Attacker = myRole, X = x, Y = y };

            try { await client.SetAsync($"Rooms/{roomID}/Shots/{shotId}", shotData); }
            catch { isMyTurn = true; }
        }

        private void StartBattle(string Turn)
        {
            isBattleStarted = true;
            Array.Clear(botGrid, 0, botGrid.Length);
            pnlDeployment.Visible = false;
            pnlDeployment.Enabled = false;
            pnlBotGrid.Visible = true;
            pnlBotGrid.Enabled = true;
            pnlBotGrid.BringToFront();
            pnlBotGrid.Focus();
            isMyTurn = (Turn == myRole);
            if (isMyTurn)
            {
                lblStatus.Text = "TRẬN ĐẤU BẮT ĐẦU! Đến lượt bạn bắn.";
                lblStatus.ForeColor = Color.Lime;
            }
            else
            {
                lblStatus.Text = "TRẬN ĐẤU BẮT ĐẦU! Đối thủ bắn trước.";
                lblStatus.ForeColor = Color.Black;
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
            parent.Invalidate();
            parent.Update();
        }

        private async void CheckIfMyShipSunk(int x, int y)
        {
            foreach (var ship in shipCoordinates)
            {
                if (ship.Value.Contains(new Point(x, y)))
                {
                    bool allHit = ship.Value.All(p => playerGrid[p.X, p.Y] == 2);
                    if (allHit)
                    {
                        string sunkShipName = ship.Key;
                        await client.SetAsync($"Rooms/{roomID}/{myRole}/SunkShips/{sunkShipName}", true);
                        SyncShipSunkUI(myRole, sunkShipName);
                    }
                    break;
                }
            }
        }

        private async void UpdateStatusWithPriority(string message, Color color, int delayMs = 0)
        {
            this.Invoke(new Action(async () => {
                if (isShowingSpecialMessage && delayMs == 0) return;
                lblStatus.Text = message;
                lblStatus.ForeColor = color;
                if (delayMs > 0)
                {
                    isShowingSpecialMessage = true;
                    await System.Threading.Tasks.Task.Delay(delayMs);
                    isShowingSpecialMessage = false;
                }
            }));
        }

        private void SyncShipSunkUI(string role, string shipName)
        {
            string pbName = (role == myRole) ? "pbPlayer" + shipName : "pbBot" + shipName;
            Control[] controls = this.Controls.Find(pbName, true);
            if (controls.Length > 0 && controls[0] is PictureBox pb)
            {
                if (pb.Enabled)
                {
                    MakeGrayscaleAndSink(pb);
                    pb.Enabled = false;
                }
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
            if (isReady || e.Button != MouseButtons.Left) return;
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
            if (isReady) return;
            if (isDragging && draggedShip != null)
            {
                int newX = draggedShip.Left + e.X - mouseOffset.X;
                int newY = draggedShip.Top + e.Y - mouseOffset.Y;
                draggedShip.Location = new Point(newX, newY);
            }
        }

        private void ship_MouseUp(object sender, MouseEventArgs e)
        {
            if (isReady || draggedShip == null) return;
            if (draggedShip == null) return;
            Point gridScreen = pnlGameGrid.PointToScreen(Point.Empty);
            Point shipScreen = draggedShip.PointToScreen(Point.Empty);
            int relativeX = shipScreen.X - gridScreen.X;
            int relativeY = shipScreen.Y - gridScreen.Y;
            int snapX = (relativeX + CELL_SIZE / 2) / CELL_SIZE * CELL_SIZE;
            int snapY = (relativeY + CELL_SIZE / 2) / CELL_SIZE * CELL_SIZE;

            if (snapX < 0 || snapY < 0 ||
                snapX + draggedShip.Width > pnlGameGrid.Width ||
                snapY + draggedShip.Height > pnlGameGrid.Height)
            {
                ReturnShipToStart();
                return;
            }

            int gridX = snapX / CELL_SIZE;
            int gridY = snapY / CELL_SIZE;
            int length = int.Parse(draggedShip.Tag.ToString());
            bool isVert = isVertical[draggedShip];
            string shipType = draggedShip.Name.Replace("pic", "");

            List<Point> oldCoords = new List<Point>();
            if (shipCoordinates.ContainsKey(shipType))
            {
                oldCoords = new List<Point>(shipCoordinates[shipType]);
                foreach (Point p in oldCoords) playerGrid[p.X, p.Y] = 0;
            }

            if (IsOverlapping(gridX, gridY, length, !isVert))
            {
                MessageBox.Show("Không được đặt chồng tàu lên nhau!", "Cảnh báo");
                foreach (Point p in oldCoords) playerGrid[p.X, p.Y] = 1;
                ReturnShipToStart();
                return;
            }

            draggedShip.Parent = pnlGameGrid;
            draggedShip.Location = new Point(snapX, snapY);

            initialShipLocations[draggedShip] = new Point(snapX, snapY);

            List<Point> coords = new List<Point>();
            for (int i = 0; i < length; i++)
            {
                int currX = isVert ? gridX : gridX + i;
                int currY = isVert ? gridY + i : gridY;
                playerGrid[currX, currY] = 1;
                coords.Add(new Point(currX, currY));
            }
            shipCoordinates[shipType] = coords;
            if (!placedShips.Contains(draggedShip)) placedShips.Add(draggedShip);
            lblStatus.Text = $"Đã đặt {placedShips.Count}/5 tàu.";
            ResetDrag();
        }

        private void ReturnShipToStart()
        {
            if (draggedShip == null) return;

            if (placedShips.Contains(draggedShip))
            {
                draggedShip.Parent = pnlGameGrid;
                draggedShip.Location = initialShipLocations[draggedShip];
            }
            else
            {
                draggedShip.Parent = pnlDeployment;
                draggedShip.Location = initialShipLocations[draggedShip];
            }

            ResetDrag();
        }

        private bool IsOverlapping(int startX, int startY, int length, bool isHorizontal)
        {
            for (int i = 0; i < length; i++)
            {
                int checkX = isHorizontal ? startX + i : startX;
                int checkY = isHorizontal ? startY : startY + i;
                if (playerGrid[checkX, checkY] == 1) return true;
            }
            return false;
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
                grid[currX, currY] = 1;
            }
        }

        private void DisableSetupControls()
        {
            foreach (var ship in placedShips) ship.Enabled = false;
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

        private async void btnReady_Click(object sender, EventArgs e)
        {
            if (placedShips.Count < 5)
            {
                MessageBox.Show($"Bạn mới đặt {placedShips.Count}/5 tàu. Vui lòng đặt đủ!");
                return;
            }
            btnReady.Enabled = false;

            try
            {
                var data = new { IsReady = true };
                var response = await client.UpdateAsync($"Rooms/{roomID}/{myRole}", data);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    this.isReady = true;
                    lblStatus.Text = "Đã sẵn sàng! Đang đợi đối thủ...";
                    lblStatus.ForeColor = Color.Yellow;
                    _ = RefreshDataFromServer();
                }
            }
            catch (Exception ex)
            {
                btnReady.Enabled = true;
                MessageBox.Show("Lỗi Ready: " + ex.Message);
            }
        }

        private void Chat_Click(object sender, EventArgs e)
        {
            
            Chat chatForm = new Chat(this.roomID, this.myRole, this.client);
            chatForm.Show();
        }

        private async void Multiplayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Xác nhận trước khi thoát (Tùy chọn)
            if (string.IsNullOrEmpty(roomID)) return;
            var result = MessageBox.Show("Bạn có chắc chắn muốn rời trận đấu?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy việc đóng form
                return;
            }

            try
            {
                if (client != null)
                {
                    // Bất kể là Player1 hay Player2 thoát, xóa sạch cả node Rooms/{roomID}
                    string currentRoom = roomID;
                    roomID = null; // Đánh dấu để tránh lặp logic
                    await client.DeleteAsync($"Rooms/{currentRoom}");
                    await client.DeleteAsync($"Chats/{currentRoom}");

                    CreateMatch formMatch = new CreateMatch();
                    formMatch.Show();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Lỗi khi đóng phòng: " + ex.Message);
            }
        }
    }
}