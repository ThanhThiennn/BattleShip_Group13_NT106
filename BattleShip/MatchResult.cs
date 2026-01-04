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
    public partial class MatchResult : Form
    {
        public MatchResult(string result, int totalShots, string opponentName)
        {
            InitializeComponent();

            if (result == "VICTORY")
            {
                lblStatus.Text = "CHIẾN THẮNG!";
                lblStatus.ForeColor = Color.White;
                this.Text = "Chinh phạt tiếp thôi";
            }
            else
            {
                lblStatus.Text = "THẤT BẠI";
                lblStatus.ForeColor = Color.White;
                this.Text = "Nháp thôi, cùng làm lại nào";
            }
            double accura = Math.Round((17.0 / totalShots) * 100, 1);

            lblOpponent.Text = opponentName;
            lblTotalShots.Text = totalShots.ToString();
            lblAccuracy.Text = accura.ToString() +"%";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
