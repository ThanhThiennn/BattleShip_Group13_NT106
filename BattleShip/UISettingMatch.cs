using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class UISettingMatch : Form
    {
        public UISettingMatch()
        {
            InitializeComponent();
            SetTimeLimitState(false);
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }
        private readonly Color COLOR_SELECTED = Color.FromArgb(50, 150, 255); 
        private readonly Color COLOR_DESELECTED = Color.FromArgb(220, 220, 220);
        private void SetTimeLimitState(bool isEnabled)
        {
            // Cập nhật giao diện (Đổi màu nút)
            if (isEnabled)
            {
                // Trạng thái Enabled được chọn
                btnEnabled.FillColor = COLOR_SELECTED;
                btnDisabled.FillColor = COLOR_DESELECTED;
            }
            else
            {
                // Trạng thái Disabled được chọn
                btnEnabled.FillColor = COLOR_DESELECTED;
                btnDisabled.FillColor = COLOR_SELECTED;
            }
        }
        private void btnDisabled_Click(object sender, EventArgs e)
        {
            SetTimeLimitState(false);
        }

        private void btnEnabled_Click(object sender, EventArgs e)
        {
            SetTimeLimitState(true);
        }

        private void btnTurnTimePlus_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtTurnTimeValue.Text, out int currentValue))
            {
                // Kiểm tra giới hạn tối đa (Max 60)
                if (currentValue < 60)
                {
                    // Tăng giá trị 5 đơn vị
                    txtTurnTimeValue.Text = (currentValue + 5).ToString();
                }
            }
        }

        private void btnTurnTimeMinus1_Click(object sender, EventArgs e)
        {
            // Thử chuyển đổi giá trị Text sang số nguyên
            if (int.TryParse(txtTurnTimeValue.Text, out int currentValue))
            {
                // Kiểm tra giới hạn tối thiểu (Min 5)
                if (currentValue > 5)
                {
                    // Giảm giá trị 5 đơn vị
                    txtTurnTimeValue.Text = (currentValue - 5).ToString();
                }
            }
        }

        private void btnPlacementTimePlus_Click(object sender, EventArgs e)
        {
            // Thử chuyển đổi giá trị Text sang số nguyên
            if (int.TryParse(txtPlacementTimeValue.Text, out int currentValue))
            {
                // Kiểm tra giới hạn tối đa (Max 180)
                if (currentValue < 180)
                {
                    // Tăng giá trị 5 đơn vị
                    txtPlacementTimeValue.Text = (currentValue + 5).ToString();
                }
            }
        }

        private void btnPlacementTimeMinus_Click(object sender, EventArgs e)
        {
            // Thử chuyển đổi giá trị Text sang số nguyên
            if (int.TryParse(txtPlacementTimeValue.Text, out int currentValue))
            {
                // Kiểm tra giới hạn tối thiểu (Min 5)
                if (currentValue > 5)
                {
                    // Giảm giá trị 5 đơn vị
                    txtPlacementTimeValue.Text = (currentValue - 5).ToString();
                }
            }
        }

        private void btnLeaveMatch_Click(object sender, EventArgs e)
        {
            FormUI nextForm = new FormUI();
            nextForm.Show();
            this.Close();
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

        private void guna2HtmlLabel4_Click(object sender, EventArgs e)
        {

        }
    }
}
