using Firebase.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BattleShip
{
    internal static class Program
    {
        // Enum mới để xác định loại Form cần hiển thị (tránh tạo Form trên luồng nền)
        private enum StartupFormType { Login, MainUI }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FirebaseService.Initialize();

         
            StartupFormType formType = Task.Run(async () =>
            {
                return await GetStartupFormTypeAsync();
            }).Result;

            Form startForm;

            if (formType == StartupFormType.MainUI)
            {
                startForm = new Lobby();
            }
            else
            {
                startForm = new Login();
            }

            Application.Run(startForm);
        }

        private static async Task<StartupFormType> GetStartupFormTypeAsync()
        {
            string refreshToken = Properties.Settings.Default.FirebaseRefreshToken;

            if (!string.IsNullOrEmpty(refreshToken))
            {
                try
                {
                    IFirebaseAuthProvider authProvider = FirebaseService.authProvider;

                    // Sử dụng Refresh Token để lấy phiên mới
                    FirebaseAuthLink authLink = await authProvider
                        .SignInWithCustomTokenAsync(refreshToken);
                    string savedEmail = Properties.Settings.Default.UserEmail;

                    SessionManager.SetSession(authLink.User.LocalId, authLink.FirebaseToken, savedEmail);

                    Properties.Settings.Default.FirebaseRefreshToken = authLink.RefreshToken;
                    Properties.Settings.Default.Save();

                    // TRẢ VỀ KIỂU FORM (KHÔNG TẠO FORM)
                    return StartupFormType.MainUI;
                }
                catch (FirebaseAuthException)
                {
                    // Xóa token và về Form Login
                    Properties.Settings.Default.FirebaseRefreshToken = null;
                    Properties.Settings.Default.Save();
                    return StartupFormType.Login;
                }
            }
            else
            {
                return StartupFormType.Login;
            }
        }
    }
}
