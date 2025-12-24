using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;



namespace BattleShip
{
    public partial class gameplayMap1 : Form
    {
        private Point mouseOffset;
        private Control draggedShip = null;
        private bool isDragging = false;
        private int[,] playerGrid = new int[GRID_SIZE, GRID_SIZE];

        private int[,] botGrid = new int[GRID_SIZE, GRID_SIZE];

        private const int CELL_SIZE = 40; 
        private const int GRID_SIZE = 11;

        private int playerHits = 0; 
        private int botHits = 0;    
        private const int TOTAL_SHIP_CELLS = 17; 

        private List<Ship> botShips = new List<Ship>();
        private List<Ship> playerShips = new List<Ship>();
        private List<Control> placedShips = new List<Control>();

        private Dictionary<Control, Point> initialShipLocations = new Dictionary<Control, Point>();
        private Dictionary<Control, bool> isVertical = new Dictionary<Control, bool>();
        private bool isSetupComplete = false; 
        private bool isReady = false; 
        

        public gameplayMap1()
        {
            InitializeComponent();

            this.UpdateStyles();
            if (pnlGameGrid != null)
            {
                pnlGameGrid.Paint += pnlGameGrid_Paint;
            }
            if (pnlBotGrid != null)
            {
                pnlBotGrid.Paint += pnlGameGrid_Paint;
            }

        }

        private void gameplayMap1_Load(object sender, EventArgs e)
        {

            this.KeyUp += gameplayMap1_KeyUp;
            initialShipLocations.Clear();
            

            List<Control> shipControls = new List<Control>
            {
                picCarrier,
                picBattleShip,
                picCruiser1,
                picCruiser2,
                picDestroyer,
            };

            foreach (var ship in shipControls)
            {
                initialShipLocations.Add(ship, ship.Location);

                ship.BackColor = Color.Transparent;
                if (!isVertical.ContainsKey(ship))
                {
                    isVertical.Add(ship, false);
                }
            }


            AssignDragDropEvents(shipControls);
            lblStatus.Text = "Kéo thả tàu để dàn trận!";
        }
        private void pnlGameGrid_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Pen gridPen = new Pen(Color.FromArgb(150, Color.LightGray), 1);
            gridPen.DashStyle = DashStyle.Dot;

            for (int i = 0; i <= GRID_SIZE; i++)
            {
                int coord = i * CELL_SIZE;
                g.DrawLine(gridPen, coord, 0, coord, pnlGameGrid.Height);
                g.DrawLine(gridPen, 0, coord, pnlGameGrid.Width, coord);
            }
        }


        private void ship_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                draggedShip = (Control)sender;
                mouseOffset = new Point(e.X, e.Y);
                isDragging = true;

                // 1. Chuyển đổi tọa độ tàu từ Control cha ban đầu sang tọa độ Form
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
                // Vị trí góc trên bên trái của lưới (trên Form)
                Point gridOrigin = pnlGameGrid.Location;

                // 1. Căn chỉnh tàu vào vị trí lưới gần nhất
                Point snappedLocation = SnapShipToGrid(draggedShip, gridOrigin);

                // 2. Kiểm tra luật chơi
                if (CheckPlacementRules(draggedShip, snappedLocation))
                {
                    draggedShip.Parent = pnlGameGrid;

                    draggedShip.Location = new Point(
                        snappedLocation.X - pnlGameGrid.Location.X,
                        snappedLocation.Y - pnlGameGrid.Location.Y
                    );


                    int gridX = (draggedShip.Left) / CELL_SIZE;
                    int gridY = (draggedShip.Top) / CELL_SIZE;

                    // Lấy độ dài tàu từ Tag (Bạn nhớ vào Designer đặt Tag cho mỗi tàu là 5, 4, 3, 3, 2)
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

            // 2. KIỂM TRA CHỒNG CHÉO:
            // (Bạn cần triển khai vòng lặp qua các tàu đã đặt để kiểm tra giao nhau)

            return true;
        }


