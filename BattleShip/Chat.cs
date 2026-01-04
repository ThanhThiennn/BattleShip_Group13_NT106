using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;

namespace BattleShip
{

    public partial class Chat : Form
    {
        IFirebaseClient client;
        string roomID;
        string myRole;
        public Chat(string id, string role, IFirebaseClient sharedClient)
        {
            InitializeComponent();
            this.roomID = id;
            this.myRole = role;
            this.client = sharedClient;

            this.Load += Chat_Load;
        }
        private void Chat_Load(object sender, EventArgs e)
        {
            //Nghe tin nhắn
            ListenForMessages();
            txtMessage.KeyDown += (s, ev) => {
                if (ev.KeyCode == Keys.Enter)
                {
                    ev.SuppressKeyPress = true;
                    btnSend.PerformClick();
                }
            };
        }

        private async void ListenForMessages()
        {
            // Lắng nghe sự thay đổi của cả nhánh Messages (là phần Ohk...)
            await client.OnAsync($"Rooms/{roomID}/Messages", (sender, args, context) => {
                if (string.IsNullOrEmpty(args.Data) || args.Data == "null") return;

                this.Invoke(new Action(() => {
                    try
                    {
                        // Kiểm tra xem Data có phải là một chuỗi JSON hợp lệ
                        if (args.Data.Trim().StartsWith("{"))
                        {
                            AppendToChatLog(args.Data);
                        }
                        else
                        {
                            RefreshChatLog();
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Lỗi nhận tin: " + ex.Message);
                    }
                }));
            });
        }

        private async void RefreshChatLog()
        {
            FirebaseResponse response = await client.GetAsync($"Rooms/{roomID}/Messages");
            if (response != null && response.Body != "null")
            {
                AppendToChatLog(response.Body);
            }
        }

        private void AppendToChatLog(string jsonData)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(jsonData)) return;

                // Thử parse theo dạng Dictionary vì Firebase lưu kèm mã ID ngẫu nhiên (-Ohk...)
                var allMessages = JsonConvert.DeserializeObject<Dictionary<string, ChatMessage>>(jsonData);

                if (allMessages != null && allMessages.Count > 0)
                {
                    // Xóa nội dung cũ để nạp lại toàn bộ danh sách mới nhất
                    rtbChatLog.Clear();
                    foreach (var item in allMessages.Values)
                    {
                        RenderSingleMessage(item);
                    }
                }
                else
                {
                    // Nếu Firebase chỉ trả về 1 đối tượng duy nhất (không bọc trong ID)
                    var singleMsg = JsonConvert.DeserializeObject<ChatMessage>(jsonData);
                    if (singleMsg != null && !string.IsNullOrEmpty(singleMsg.content))
                    {
                        RenderSingleMessage(singleMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi hiển thị: " + ex.Message);
            }
        }

        private void RenderSingleMessage(ChatMessage msg)
        {
            if (msg == null || string.IsNullOrEmpty(msg.content)) return;
            bool isMe = (msg.sender == myRole);

            rtbChatLog.SelectionStart = rtbChatLog.TextLength;
            rtbChatLog.SelectionAlignment = isMe ? HorizontalAlignment.Right : HorizontalAlignment.Left;

            rtbChatLog.SelectionColor = isMe ? Color.White : Color.Black;
            rtbChatLog.SelectionFont = new Font(rtbChatLog.Font, FontStyle.Bold);

            rtbChatLog.AppendText($"{(isMe ? "Bạn" : "Đối thủ")} ({msg.time}):\n");
            rtbChatLog.SelectionFont = new Font(rtbChatLog.Font, FontStyle.Regular);
            rtbChatLog.AppendText(msg.content + "\n\n");

            rtbChatLog.ScrollToCaret();
        }

        private async void btnSend_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtMessage.Text)) return;

            var msg = new ChatMessage
            {
                sender = myRole,
                content = txtMessage.Text,
                time = DateTime.Now.ToString("HH:mm")
            };

            try
            {
                // Đẩy vào nhánh Messages bên trong RoomID cụ thể
                await client.PushAsync($"Rooms/{roomID}/Messages", msg);
                txtMessage.Clear();
                txtMessage.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Không gửi được tin nhắn: " + ex.Message);
            }
        }
    }
    public class ChatMessage
    {
        public string sender { get; set; }
        public string content { get; set; }
        public string time { get; set; }
    }
}

