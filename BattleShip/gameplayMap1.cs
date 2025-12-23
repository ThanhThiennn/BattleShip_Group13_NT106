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

        private const int CELL_SIZE = 40; 
        private const int GRID_SIZE = 11; 

        private Dictionary<Control, Point> initialShipLocations = new Dictionary<Control, Point>();
        private Dictionary<Control, bool> isVertical = new Dictionary<Control, bool>();
        private bool isSetupComplete = false; 
        private bool isReady = false; 
        private List<Control> placedShips = new List<Control>();
        public gameplayMap1()
        {
            InitializeComponent();

            this.UpdateStyles();
            if (pnlGameGrid != null)
            {
                pnlGameGrid.Paint += pnlGameGrid_Paint;
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

                    // Tàu đã được đặt, có thể khóa khả năng kéo lại
                    // draggedShip.Enabled = false; 
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

        }

        private void btnRandom_Click(object sender, EventArgs e)
        {
            const int TOTAL_SHIPS_TO_PLACE = 5; // Hoặc số lượng tàu thực tế của bạn

            if (placedShips.Count < TOTAL_SHIPS_TO_PLACE)
            {
                MessageBox.Show("Vui lòng đặt tất cả các tàu trước khi sẵn sàng!", "Chưa Hoàn tất");
                return;
            }

            // Nếu tất cả tàu đã đặt hợp lệ
            isSetupComplete = true;
            isReady = true;

            // 1. Khóa UI đặt tàu
            DisableSetupControls();

            // 2. Thay đổi giao diện nút
            btnReady.Text = "READY!";
            btnReady.Enabled = false;

            // 3. (Giai đoạn Mạng): Gửi tín hiệu "Ready" lên Server/Database
            // Ví dụ: await FirebaseService.UpdateMatchStatusAsync(SessionManager.CurrentUserID, true);

            MessageBox.Show("Đã sẵn sàng. Đang chờ đối thủ...", "Sẵn sàng");

            // NOTE: SẼ ĐƯỢC KÍCH HOẠT KHI CẢ HAI NGƯỜI CHƠI ĐỀU SẴN SÀNG
            // CheckBothPlayersReady();
        }
        private void DisableSetupControls()
        {
            // Khóa tất cả các tàu để người chơi không thể di chuyển chúng nữa
            foreach (var ship in placedShips)
            {
                ship.Enabled = false;
                // Bỏ gán sự kiện chuột (Tùy chọn)
                // ship.MouseDown -= ship_MouseDown;
            }
            // Ẩn/Khóa pnlDeployment
            pnlDeployment.Enabled = false;
        }
    }
}