        private void AssignDragDropEvents(List<Control> controls)
        {
            foreach (Control control in controls)
            {
                control.MouseDown += ship_MouseDown;
                control.MouseMove += ship_MouseMove;
                control.MouseUp += ship_MouseUp;
            }
        }
        private void gameplayMap1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.R)
            {
                if (draggedShip != null)
                {
                    RotateShip(draggedShip);

                }
            }
        }
        private void RotateShip(Control ship)
        {

            if (!isVertical.ContainsKey(ship))
            {
                isVertical.Add(ship, false);
            }

            int temp = ship.Width;
            ship.Width = ship.Height;
            ship.Height = temp;


            ((PictureBox)ship).Image.RotateFlip(RotateFlipType.Rotate90FlipNone);

            isVertical[ship] = !isVertical[ship];

            this.Refresh();
        }

        private void btnReady_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra đủ 5 tàu chưa
            const int TOTAL_SHIPS_TO_PLACE = 5;
            if (placedShips.Count < TOTAL_SHIPS_TO_PLACE)
            {
                MessageBox.Show("Vui lòng đặt tất cả các tàu trước khi sẵn sàng!", "Chưa Hoàn tất");
                return;
            }

            // 2. Thiết lập trạng thái
            isSetupComplete = true;
            isReady = true;

            // 3. Khóa các tàu trên lưới của mình
            DisableSetupControls();

            // 4. Gọi Bot dàn trận (Dữ liệu ẩn)
            GenerateBotShips();

            // 5. ĐIỀU KHIỂN GIAO DIỆN (Phần quan trọng bị thiếu)
            btnReady.Visible = false;       // Ẩn nút Ready
            if (btnRandom != null) btnRandom.Visible = false; // Ẩn nút Random (nếu có)

            // Ẩn bảng chứa tàu thừa bên trái/phải
            if (pnlDeployment != null) pnlDeployment.Visible = false;

            // HIỆN BẢNG BẮN CỦA BOT
            pnlBotGrid.Visible = true;
            pnlBotGrid.BringToFront(); // Đảm bảo nó nằm trên cùng để nhìn thấy và click được

            // 6. Thông báo trạng thái
            lblStatus.ForeColor = Color.SpringGreen;
            lblStatus.Text = "TRẬN ĐẤU BẮT ĐẦU! LƯỢT CỦA BẠN.";

            // MessageBox.Show("Bot đã dàn trận xong. Hãy bắn vào lưới bên phải!", "Chiến!");
        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            Array.Clear(playerGrid, 0, playerGrid.Length);

            // 2. Chạy logic đặt tàu ngẫu nhiên cho Player
            // (Phần này khó hơn xíu vì phải di chuyển các PictureBox tàu thực tế)
            // Tôi sẽ hướng dẫn bạn di chuyển PictureBox sau nếu bạn cần.

            // Hiện tại, hãy gọi hàm cho Bot trước để chuẩn bị "kẻ thù"
            GenerateBotShips();
            MessageBox.Show("Bot đã dàn trận xong! Đến lượt bạn.");
        }
        private void DisableSetupControls()
        {
            foreach (var ship in placedShips)
            {
                ship.Enabled = false;
            }
            pnlDeployment.Enabled = false;
        }
        private void GenerateBotShips()
        {
            botShips.Clear();
            // Xóa sạch ma trận cũ trước khi đặt mới
            Array.Clear(botGrid, 0, botGrid.Length);

            Random rand = new Random();
            // Cấu hình tên và độ dài 5 loại tàu
            var shipConfigs = new List<(string Name, int Length)> {
        ("Carrier", 5), ("Battleship", 4), ("Cruiser1", 3), ("Cruiser2", 3), ("Destroyer", 2)
    };

            foreach (var config in shipConfigs)
            {
                Ship newShip = new Ship(config.Name);
                bool placed = false;

                while (!placed)
                {
                    int x = rand.Next(0, GRID_SIZE);
                    int y = rand.Next(0, GRID_SIZE);
                    bool isVert = rand.Next(0, 2) == 0;

                    if (CanPlaceShip(botGrid, x, y, config.Length, isVert))
                    {
                        for (int i = 0; i < config.Length; i++)
                        {
                            int currX = isVert ? x : x + i;
                            int currY = isVert ? y + i : y;

                            botGrid[currX, currY] = 1; // Đánh dấu có tàu
                            newShip.Coordinates.Add(new Point(currX, currY)); // Lưu tọa độ vào Class Ship
                        }
                        botShips.Add(newShip);
                        placed = true;
                    }
                }
            }
        }

        // Kiểm tra xem vị trí ngẫu nhiên có đặt được tàu không
        private bool CanPlaceShip(int[,] grid, int x, int y, int length, bool isVertical)
        {
            for (int i = 0; i < length; i++)
            {
                int currX = isVertical ? x : x + i;
                int currY = isVertical ? y + i : y;

                // Kiểm tra xem có ngoài lưới không hoặc đã có tàu chưa
                if (currX >= GRID_SIZE || currY >= GRID_SIZE || grid[currX, currY] != 0)
                    return false;
            }
            return true;
        }

        // Đánh dấu tàu vào ma trận
        private void PlaceShipOnMatrix(int[,] grid, int x, int y, int length, bool isVertical)
        {
            for (int i = 0; i < length; i++)
            {
                int currX = isVertical ? x : x + i;
                int currY = isVertical ? y + i : y;
                grid[currX, currY] = 1; // 1 nghĩa là có tàu
            }
        }
        private void pnlBotGrid_MouseClick(object sender, MouseEventArgs e)
        {
            if (!isReady) return;
            int x = e.X / CELL_SIZE;
            int y = e.Y / CELL_SIZE;
            if (x >= GRID_SIZE || y >= GRID_SIZE) return;

            // Nếu ô này đã bắn rồi thì không làm gì cả
            if (botGrid[x, y] == 2 || botGrid[x, y] == -1) return;

            if (botGrid[x, y] == 1) // BẮN TRÚNG
            {
                botGrid[x, y] = 2; // Đánh dấu đã bắn trúng
                botHits++;
                ShowHitMarker(pnlBotGrid, x, y, "Hit");

                lblStatus.ForeColor = Color.Red;

                // KIỂM TRA XEM TRÚNG CON TÀU NÀO
                foreach (var ship in botShips)
                {
                    if (ship.Coordinates.Contains(new Point(x, y)))
                    {
                        ship.Hits++; // Tăng số ô bị trúng của con tàu đó
                        if (ship.IsSunk) // Nếu tàu này chìm hẳn
                        {
                            lblStatus.Text = $"BẠN ĐÃ ĐÁNH CHÌM TÀU {ship.Name.ToUpper()}!";
                            lblStatus.ForeColor = Color.Yellow;

                            // GỌI HÀM LẬT ẢNH VÀ LÀM XÁM (Dùng tên PictureBox bạn đã đặt)
                            UpdateBotShipStatusUI(ship.Name);
                        }
                        else
                        {
                            lblStatus.Text = "TRÚNG RỒI! Bắn tiếp đi!";
                        }
                        break;
                    }
                }

                if (botHits >= TOTAL_SHIP_CELLS) EndGame("BẠN ĐÃ CHIẾN THẮNG!");
            }
            else // BẮN TRƯỢT
            {
                botGrid[x, y] = -1;
                ShowHitMarker(pnlBotGrid, x, y, "Miss");
                lblStatus.ForeColor = Color.Black;
                lblStatus.Text = "TRƯỢT RỒI! Đến lượt Bot.";
                BotTurn();
            }
        }

        
        private void UpdateBotShipStatusUI(string shipName)
        {
            // Tìm PictureBox theo tên (Ví dụ: pbBotCarrier)
            string controlName = "pbBot" + shipName;

            // Tìm trong Form xem có Control nào tên như vậy không
            Control[] matches = this.Controls.Find(controlName, true);

            if (matches.Length > 0 && matches[0] is PictureBox pb)
            {
                // Gọi hàm làm xám và lật ngược mà chúng ta đã viết
                MakeGrayscaleAndSink(pb);
            }
        }
        private void UpdateShipStatusUI(string side, string shipName)
        {
            string pbName = (side == "Bot" ? "pbBot" : "pbPlayer") + shipName;
            Control[] matches = this.Controls.Find(pbName, true);

            if (matches.Length > 0 && matches[0] is PictureBox pb)
            {
                MakeGrayscale(pb);
            }
        }
        private void MakeGrayscaleAndSink(PictureBox pb)
        {
            if (pb.Image == null) return;

            // 1. Tạo một bản sao của ảnh để không làm hỏng ảnh gốc trong Resources
            Bitmap bmp = new Bitmap(pb.Image);

            // 2. Lật ngược ảnh (Flip dọc - Nhìn như tàu bị lật úp)
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);

            // 3. Tiến hành làm xám ảnh đã lật
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

        private void ShowHitMarker(Control parent, int x, int y, string type)
        {
            PictureBox marker = new PictureBox();
            marker.Size = new Size(30, 30); // Kích thước icon
            marker.SizeMode = PictureBoxSizeMode.StretchImage;
            marker.BackColor = Color.Transparent;

            // Căn giữa icon vào ô lưới 
            marker.Location = new Point(x * CELL_SIZE + 5, y * CELL_SIZE + 5);

            if (type == "Hit")
            {
                marker.Image = Properties.Resources.hit_icon;
            }
            else if (type == "Miss")
            {
                marker.Image = Properties.Resources.miss_icon; 
            }
            else
            {
                marker.BackColor = Color.FromArgb(100, Color.Orange);
            }

            marker.Enabled = false; 
            parent.Controls.Add(marker);
            marker.BringToFront();
        }
        private async void BotTurn()
        {
            await Task.Delay(1000);
            Random rand = new Random();
            bool shotValid = false;

            while (!shotValid)
            {
                int x = rand.Next(0, GRID_SIZE);
                int y = rand.Next(0, GRID_SIZE);

                if (playerGrid[x, y] == 0 || playerGrid[x, y] == 1)
                {
                    shotValid = true;
                    if (playerGrid[x, y] == 1) // BOT BẮN TRÚNG
                    {
                        playerGrid[x, y] = 2;
                        playerHits++;
                        ShowHitMarker(pnlGameGrid, x, y, "Hit");

                        lblStatus.ForeColor = Color.Red;

                        // KIỂM TRA TÀU NÀO CỦA BẠN BỊ CHÌM
                        foreach (var ship in playerShips)
                        {
                            if (ship.Coordinates.Contains(new Point(x, y)))
                            {
                                ship.Hits++;
                                if (ship.IsSunk)
                                {
                                    lblStatus.Text = $"Bot đã đánh chìm {ship.Name.ToUpper()} của bạn!";
                                    UpdatePlayerShipStatusUI(ship.Name); // Làm xám UI bên Player
                                }
                                break;
                            }
                        }

                        if (playerHits >= TOTAL_SHIP_CELLS)
                        {
                            EndGame("BOT ĐÃ CHIẾN THẮNG!");
                            return;
                        }
                        BotTurn(); // Bot bắn tiếp nếu trúng
                    }
                    else // BOT BẮN TRƯỢT
                    {
                        playerGrid[x, y] = -1;
                        ShowHitMarker(pnlGameGrid, x, y, "Miss");
                        lblStatus.ForeColor = Color.Black;
                        lblStatus.Text = "Bot bắn trượt! Đến lượt của bạn.";
                    }
                }
            }
        }
        private void MakeGrayscale(PictureBox pb)
        {
            var original = pb.Image;
            if (original == null) return;

            Bitmap grayImage = new Bitmap(original.Width, original.Height);
            using (Graphics g = Graphics.FromImage(grayImage))
            {
                // Ma trận màu để biến ảnh thành trắng đen 
                System.Drawing.Imaging.ColorMatrix colorMatrix = new System.Drawing.Imaging.ColorMatrix(
                    new float[][]
                    {
                new float[] {.3f, .3f, .3f, 0, 0},
                new float[] {.59f, .59f, .59f, 0, 0},
                new float[] {.11f, .11f, .11f, 0, 0},
                new float[] {0, 0, 0, 1, 0},
                new float[] {0, 0, 0, 0, 1}
                    });

                using (var attributes = new System.Drawing.Imaging.ImageAttributes())
                {
                    attributes.SetColorMatrix(colorMatrix);
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                        0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            pb.Image = grayImage;
            pb.Enabled = false; 
        }
        private void UpdatePlayerShipStatusUI(string shipName)
        {

            string controlName = "pbPlayer" + shipName;
            Control[] matches = this.Controls.Find(controlName, true);

            if (matches.Length > 0 && matches[0] is PictureBox pb)
            {
                MakeGrayscaleAndSink(pb);
            }
        }
        private void EndGame(string message)
        {
            isReady = false; 

            lblStatus.Text = "GAME OVER: " + message;
            lblStatus.ForeColor = Color.Yellow;
            lblStatus.Font = new Font(lblStatus.Font, FontStyle.Bold);

            MessageBox.Show(message, "Kết thúc trận đấu", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //Hiện toàn bộ tàu còn lại của Bot
            RevealBotShips();
        }

        // Hàm bổ trợ để hiện các ô tàu Bot chưa bị bắn trúng khi game kết thúc
        private void RevealBotShips()
        {
            for (int x = 0; x < GRID_SIZE; x++)
            {
                for (int y = 0; y < GRID_SIZE; y++)
                {
                    if (botGrid[x, y] == 1) // Ô có tàu nhưng chưa bị bắn
                    {
                        // Hiện dấu màu xanh dương mờ hoặc cam để phân biệt với ô đã trúng
                        ShowHitMarker(pnlBotGrid, x, y, "Reveal");
                    }
                }
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    public class Ship
    {
        public string Name { get; set; }
        public List<Point> Coordinates { get; set; } = new List<Point>();
        public int Hits { get; set; }
        public bool IsSunk => Hits >= Coordinates.Count;
        public Ship(string name) { Name = name; }
    }
}
