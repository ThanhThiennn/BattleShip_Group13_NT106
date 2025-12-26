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
    public partial class RoomCode : Form
    {
        public string RoomID { get; private set; }
        public RoomCode()
        {
            InitializeComponent();
        }

        private void btnJoin_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(tbRoomCode.Text))
            {
                this.RoomID = tbRoomCode.Text.Trim();
                this.DialogResult = DialogResult.OK; 
                this.Close();
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã phòng!");
            }
        }

        private void tbRoomCode_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
