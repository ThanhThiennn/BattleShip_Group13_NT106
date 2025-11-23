using Firebase.Auth;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    //public static class FirebaseConfig
    //{
    //    public const string ApiKey = "AIzaSyDGPy01-_XkcQNozJ-VTNWtP3xvrY8Nwqo";

    //    public const string DatabaseUrl = "";
    //}
    public class FirebaseService
    {
        // Tên Biến Môi trường đã được thiết lập trong Windows
        private const string ApiKeyEnvironmentVariable = "FIREBASE_API_KEY";
        private const string DatabaseUrl = "https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app";

        // Login/ Register / ForgetPass
        public static FirebaseAuthProvider authProvider;

 
        public static FirebaseClient firebaseClient;

        /// <summary>
        /// Khởi tạo kết nối đến Firebase bằng API Key lấy từ Biến Môi trường.
        /// </summary>
        public static void Initialize()
        {
            string apiKey = Environment.GetEnvironmentVariable(ApiKeyEnvironmentVariable);

            if (string.IsNullOrEmpty(apiKey))
            {
                MessageBox.Show(
                    $"Lỗi cấu hình: Vui lòng thiết lập biến môi trường '{ApiKeyEnvironmentVariable}' với API Key Firebase của bạn.",
                    "Lỗi Bảo Mật Cấu Hình", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
                return;
            }

            authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(apiKey));
            firebaseClient = new FirebaseClient(DatabaseUrl);
        }
    }
}
