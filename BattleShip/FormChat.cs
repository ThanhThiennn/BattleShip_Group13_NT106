using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Net;

namespace BattleShip
{
    public partial class FormChat : Form
    {
        Socket client;
        IPEndPoint IP;
        Thread receiveThread;

        // Hàm kết nối
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            client.Connect(IP);
            // Chạy một luồng riêng để đợi nhận tin nhắn mà không làm treo UI
            receiveThread = new Thread(Receive);
            receiveThread.IsBackground = true;
            receiveThread.Start();
        }
        public FormChat()
        {
            InitializeComponent();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string message = txtMessage.Text;
            if (!string.IsNullOrEmpty(message))
            {
                // Thêm prefix CHAT: để bên kia phân biệt
                Send("CHAT:" + message);
                AddMessage("Me: " + message); // Hiển thị lên màn hình của mình
                txtMessage.Clear();
            }
        }
        void Send(string s)
        {
            if (client != null && client.Connected)
            {
                client.Send(Encoding.UTF8.GetBytes(s));
            }
            else
            {
                MessageBox.Show("Chưa kết nối đến đối thủ!");
            }
        }
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);

                    string message = Encoding.UTF8.GetString(data).TrimEnd('\0');

                    // Kiểm tra xem là tin nhắn chat hay dữ liệu game
                    if (message.StartsWith("CHAT:"))
                    {
                        string actualMsg = message.Replace("CHAT:", "");
                        AddMessage("Opponent: " + actualMsg);
                    }
                    else if (message.StartsWith("ATTACK:"))
                    {
                        // Xử lý logic bắn tàu ở đây
                    }
                }
            }
            catch { Close(); }
        }
        void AddMessage(string s)
        {
            rtbChatLog.Invoke(new MethodInvoker(delegate () {
                rtbChatLog.AppendText(s + Environment.NewLine);
                rtbChatLog.ScrollToCaret(); // Tự động cuộn xuống cuối
            }));
        }

        private void rtbChatLog_TextChanged(object sender, EventArgs e)
        {
            rtbChatLog.SelectionStart = rtbChatLog.Text.Length;
            rtbChatLog.ScrollToCaret();
        }
    }
}
