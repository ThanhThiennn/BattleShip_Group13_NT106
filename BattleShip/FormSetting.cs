using System;
using System.Drawing;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace BattleShip
{
    public partial class FormSetting : Form
    {
        private readonly Color DefaultFillColor = Color.FromArgb(54, 63, 71);
        private readonly Color ActiveFillColor = Color.FromArgb(47, 54, 61);
        // Chỉ giữ lại các mục bạn có nút và User Control
        private enum SettingSection { MyAccount, About }

        public FormSetting()
        {
            InitializeComponent();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.PerformAutoScale();
            // Tải My Account mặc định
            LoadSectionContent(SettingSection.MyAccount, btnMyAccount);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // KHU VỰC QUẢN LÝ HIGHLIGHT VÀ TẢI NỘI DUNG
        private void ResetButtonColors()
        {
            btnMyAccount.FillColor = DefaultFillColor;
            btnAbout.FillColor = DefaultFillColor;
        }

        private void HighlightButton(Guna2Button button)
        {
            ResetButtonColors();
            button.FillColor = ActiveFillColor;
        }

        

        private void LoadSectionContent(SettingSection section, Guna2Button activeButton)
        {
            PanelRight.Controls.Clear();
            Control contentControl = null;

            switch (section)
            {
                case SettingSection.MyAccount:
                    contentControl = new UIMyAccount();
                    break;
                case SettingSection.About:
                    contentControl = new UIAbout();
                    break;
                    
            }

            if (contentControl != null)
            {
                contentControl.Dock = DockStyle.Fill;
                PanelRight.Controls.Add(contentControl);
                contentControl.BringToFront();
            }

            HighlightButton(activeButton);
        }

        private void btnMyAccount_Click(object sender, EventArgs e)
        {
            LoadSectionContent(SettingSection.MyAccount, (Guna2Button)sender);
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            LoadSectionContent(SettingSection.About, (Guna2Button)sender);
        }

    }
}