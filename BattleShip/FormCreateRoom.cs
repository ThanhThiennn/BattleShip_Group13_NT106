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
    public partial class FormCreateRoom : Form
    {
        public string RoomName { get; private set; }
        public string RoomID { get; private set; }
        public FormCreateRoom()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            // 1. Kiểm tra mã phòng đúng 8 số
            string id = txtRoomID.Text;
            if (id.Length == 8 && id.All(char.IsDigit))
            {
                RoomName = txtRoomName.Text;
                RoomID = id;

                this.DialogResult = DialogResult.OK; // Đánh dấu là thành công
                this.Close();
            }
            else
            {
                MessageBox.Show("Mã phòng phải nhập đúng 8 ký tự số!");
            }
        }
    }
}
