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

        //private bool isSetupComplete = false;
        private bool isReady = false;

        

        IFirebaseConfig config = new FirebaseConfig
        {
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
            pnlBotGrid.MouseClick += pnlBotGrid_MouseClick;
            this.KeyPreview = true;
            this.KeyUp += Multiplayer_KeyUp;

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
        private async void Multiplayer_Load(object sender, EventArgs e)
        {
            // 1. Lấy Key từ biến môi trường
            string testKey = Environment.GetEnvironmentVariable("FIREBASE_SECRET_KEY", EnvironmentVariableTarget.User);

            if (string.IsNullOrEmpty(testKey))
            {
                MessageBox.Show("CẢNH BÁO: Máy chưa nhận biến môi trường FIREBASE_SECRET_KEY!");
            }
            else
            {
                // QUAN TRỌNG: Gán Key vừa lấy được vào cấu hình
                config.AuthSecret = testKey;
                Console.WriteLine("Secret Key đã nạp thành công: " + testKey.Substring(0, 5) + "...");
            }

            try
            {
                // 2. Khởi tạo client với cấu hình đã có Key mới
                client = new FireSharp.FirebaseClient(config);

                if (client != null)
                {
                    lblStatus.Text = "Đã kết nối. Đang đồng bộ...";

                    // 3. Đăng ký tai nghe trước
                    ListenToRoom();

                    // 4. Lấy dữ liệu lần đầu để hiện tên đối thủ ngay lập tức
                    var response = await client.GetAsync($"Rooms/{roomID}");
                    if (response != null && response.Body != "null")
                    {
                        var currentRoom = response.ResultAs<Room>();
                        UpdatePlayerListUI(currentRoom);
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

            client.OnAsync($"Rooms/{roomID}/Turn", (sender, args, context) => {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;
                string currentTurn = args.Data.Replace("\"", "").Trim();

                this.Invoke(new Action(() => {
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
            System.Threading.Tasks.Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (isBattleStarted)
                        {
                            await CheckForNewShots();
                        }
                        await System.Threading.Tasks.Task.Delay(500);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi polling shots: {ex.Message}");
                    }
                }
            });

            System.Threading.Tasks.Task.Run(async () =>
            {
                while (true)
                {
                    try
                    {
                        if (isBattleStarted)
                        {
                            await CheckForNewResponses();
                        }
                        await System.Threading.Tasks.Task.Delay(500);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Lỗi polling responses: {ex.Message}");
                    }
                }
            });
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
                catch (Exception ex)
                {
                    // Tránh hiện lỗi JSON lên màn hình
                }
            });
        }
        private void HandleShipSunkNotification(string role, string shipName)
        {
            string opponentRole = (myRole == "Player1") ? "Player2" : "Player1";

            this.Invoke(new Action(() => {

                SyncShipSunkUI(role, shipName);


                if (role == opponentRole)
                {
                    UpdateStatusWithPriority($"TUYỆT VỜI! Bạn đã bắn hạ tàu {shipName}!", Color.Red, 3000);
                }
                else
                {
                    UpdateStatusWithPriority($"CẨN THẬN! Tàu {shipName} của bạn đã bị chìm!", Color.Red, 3000);
                }
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

                    if (shot.Attacker == myRole)
                    {
                        //System.Diagnostics.Debug.WriteLine($"Bỏ qua shot của mình: {shotId}");
                        continue;
                    }

                    if (handledShots.Contains(shotId))
                    {
                        //System.Diagnostics.Debug.WriteLine($"xử lý shot: {shotId}");
                        continue;
                    }

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

                        if (myRemainingCells == 0)
                        {
                            ShowFinalResult("DEFEAT");
                        }
                    }));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($" Lỗi CheckForNewShots: {ex.Message}");
            }
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
                            if (playerHits >= 17)
                            {
                                ShowFinalResult("VICTORY");
                            }
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
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi CheckForNewResponses: {ex.Message}");
            }
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
            catch (Exception ex) { MessageBox.Show("Loi: "+ex); }
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
            catch { /* Tránh loop vô tận */ }
        }

        private void ProcessRoomUpdate(Room roomData)
        {
            UpdatePlayerListUI(roomData);
            if (isBattleStarted)
            {
                // Nếu mình là Player1 mà Player2 biến mất, hoặc ngược lại
                bool opponentLeft = (myRole == "Player1" && roomData.Player2 == null) ||
                                    (myRole == "Player2" && roomData.Player1 == null);

                if (opponentLeft)
                {
                    isBattleStarted = false; // Dừng trận đấu
                    MessageBox.Show("Đối thủ đã rời trận đấu! Bạn sẽ được đưa về sảnh.", "Thông báo");
                    this.Invoke(new Action(() => this.Close()));
                    return;
                }
            }

            if (roomData.Player1 != null && roomData.Player2 != null)
            {
                bool p1Ready = roomData.Player1.IsReady;
                bool p2Ready = roomData.Player2.IsReady;

                System.Diagnostics.Debug.WriteLine($"[DEBUG] P1 Ready: {p1Ready} | P2 Ready: {p2Ready}");

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

        private void UpdatePlayerListUI(Room room)
        {
            if (room == null) return;
            if (this.lstPlayer.InvokeRequired)
            {
                this.lstPlayer.Invoke(new Action(() => UpdatePlayerListUI(room)));
                return;
            }

            lstPlayer.Items.Clear(); 

            if (room.Player1 != null)
            {
                string status = room.Player1.IsReady ? " [Ready]" : "";
                lstPlayer.Items.Add(room.Player1.Name + status);
            }
            if (room.Player2 != null)
            {
                string status = room.Player2.IsReady ? " [Ready]" : "";
                lstPlayer.Items.Add(room.Player2.Name + status);
            }
        }


        private async void pnlBotGrid_MouseClick(object sender, MouseEventArgs e)
        {

            if (!isBattleStarted || !isMyTurn)
            {
                System.Diagnostics.Debug.WriteLine("❌ Không được phép bắn!");
                return;
            }

            int x = e.X / CELL_SIZE;
            int y = e.Y / CELL_SIZE;


            if (x >= GRID_SIZE || y >= GRID_SIZE || botGrid[x, y] != 0)
            {
                System.Diagnostics.Debug.WriteLine("Ô này không hợp lệ hoặc đã bắn rồi!");
                return;
            }

            isMyTurn = false;
            lblStatus.Text = "Đang bắn...";

            string shotId = Guid.NewGuid().ToString();

            var shotData = new Shot
            {
                Attacker = myRole,
                X = x,
                Y = y
            };


            try
            {
                var response = await client.SetAsync($"Rooms/{roomID}/Shots/{shotId}", shotData);

            }
            catch 
            {
                //System.Diagnostics.Debug.WriteLine($" LỖI gửi Shot: {ex.Message}");
                isMyTurn = true; 
            }
        }


        private void StartBattle(string Turn)
        {
            isBattleStarted = true; // Đánh dấu game đã bắt đầu
            Array.Clear(botGrid, 0, botGrid.Length); 
            pnlDeployment.Visible = false; 
            pnlDeployment.Enabled = false; 
            pnlBotGrid.Visible = true; 
            pnlBotGrid.Enabled = true; pnlBotGrid.BringToFront(); 
            pnlBotGrid.Focus(); 
            isMyTurn = (Turn == myRole); 
            if (isMyTurn) 
            { 
                lblStatus.Text = "TRẬN ĐẤU BẮT ĐẦU! Đến lượt bạn bắn."; 
                lblStatus.ForeColor = Color.Lime; 
            } 
            else { 
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
            //System.Diagnostics.Debug.WriteLine($"[ShowHitMarker] Parent: {parent.Name}, X: {x}, Y: {y}, Type: {type}");

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

                        // Gửi giá trị true đơn giản lên node của tàu đó
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
                // Nếu đang hiện tin nhắn đặc biệt (như hạ tàu), không cho tin nhắn thường ghi đè
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
                if (pb.Enabled) // Chỉ xử lý nếu tàu này vẫn đang "sống" (Enabled = true)
                {
                    MakeGrayscaleAndSink(pb);
                    pb.Enabled = false; // Vô hiệu hóa để không hiện thông báo lần 2
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

            // Lưu logic vào mảng và dictionary tọa độ
            List<Point> coords = new List<Point>();
            for (int i = 0; i < length; i++)
            {
                int currX = isVert ? gridX : gridX + i;
                int currY = isVert ? gridY + i : gridY;

                playerGrid[currX, currY] = 1; // 1 nghĩa là có tàu
                coords.Add(new Point(currX, currY));
            }

            shipCoordinates[shipType] = coords;

            if (!placedShips.Contains(draggedShip)) placedShips.Add(draggedShip);
            lblStatus.Text = $"Đã đặt {placedShips.Count}/5 tàu.";
            ResetDrag();
        }

        private void ReturnShipToStart()
        {
            draggedShip.Parent = pnlDeployment;
            draggedShip.Location = initialShipLocations[draggedShip];
            ResetDrag();
        }

        private bool IsOverlapping(int startX, int startY, int length, bool isHorizontal)
        {
            for (int i = 0; i < length; i++)
            {
                int checkX = isHorizontal ? startX + i : startX;
                int checkY = isHorizontal ? startY : startY + i;

                if (playerGrid[checkX, checkY] == 1)
                {
                    return true; 
                }
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


        private async void btnReady_Click(object sender, EventArgs e)
        {
            if (placedShips.Count < 5)
            {
                MessageBox.Show($"Bạn mới đặt {placedShips.Count}/5 tàu. Vui lòng đặt đủ!");
                return;
            }

            btnReady.Enabled = false; // Khóa nút ngay lập tức để tránh bấm nhiều lần

            try
            {
                var data = new { IsReady = true };
                var response = await client.UpdateAsync($"Rooms/{roomID}/{myRole}", data);

                if (response.StatusCode == HttpStatusCode.OK)
                {
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

        private async void Multiplayer_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Xác nhận trước khi thoát (Tùy chọn)
            var result = MessageBox.Show("Bạn có chắc chắn muốn rời trận đấu?", "Xác nhận", MessageBoxButtons.YesNo);
            if (result == DialogResult.No)
            {
                e.Cancel = true; // Hủy việc đóng form
                return;
            }

            try
            {
                if (myRole == "Player1")
                {
                    // Nếu là chủ phòng thoát, xóa luôn cả phòng
                    await client.DeleteAsync($"Rooms/{roomID}");
                }
                else if (myRole == "Player2")
                {
                    // Nếu là khách thoát, chỉ xóa thông tin Player2 để người khác có thể vào
                    await client.DeleteAsync($"Rooms/{roomID}/Player2");

                    // Cập nhật lại trạng thái phòng về "waiting" nếu cần
                    await client.SetAsync($"Rooms/{roomID}/Status", "readying");
                }
            }
            catch (Exception ex)
            {
                // Có thể bỏ qua lỗi khi đang đóng form
                Console.WriteLine("Lỗi khi rời phòng: " + ex.Message);
            }
        }
    }
}