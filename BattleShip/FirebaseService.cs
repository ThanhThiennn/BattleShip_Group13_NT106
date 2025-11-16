using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Auth;
using Firebase.Database;

namespace BattleShip
{
    public static class FirebaseConfig
    {
        public const string ApiKey = "AIzaSyDGPy01-_XkcQNozJ-VTNWtP3xvrY8Nwqo";

        public const string DatabaseUrl = "https://battleshiponline-35ac2-default-rtdb.asia-southeast1.firebasedatabase.app";
    }
    public class FirebaseService
    {
        //Login/ Register / ForgetPass
        public static FirebaseAuthProvider authProvider;

        // Đối tượng dùng cho thao tác dữ liệu game Realtime Database
        public static FirebaseClient firebaseClient;

        /// <summary>
        /// Khởi tạo kết nối đến Firebase.
        /// Hàm này chỉ cần gọi MỘT LẦN khi ứng dụng khởi động.
        /// </summary>
        public static void Initialize()
        {
            authProvider = new FirebaseAuthProvider(new Firebase.Auth.FirebaseConfig(FirebaseConfig.ApiKey));
            firebaseClient = new FirebaseClient(FirebaseConfig.DatabaseUrl);
        }
    }
}
